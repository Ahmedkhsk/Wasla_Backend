namespace Wasla_Backend.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task AddItem(AddMenuItemDto dto);

        Task UpdateItem(UpdateMenuItemDto dto);

        Task DeleteItem(int id);
    }
}
