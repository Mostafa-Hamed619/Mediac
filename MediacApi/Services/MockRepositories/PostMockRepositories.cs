using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Blogs;
using MediacApi.DTOs.Posts;
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
                    (ub, p) => new {userBlog = ub , post = p}
                )
                .Join(
                    _db.Users,
                    u => u.post.AuthorId,
                    up => up.Id,
                    (u, up) => new getPostPagingDto
                    {
                        Id = u.post.Id,
                        AuthorId = u.post.AuthorId,
                        AuthorName = up.UserName,
                        AuthorImage = up.PhotoImage,
                        postImage = u.post.postImage,
                        PostName = u.post.PostName,
                        firstHeader = u.post.firstHeader,
                        firstBody = u.post.firstBody,
                        visible = u.post.visible,
                    }
                ).ToList();

            var pageSize = Math.Ceiling(posts.Count() / pageResult);
            var result = posts.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToList();

            return result;
        }

        public async Task<IEnumerable<PostBlogSearch>> postBlogSearches(string postBlog)
        {
            var result = await _db.Posts
                .Join(
                _db.Blogs,
                p => p.BlogNumber,
                b => b.Id,
                (p, b) => new { post = p, blog = b }
                ).Where(pb => pb.post.PostName == postBlog || pb.blog.blogName == postBlog)
                .Join(
                _db.Users,
                pb => pb.post.AuthorId,
                u => u.Id,
                (pb, u) => new PostBlogSearch
                {
                    BlogId = pb.blog.Id,
                    BlogDescription = pb.blog.blogDescription,
                    BlogImage = pb.blog.blogImage,
                    BlogName = pb.blog.blogName,
                    PostAuthor = u.UserName,
                    AuthorImage = u.PhotoImage,
                    Posts = pb.blog.posts != null?
                    pb.blog.posts.Select( p =>new getPostDto
                    {
                        AuthorId = p.AuthorId,
                        Id = pb.post.Id,
                        PostImage = pb.post.postImage,
                        PostName = pb.post.PostName,
                        Visible = pb.post.visible
                    }).ToList()
                    : new List<getPostDto>()
                }
                ).ToListAsync();

            return result;
        }

        public Task UpdatePostAsync(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
