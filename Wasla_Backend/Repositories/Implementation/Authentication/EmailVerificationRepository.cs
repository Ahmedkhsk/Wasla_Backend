namespace Wasla_Backend.Repositories.Implementation
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly Context _context;
        public EmailVerificationRepository(Context context)
        {
            _context = context;
        }

        public async Task AddVerificationAsync(string email, string code, TimeSpan validFor)
        {
            var verification = new EmailVerification
            {
                Email = email,
                VerificationCode = code,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(validFor)
            };
            _context.EmailVerifications.Add(verification);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailVerification?> GetByEmailAndCodeAsync(string email, string code)
        {
            return await _context.EmailVerifications
                .FirstOrDefaultAsync(v => v.Email == email && v.VerificationCode == code && v.ExpiresAt > DateTime.UtcNow);
        }
        public async Task<EmailVerification?> GetVerificationAsync(string email)
        {
            return await _context.EmailVerifications
                .FirstOrDefaultAsync(v => v.Email == email && v.ExpiresAt > DateTime.UtcNow);
        }

        public async Task RemoveAsync(EmailVerification entity)
        {
            _context.EmailVerifications.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveExpiredAsync()
        {
            var expired = await _context.EmailVerifications
                .Where(v => v.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            _context.EmailVerifications.RemoveRange(expired);
            await _context.SaveChangesAsync();
        }
    }

}
