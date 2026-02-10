namespace Wasla_Backend.Services.Implementation.GymService
{
    public class GymService : IGymService
    {
        private readonly IGenericRepository<Gym> _gymRepo;
        private readonly IMapper _mapper;
        private readonly string _imagePath;

        public GymService(IGenericRepository<Gym> gymRepo , IMapper mapper, IWebHostEnvironment webHostEnvironment) 
        {
            _gymRepo = gymRepo;
            _mapper = mapper;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathGym.TrimStart('/'));
        }

        public async Task CompleteRegister(GymCompleteRegisterDto service)
        {
            var gym = await _gymRepo.GetByIdAsync(service.id);
            
            if (gym == null)
                throw new NotFoundException("GymNotFound");

            _mapper.Map(service, gym);

            if (service.photo != null)
                gym.ProfilePhoto = await FileOperation.SaveFile(service.photo, _imagePath);

            if (service.photos != null && service.photos.Any())
            {
                gym.images ??= new List<string>();

                foreach (var photo in service.photos)
                {
                    var imagePath = await FileOperation.SaveFile(photo, _imagePath);
                    gym.images.Add(imagePath);
                }
            }

            gym.IsCompleteRegistration = true;

            await _gymRepo.SaveChangesAsync();
        }
    }
}
