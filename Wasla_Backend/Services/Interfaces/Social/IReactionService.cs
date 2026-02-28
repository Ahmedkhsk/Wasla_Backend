namespace Wasla_Backend.Services.Interfaces
{
    public interface IReactionService
    {
       public Task ToggleReaction(ToggleReactionDto dto);
    }
}
