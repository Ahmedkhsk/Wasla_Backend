

namespace Wasla_Backend.Services.Implementation.technican
{
    public class TechnicianBookingService : ITechnicianBookingService
    {
        private readonly ITechnicianBookingRepository _technicianBookingRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IResidentRepository _residentRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IDateTimeHelper _dateTimeHelper ;
        private readonly IHubContext<BookingHub> _hub;
        private readonly IUserRepository _userrepository;
        private readonly IUserAuthorizationService _userAuthorizationService;
        public TechnicianBookingService(ITechnicianBookingRepository technicianBookingRepository, IFileUrlBuilderService fileUrlBuilderService
            , IResidentRepository residentRepository, ITechnicianRepository technicianRepository, IDateTimeHelper dateTimeHelper
            , IHubContext<BookingHub> hub,IUserRepository userRepository, IUserAuthorizationService userAuthorizationService

            )
        {
            _technicianBookingRepository = technicianBookingRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
            _residentRepository = residentRepository;
            _technicianRepository = technicianRepository;
            _dateTimeHelper = dateTimeHelper;
            _hub = hub;
            _userrepository = userRepository;
            _userAuthorizationService = userAuthorizationService;
        }

       
         public async Task AcceptBooking(int bookingId)
        {
            var booking = await _technicianBookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(booking.TechnicianId);

            await _technicianBookingRepository.AcceptBookingAsync(bookingId);

            BackgroundJob.Schedule(
     () => _technicianBookingRepository.CompleteBookingAsync(bookingId),
     booking.Date
 );
            var photo=_userrepository.GetUserPhoto(booking.TechnicianId);
            photo = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    booking.ResidentId,
    NotificationType.technicianAcceptBooking,
    booking.Id.ToString(),
    photo,
    "en",
    null
));
        }


        public async Task CancelBooking(int bookingId, bool isResident)
        {
            var booking = await _technicianBookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);
            if (isResident)
                await _userAuthorizationService.CheckOwnershipByIdAsync(booking.ResidentId);
            else
                await _userAuthorizationService.CheckOwnershipByIdAsync(booking.TechnicianId);

            booking.Status = TechnicianBookingStatus.Cancelled;
            booking.baseBookingStatus = BaseBookingStatus.Cancelled;

            _technicianBookingRepository.Update(booking);
            await _technicianBookingRepository.SaveChangesAsync();

            if (isResident)
            {
                var photo = _userrepository.GetUserPhoto(booking.ResidentId);
                photo = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                    booking.TechnicianId,
                    NotificationType.technicianCancelBooking,
                    booking.Id.ToString(),
                    photo,
                    "en",
                    null
                ));
            }
            else
            {
                var photo = _userrepository.GetUserPhoto(booking.TechnicianId);
                photo = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                    booking.ResidentId,
                    NotificationType.userTechnicianBookingCancelled,
                    booking.Id.ToString(),
                    photo,
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
            await _userAuthorizationService.CheckOwnershipByIdAsync(residentId);
            var result= await _technicianBookingRepository.GetByResidentIdAndSpecialization(residentId, specialization);
            result.ForEach(r => r.TechnicianImage = _fileUrlBuilderService.GetMediaUrl(r.TechnicianImage, MediaType.userImage));
            return result;
        }

        public async Task RejectBooking(int bookingId)
        {
            var booking = await _technicianBookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(booking.TechnicianId);

            booking.Status = TechnicianBookingStatus.Rejected;
            booking.baseBookingStatus = BaseBookingStatus.Cancelled;

            _technicianBookingRepository.Update(booking);
            await _technicianBookingRepository.SaveChangesAsync();
            var photo=_userrepository.GetUserPhoto(booking.TechnicianId);
            photo = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                booking.ResidentId,
                NotificationType.technicianRejectBooking,
                booking.Id.ToString(),
                photo,
                "en",
                null
            ));
        }

        public async Task<int> RequestBooking(TechnicianBookingRequestDto request)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(request.ResidentId);
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
             price = request.Price,
             Date = request.BookingDate,
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
            await _userAuthorizationService.CheckOwnershipByIdAsync(residentId);
            var result= await _technicianBookingRepository.technicianBookingOfResidents(residentId);
            result.ForEach(r => r.TechnicianImage = _fileUrlBuilderService.GetMediaUrl(r.TechnicianImage, MediaType.userImage));
            return result;
        }

        public async Task<List<BookingDetailsForTechnicianDto>> technicianBookingOfTechnician(string technicianId)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(technicianId);
            var result= await _technicianBookingRepository.technicianBookingOfTechnician(technicianId);
            result.ForEach(r => r.ResidentImage = _fileUrlBuilderService.GetMediaUrl(r.ResidentImage, MediaType.userImage));
            return result;
        }
    }
}
