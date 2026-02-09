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
            var gym = await _gymRepo.GetByIdAsync(service.email);

            if(gym == null)
                throw new NotFoundException("GymNotFound");

            _mapper.Map<Gym>(service);

            gym.ProfilePhoto = await FileOperation.SaveFile(service.photo, _imagePath);
            
            var images = new List<string>();
            
            if (service.photos != null)
            {
                foreach (var photo in service.photos)
                {
                    var imagePath = await FileOperation.SaveFile(photo, _imagePath);
                    images.Add(imagePath);
                }
            }
            
            gym.images = images;
            await _gymRepo.AddAsync(gym);
            await _gymRepo.SaveChangesAsync();
        }
    }
}
