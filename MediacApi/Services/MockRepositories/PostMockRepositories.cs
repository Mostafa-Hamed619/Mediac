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

        public async Task AddPostAsync(Post post)
        {
            await _db.AddAsync(post);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Guid id)
        {
            var Post = await _db.Posts.FindAsync(id);
            _db.Posts.Remove(Post);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> getAllPosts()
        {
            var result = await _db.Posts.ToListAsync();
            return result;
        }

        public async Task<Guid> GetBlogId(string blogName)
        {
            var result = await _db.Blogs.FirstOrDefaultAsync(x => x.blogName == blogName);
            return result.Id;
        }

        public async Task<Guid> Getid(string Name)
        {
            var result = await _db.Posts.FirstOrDefaultAsync(x => x.PostName == Name);
            return result.Id;
        }

        public async Task<Post> getPostAsync(Guid id)
        {
            var Post = await _db.Posts.FirstOrDefaultAsync(x => x.Id == id);
            return Post;
        }

        public async Task<int> getPostCount()
        {
            var Count = await _db.Posts.CountAsync();
            return Count;
        }

        public async Task<IEnumerable<Post>> Paginationposts(int page)
        {
            float pageResult = 5f;

            var pageSize = Math.Ceiling(await _db.Posts.CountAsync() / pageResult);

            var result = await _db.Posts.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();

            return result;
        }

        public Task UpdatePostAsync(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
