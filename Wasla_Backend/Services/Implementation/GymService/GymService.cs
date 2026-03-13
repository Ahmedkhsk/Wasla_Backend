namespace Wasla_Backend.Services.Implementation.GymService
{
    public class GymService : IGymService
    {
        private readonly IGymRepository _gymRepo;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public GymService(
            IGymRepository gymRepo,
            IMapper mapper,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _gymRepo = gymRepo;
            _mapper = mapper;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<PagedResult<AllGymsDataDto>> AllGyms(int pageNumber, int pageSize)
        {
            var data = await _gymRepo.AllGyms(pageNumber, pageSize);

            data.ForEach(g =>
            {
                g.ImageUrl = _fileUrlBuilderService.GetMediaUrl(g.ImageUrl, MediaType.userImage);
            });

            return new PagedResult<AllGymsDataDto>
            {
                Data = data,
                TotalCount = await _gymRepo.CountAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task CompleteRegister(GymCompleteRegisterDto service)
        {
            var gym = await _gymRepo.GetByGmailAsync(service.gmail);
            if (gym == null)
                throw new NotFoundException(LocalizationKey.GymNotFound);

            _mapper.Map(service, gym);
            gym.FullName = service.ownerName;

            if (service.photo != null)
                gym.ProfilePhoto = await _fileService.AddFileAsync(
                    service.photo,
                    _fileUrlBuilderService.GetPath(MediaType.userImage)
                );

            if (service.photos != null && service.photos.Any())
                gym.images = await _fileService.AddFilesAsync(
                    service.photos,
                    _fileUrlBuilderService.GetPath(MediaType.gymImage)
                );

            gym.IsCompleteRegistration = true;
            await _gymRepo.SaveChangesAsync();
        }

        public async Task UpdateProfile(UpdateProfileGym dto)
        {
            var gym = await _gymRepo.GetByGmailAsync(dto.gmail);
            if (gym == null)
                throw new NotFoundException(LocalizationKey.GymNotFound);

            _mapper.Map(dto, gym);

            gym.ProfilePhoto = await _fileService.ReplaceFileAsync(
                gym.ProfilePhoto,
                dto.photo,
                _fileUrlBuilderService.GetPath(MediaType.userImage)
            );

            if (dto.photos != null && dto.photos.Any())
            {
                _fileService.DeleteFiles(gym.images, _fileUrlBuilderService.GetPath(MediaType.gymImage));
                gym.images = await _fileService.AddFilesAsync(
                    dto.photos,
                    _fileUrlBuilderService.GetPath(MediaType.gymImage)
                );
            }

            await _gymRepo.SaveChangesAsync();
        }

        public async Task<GymProfileDto> GymProfile(string id)
        {
            var gym = await _gymRepo.GetByIdAsync(id);
            if (gym == null)
                throw new NotFoundException(LocalizationKey.GymNotFound);

            var data = await _gymRepo.GymProfile(id);
            data.profilePhoto = _fileUrlBuilderService.GetMediaUrl(data.profilePhoto, MediaType.userImage);
            data.photos = data.photos
                .Select(p => _fileUrlBuilderService.GetMediaUrl(p, MediaType.gymImage))
                .ToList();

            return data;
        }
    }
}
