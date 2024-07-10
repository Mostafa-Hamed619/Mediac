using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Comments;
using MediacApi.Services.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MediacApi.Services.MockRepositories
{
    public class CommentMockRepository : ICommentRepository
    {
        private readonly MediacDbContext _db;
        private readonly ihttpAccessor context;

        public CommentMockRepository(MediacDbContext db,ihttpAccessor context)
        {
            this._db = db;
            this.context = context;
        }

        public async Task AddCommentAsync(AddCommentDto comment)
        {
            var userId = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Post =( await _db.Posts.FirstOrDefaultAsync(c => c.PostName == comment.PostName)).Id;

            Comments newComment = new Comments()
            {
                userId = userId,
                postId = Post
            };
            

            await _db.Comments.AddAsync(newComment);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comments>> GetPostCommentsAsync(Guid postId)
        {

            var Comments = await _db.Comments.Where(c => c.postId == postId).ToListAsync();

            return Comments;
        }
    }
}
