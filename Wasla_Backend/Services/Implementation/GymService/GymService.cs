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

        public async Task<List<AllGymsDataDto>> AllGyms(int pageNumber, int pageSize)
        {
            return await _gymRepo.AllGyms(pageNumber,pageSize);
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


        public async Task<GymProfileDto> GymProfile(string id)
        {
            var gym =await _gymRepo.GetByIdAsync(id);
            if (gym == null)
                throw new NotFoundException("GymNotFound");
            return await _gymRepo.GymProfile(id);
        }
    }
}
