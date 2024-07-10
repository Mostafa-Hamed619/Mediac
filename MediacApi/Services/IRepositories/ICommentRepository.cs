using MediacApi.Data.Entities;
using MediacApi.DTOs.Comments;

namespace MediacApi.Services.IRepositories
{
    public interface ICommentRepository
    {
        public Task AddCommentAsync(AddCommentDto comment);

        public Task<IEnumerable<Comments>> GetPostCommentsAsync(Guid postId);
    }
}
