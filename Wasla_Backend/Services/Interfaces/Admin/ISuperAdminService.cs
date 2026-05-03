namespace Wasla_Backend.Services.Interfaces
{
    public interface ISuperAdminService
    {
        Task AddAdminAsync(AddAdminDto dto);
        Task<IEnumerable<AdminResponseDto>> GetAllAdminsAsync();
        Task RemoveAdminAsync(string adminId);
        Task ToggleAdminStatusAsync(string adminId);
    }
}