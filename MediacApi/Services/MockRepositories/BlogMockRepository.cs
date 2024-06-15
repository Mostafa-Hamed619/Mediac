using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacBack.Services.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediacBack.Services.MockRepositories
{
    public class BlogMockRepository : IBlogRepository
    {
        private readonly MediacDbContext db;

        public BlogMockRepository(MediacDbContext db)
        {
            this.db = db;
        }

        public async Task AddBlog(Blog blog)
        {
            await db.Blogs.AddAsync(blog);
            await db.SaveChangesAsync();
        }

        public Task<bool> DeleteBlog(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task followBlog(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Blog>> GetAll()
        {
            var result = await db.Blogs.ToListAsync();

            return result;
        }

        public Task UnfollowBlog(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
