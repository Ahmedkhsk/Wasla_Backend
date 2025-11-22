
namespace Wasla_Backend.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> RegisterAsync(RegisterDto model);
        public Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto model);
        public Task<LoginResponse> LoginAsync(LoginDto model);
        public Task<IdentityResult> ForgetPasswordAsync(ForgetPasswordDto model);
        public Task<IdentityResult> CheckMailForVerficatio(CheckMailDto model);
        public Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto model);
        public Task<IdentityResult> VerifyEmailAsync(VerificationEmailDto model);
        
        public Task approveAndVerify(string gmail);
        public Task<object> AllUsers();
        public  Task Delete(string gmail);



    }
}
