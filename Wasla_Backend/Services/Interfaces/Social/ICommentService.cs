using Wasla_Backend.DTOs.PaginationDTOS;

namespace Wasla_Backend.Services.Interfaces
{
    public interface ICommentService
    {
        public Task AddComment(AddCommentDto dto);
        public Task UpdateComment(UpdateCommentDto dto);
        public Task DeleteComment(int commentId);
        public Task<PagedResult<GetCommentsResponse>> GetCommentsResponsesByPostId(GetCommentDto dto);

    }
}
