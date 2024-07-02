using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Blogs;
using MediacApi.DTOs.Posts;
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

        public async Task<bool> DeleteBlog(Guid id)
        {
            var Blog = await db.Blogs.FindAsync(id);

            db.Blogs.Remove(Blog);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<Blog> getBlog(Guid id)
        {
            var blog = await db.Blogs.FindAsync(id);

            return blog;
        }
        public async Task followBlog(Guid id)
        {
            var Blog = await db.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            Blog.checkFollow = true;
            Blog.followers = Blog.followers + 1;
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<getBlogDto>> GetAll()
        {
            var result = await db.Blogs.Include(b=> b.posts).ToListAsync();

            var blogsDto = new List<getBlogDto>();
            foreach (var blog in result)
            {
                var blogDto = new getBlogDto()
                {
                    Id = blog.Id,
                    BlogName = blog.blogName,
                    BlogImage = blog.blogImage,
                    BlogsDescription = blog.blogDescription,
                    Follower = blog.followers,
                    CheckFollow = blog.checkFollow,

                };

                blogDto.Post = blog.posts.Select(p => new getPostDto()
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    PostImage = p.postImage,
                    PostName = p.PostName,
                    Visible = p.visible
                }).ToList();
                blogsDto.Add(blogDto);
            }
            return blogsDto;
        }

        public async Task UnfollowBlog(Guid id)
        {
            var Blog = await db.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            Blog.followers = Blog.followers - 1;
            if (Blog.followers == 0)
            {
                Blog.checkFollow = false;
            }
            await db.SaveChangesAsync();
        }
    }
}
