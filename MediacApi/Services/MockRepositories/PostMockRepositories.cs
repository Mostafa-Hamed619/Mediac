using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacBack.Services.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace MediacBack.Services.MockRepositories
{
    public class PostMockRepositories : IPostRepository
    {
        private readonly MediacDbContext _db;
        public PostMockRepositories(MediacDbContext db)
        {
            this._db = db;
        }

        public Task AddPostAsync(Post post, Guid BlogId)
        {
            throw new NotImplementedException();
        }

        public Task DeletePostAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> getAllPosts()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GetBlogId(string blogName)
        {
            throw new NotImplementedException();
        }

        public Task<Post> getPostAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> getPostCount()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> Paginationposts(int page)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePostAsync(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
