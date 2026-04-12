

namespace Wasla_Backend.Services.Implementation.technican
{
    public class TechnicianBookingService : ITechnicianBookingService
    {
        private readonly ITechnicianBookingRepository _technicianBookingRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IResidentRepository _residentRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly DateTimeHelper _dateTimeHelper ;
        private readonly IHubContext<BookingHub> _hub;
        public TechnicianBookingService(ITechnicianBookingRepository technicianBookingRepository, IFileUrlBuilderService fileUrlBuilderService
            , IResidentRepository residentRepository, ITechnicianRepository technicianRepository, DateTimeHelper dateTimeHelper
            , IHubContext<BookingHub> hub
            )
        {
            _technicianBookingRepository = technicianBookingRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
            _residentRepository = residentRepository;
            _technicianRepository = technicianRepository;
            _dateTimeHelper = dateTimeHelper;
            _hub = hub;
        }

       
         public async Task AcceptBooking(int bookingId)
        {
            var booking = await _technicianBookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);

            await _technicianBookingRepository.ChangeBookingStatus(bookingId, TechnicianBookingStatus.Accepted);

            Hangfire.BackgroundJob.Schedule(
                () => _technicianBookingRepository.ChangeBookingStatus(bookingId, TechnicianBookingStatus.Done),
                booking.BookingDate
            );
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    booking.ResidentId,
    NotificationType.technicianAcceptBooking,
    booking.Id.ToString(),
    null,
    "en",
    null
));
        }


        public async Task CancelBooking(int bookingId, bool isResident)
        {
            var booking = await _technicianBookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);

            booking.Status = TechnicianBookingStatus.Cancelled;

            _technicianBookingRepository.Update(booking);
            await _technicianBookingRepository.SaveChangesAsync();

            if (isResident)
            {
                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                    booking.TechnicianId,
                    NotificationType.technicianCancelBooking,
                    booking.Id.ToString(),
                    null,
                    "en",
                    null
                ));
            }
            else
            {
             Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                    booking.ResidentId,
                    NotificationType.userTechnicianBookingCancelled,
                    booking.Id.ToString(),
                    null,
                    "en",
                    null
                ));
            }
        }

        public async Task<BookingDetailsForTechnicianDto> GetBookingDetailsForTechnician(int bookingId)
        {
            var IsExist = await _technicianBookingRepository.IsExist(bookingId);
            if (!IsExist)
                throw new NotFoundException(LocalizationKey.BookingNotFound);
            var result= await _technicianBookingRepository.DetailsForTechnician(bookingId);
            result.ResidentImage = _fileUrlBuilderService.GetMediaUrl(result.ResidentImage,MediaType.userImage);
            return result;
        }

        public async Task<List<TechnicianBookingOfResident>> GetByResidentIdAndSpecialization(string residentId, TechnicianSpecialty specialization)
        {
            var result= await _technicianBookingRepository.GetByResidentIdAndSpecialization(residentId, specialization);
            result.ForEach(r => r.TechnicianImage = _fileUrlBuilderService.GetMediaUrl(r.TechnicianImage, MediaType.userImage));
            return result;
        }

        public async Task RejectBooking(int bookingId)
        {
            var booking = await _technicianBookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);

            booking.Status = TechnicianBookingStatus.Rejected;

            _technicianBookingRepository.Update(booking);
            await _technicianBookingRepository.SaveChangesAsync();

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                booking.ResidentId,
                NotificationType.technicianRejectBooking,
                booking.Id.ToString(),
                null,
                "en",
                null
            ));
        }

        public async Task<int> RequestBooking(TechnicianBookingRequestDto request)
        {
            var resident =await _residentRepository.GetByIdAsync(request.ResidentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);
            var technician = await _technicianRepository.GetByIdAsync(request.TechnicianId);
            if (technician == null)
                throw new NotFoundException(LocalizationKey.TechnicianNotFound);
         var booking = new TechnicianBooking
         {
             ResidentId = request.ResidentId,
             TechnicianId = request.TechnicianId,
             Price = request.Price,
             BookingDate = request.BookingDate,
             Specialty = (TechnicianSpecialty)technician.Specialty,
             ServiceProviderType = ServiceProviderType.Technician,
             CreatedAt = _dateTimeHelper.Now,
             Status = TechnicianBookingStatus.Pending
         };
            await _technicianBookingRepository.AddAsync(booking);
            await _technicianBookingRepository.SaveChangesAsync();
            await _hub.Clients.User(technician.Id).SendAsync("TechnicianBookingRequested", booking.Id);
            var metadata = new Dictionary<string, string>
{
    { "UserName", resident.FullName }
};
            BackgroundJob.Enqueue<NotificationFunction>(
    x => x.sendNotification(
        technician.Id,
        NotificationType.technicianNewBookingRequest,
        booking.Id.ToString(),
        _fileUrlBuilderService.GetMediaUrl(resident.ProfilePhoto, MediaType.userImage),
        "en",
        metadata
    ));
            return booking.Id;

        }

        public async Task<List<TechnicianBookingOfResident>> technicianBookingOfResidents(string residentId)
        {
            var result= await _technicianBookingRepository.technicianBookingOfResidents(residentId);
            result.ForEach(r => r.TechnicianImage = _fileUrlBuilderService.GetMediaUrl(r.TechnicianImage, MediaType.userImage));
            return result;
        }

        public async Task<List<BookingDetailsForTechnicianDto>> technicianBookingOfTechnician(string technicianId)
        {
            var result= await _technicianBookingRepository.technicianBookingOfTechnician(technicianId);
            result.ForEach(r => r.ResidentImage = _fileUrlBuilderService.GetMediaUrl(r.ResidentImage, MediaType.userImage));
            return result;
        }
    }
}
