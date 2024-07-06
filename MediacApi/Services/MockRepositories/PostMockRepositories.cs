using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Blogs;
using MediacApi.Services.IRepositories;
using MediacBack.DTOs.Posts;
using MediacBack.Services.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace MediacBack.Services.MockRepositories
{
    public class PostMockRepositories : IPostRepository
    {
        private readonly MediacDbContext _db;
        private readonly ihttpAccessor context;

        public PostMockRepositories(MediacDbContext db, ihttpAccessor context)
        {
            this._db = db;
            this.context = context;
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

        public async Task<IEnumerable<getPostPagingDto>> Paginationposts(int page)
        {
            var userId = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            float pageResult = 3f;


            var userBlogs =await _db.subscribes.Where(s => s.FollowerId == userId).Select(n => n.blog).ToListAsync();
            var posts = userBlogs
                .Join(
                    _db.Posts,
                    ub => ub.Id,
                    p => p.BlogNumber,
                    (ub, p) => new getPostPagingDto
                    {
                        Id = p.Id,
                        AuthorId = p.AuthorId,
                        PostName = p.PostName,
                        postImage = p.postImage,
                        firstHeader = p.firstHeader,
                        firstBody = p.firstBody,
                        visible = p.visible
                    }
                ).ToList();

            var pageSize = Math.Ceiling(posts.Count() / pageResult);
            var result = posts.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToList();

            return result;
        }

        public Task UpdatePostAsync(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
