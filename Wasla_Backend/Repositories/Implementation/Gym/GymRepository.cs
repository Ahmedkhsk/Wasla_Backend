

namespace Wasla_Backend.Repositories.Implementation.Gyms
{
    public class GymRepository : GenericRepository<Gym>, IGymRepository
    {
        public GymRepository(Context context) : base(context)
        {
        }

        public async Task<List<AllGymsDataDto>> AllGyms(int pageNumber, int pageSize)
        {
            return await _context.Gyms
                .OrderBy(g => g.Id) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new AllGymsDataDto
                {
                    Id = g.Id,
                    Name = g.BusinessName,
                    Description = g.Description,
                    Rating = g.Rating,
                    ImageUrl = g.ProfilePhoto
                })
                .ToListAsync();
        }


        public Task<Gym> GetByGmailAsync(string gmail)
        {
           return _context.Gyms.FirstOrDefaultAsync(g => g.Email == gmail);
        }

        public Task<GymProfileDto> GymProfile(string id)
        {
            return _context.Gyms.Where(g => g.Id == id).Select(g => new GymProfileDto
            {
                id = g.Id,
                email = g.Email,
                businessName = g.BusinessName,
                ownerName = g.OwnerName,
                description = g.Description,
                phones = g.phones,
                profilePhoto = g.ProfilePhoto,
                photos = g.images,
                ReviewsCount = g.Reviews.Count(),
                rating = g.Rating
            }).FirstOrDefaultAsync();
        }
    }
}
