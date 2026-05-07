using Wasla_Backend.DTOs;

namespace Wasla_Backend.Services.Implementation
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepository _ResidentRepository;
        private readonly IResidentIdentityRepository _ResidentIdentityRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ResidentService> _localizer;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IBaseBookingRepository _bookingRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserAuthorizationService _userAuthorizationService;


        public ResidentService(
            IResidentRepository ResidentRepository,
            IResidentIdentityRepository ResidentIdentityRepository,
            IMapper mapper,
            IStringLocalizer<ResidentService> localizer,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            IBaseBookingRepository bookingRepository,
            IOrderRepository orderRepository,
            IUserAuthorizationService userAuthorizationService
        )
        {
            _ResidentRepository = ResidentRepository;
            _ResidentIdentityRepository = ResidentIdentityRepository;
            _mapper = mapper;
            _localizer = localizer;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _bookingRepository = bookingRepository;
            _orderRepository = orderRepository;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task CompleteResidentRegister(ResidentCompleteRegisterDto model)
        {
            var regex = new Regex(@"^\d{14}$");
            if (!regex.IsMatch(model.NationalId))
                throw new BadRequestException(LocalizationKey.InvalidNationalId);

            var existingIdentity = await _ResidentIdentityRepository.GetByNationalIDAndGmail(model.NationalId, model.Email);
            if (existingIdentity == null)
                throw new BadRequestException(LocalizationKey.NoUnitFound);

            var resident = await _ResidentRepository.GetByEmail(model.Email);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            _mapper.Map(model, resident);
            resident.ProfilePhoto = await _fileService.AddFileAsync(
                model.Image,
                _fileUrlBuilderService.GetPath(MediaType.userImage)
            );
            resident.IsCompleteRegistration = true;

            _ResidentRepository.Update(resident);
            await _ResidentRepository.SaveChangesAsync();
            var image = _fileUrlBuilderService.GetMediaUrl(resident.ProfilePhoto, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
             resident.Id,
             NotificationType.residentCompleteInfoScreen,
             resident.Id,
             image,
             "en",
             null
         ));
        }

        public async Task EditProfile(EditProfileDto editProfileDto)
        {
            var user = await _ResidentRepository.GetByIdAsync(editProfileDto.id);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(user.Id);

            _mapper.Map(editProfileDto, user);

            user.ProfilePhoto = await _fileService.ReplaceFileAsync(
                user.ProfilePhoto,
                editProfileDto.image,
                _fileUrlBuilderService.GetPath(MediaType.userImage)
            );

            _ResidentRepository.Update(user);
            await _ResidentRepository.SaveChangesAsync();
        }

        public async Task<ResponseProfileDto> GetProfile(string userId)
        {
            var user = await _ResidentRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            return _mapper.Map<ResponseProfileDto>(user);
        }

        public async Task<ResidentChartDto> GetResidentChartAsync(string residentId)
        {
            var resident = await _ResidentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(resident.Id);

            var bookings = await _bookingRepository.GetByResidentId(residentId);
            var orders = await _orderRepository.GetBookingPerUser(residentId);

            var allActivities = bookings
            .Concat(orders)
            .Where(x => x.Date.Year > 2000)
            .ToList();
            var dto = new ResidentChartDto
            {
                numOfBookings = allActivities.Count,
                totalAmount = allActivities.Sum(b => b.Price),
            };

            dto.years = allActivities
                .GroupBy(b => b.Date.Year)
                .Select(yearGroup => new ResidentYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(b => b.Date.Month)
                        .Select(monthGroup => new ResidentMonthDto
                        {
                            month = monthGroup.Key,
                            bookings = monthGroup.Count(),
                            amount = monthGroup.Sum(b => b.Price)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToList();

            return dto;
        }
    }
}