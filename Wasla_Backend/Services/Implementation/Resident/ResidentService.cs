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
        private readonly IBookingRepository _bookingRepository;

        public ResidentService(
            IResidentRepository ResidentRepository,
            IResidentIdentityRepository ResidentIdentityRepository,
            IMapper mapper,
            IStringLocalizer<ResidentService> localizer,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            IBookingRepository bookingRepository
        )
        {
            _ResidentRepository = ResidentRepository;
            _ResidentIdentityRepository = ResidentIdentityRepository;
            _mapper = mapper;
            _localizer = localizer;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _bookingRepository = bookingRepository;
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
        }

        public async Task EditProfile(EditProfileDto editProfileDto)
        {
            var user = await _ResidentRepository.GetByIdAsync(editProfileDto.id);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

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

            var bookings = await _bookingRepository.GetBookingsForResidentAsync(residentId);
            bookings = bookings.Where(b => b.bookingStatus == BookingStatus.completed).ToList();

            var dto = new ResidentChartDto
            {
                numOfBookings = bookings.Count,
                totalAmount = bookings.Sum(b => b.price),
            };

            dto.years = bookings
                .GroupBy(b => b.bookingDate.Year)
                .Select(yearGroup => new ResidentYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(b => b.bookingDate.Month)
                        .Select(monthGroup => new ResidentMonthDto
                        {
                            month = monthGroup.Key,
                            bookings = monthGroup.Count(),
                            amount = monthGroup.Sum(b => b.price)
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