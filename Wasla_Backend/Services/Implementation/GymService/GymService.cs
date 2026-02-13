namespace Wasla_Backend.Services.Implementation.GymService
{
    public class GymService : IGymService
    {
        private readonly IGymRepository _gymRepo;
        private readonly IMapper _mapper;
        private readonly string _imagePath;

        public GymService(IGymRepository gymRepo , IMapper mapper, IWebHostEnvironment webHostEnvironment) 
        {
            _gymRepo = gymRepo;
            _mapper = mapper;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathGym.TrimStart('/'));
        }

        public async Task<PagedResult<AllGymsDataDto>> AllGyms(int pageNumber, int pageSize)
        {
            var data =  await _gymRepo.AllGyms(pageNumber,pageSize);

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
                throw new NotFoundException("GymNotFound");

            _mapper.Map(service, gym);

            if (service.photo != null)
                gym.ProfilePhoto = await FileOperation.SaveFile(service.photo, _imagePath);

            if (service.photos != null && service.photos.Any())
            {
                var images = gym.images ?? new List<string>();

                foreach (var photo in service.photos)
                {
                    var imagePath = await FileOperation.SaveFile(photo, _imagePath);
                    images.Add(imagePath);
                }

                gym.images = images;  
            }

            gym.IsCompleteRegistration = true;

            await _gymRepo.SaveChangesAsync();
        }

        public async Task UpdateProfile(UpdateProfileGym dto)
        {
            var gym = await _gymRepo.GetByGmailAsync(dto.gmail);

            if (gym == null)
                throw new NotFoundException("GymNotFound");

            _mapper.Map(dto, gym);

            if (dto.photo != null)
            {
                var oldProfilePhoto = gym.ProfilePhoto;
                var newProfilePhoto = await FileOperation.SaveFile(dto.photo, _imagePath);
                gym.ProfilePhoto = newProfilePhoto;

                if (!string.IsNullOrEmpty(oldProfilePhoto))
                    FileOperation.DeleteFile(oldProfilePhoto, _imagePath);
            }

            if (dto.photos != null && dto.photos.Any())
            {
                if (gym.images != null)
                {
                    foreach (var img in gym.images)
                        FileOperation.DeleteFile(img, _imagePath);
                }

                var newImages = new List<string>();
                foreach (var photo in dto.photos)
                {
                    var savedPath = await FileOperation.SaveFile(photo, _imagePath);
                    newImages.Add(savedPath);
                }

                gym.images = newImages;
            }

            await _gymRepo.SaveChangesAsync();
        }


        public async Task<GymProfileDto> GymProfile(string id)
        {
            var gym =await _gymRepo.GetByIdAsync(id);
            if (gym == null)
                throw new NotFoundException("GymNotFound");
            return await _gymRepo.GymProfile(id);
        }
    }
}
