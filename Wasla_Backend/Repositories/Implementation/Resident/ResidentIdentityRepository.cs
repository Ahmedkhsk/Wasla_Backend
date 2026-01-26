
using Wasla_Backend.Repositories.Interfaces;

namespace Wasla_Backend.Repositories.Implementation
{
    public class ResidentIdentityRepository : GenericRepository<ResidentIdentity>, IResidentIdentityRepository
    {
        public ResidentIdentityRepository(Context context) : base(context)
        {
        }
        public async Task<ResidentIdentity> GetByNationalIDAndGmail(string NationalID,string gmail)
        {
           return await _context.residentIdentities.FirstOrDefaultAsync(r => r.NationalId == NationalID&&r.Gmail==gmail);
        }
    }
}
