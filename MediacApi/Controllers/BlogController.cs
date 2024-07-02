using MediacApi.Data.Entities;
using MediacApi.DTOs.Blogs;
using MediacBack.DTOs.Blogs;
using MediacBack.HelperClasses;
using MediacBack.Services.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Security.Claims;

namespace MediacBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository blogRepo;
        private readonly IFileRespository fileRespository;
        private readonly IMemoryCache cache;
        private readonly string blogCacheKey = "BlogKey";

        public BlogController(IBlogRepository blogRepo,IFileRespository fileRespository,IMemoryCache _cache)
        {
            this.blogRepo = blogRepo;
            this.fileRespository = fileRespository;
            cache = _cache;
        }

        [HttpPost("Add-Blog")]
        public async Task<IActionResult> Addblog([FromForm] AddBlogDto model)
        {
            if(!ModelState.IsValid) { return BadRequest(ModelState); }
            else
            {
                Blog newBlog = new Blog()
                {
                    blogName = model.blogName,
                    blogDescription = model.blogDescription,
                };

                if(model.blogImage != null)
                {
                    var result = fileRespository.SaveImage(model.blogImage, Folder.BlogFolder);

                    if(result.Item1 == 1)
                    {
                        newBlog.blogImage = result.Item2;
                    }
                }

                await blogRepo.AddBlog(newBlog);
                Log.Information($"new blog is added with name {model.blogName}");
                return Ok("Blog created");
            }
        }


        [HttpGet("Get-Blogs")]
        [ActionName("Getblogs")]
        public async Task<IActionResult> Getblogs()
        {
            if (cache.TryGetValue(blogCacheKey, out IEnumerable<getBlogDto>? blogs))
            {
                Log.Information("blog found in cache");
            }
            else
            {
                Log.Information("Blogs has not been found in cache.fetching the data....");

                 blogs = await blogRepo.GetAll();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                    Priority = CacheItemPriority.Normal,
                    Size = 1
                };

                cache.Set(blogCacheKey, blogs, cacheEntryOptions);
              
            }
            if (blogs == null) return NotFound("No Blogs has been Added yet");
            else { return Ok(blogs); }
        }

        [HttpDelete("Delete-Blogs/{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var result = await blogRepo.DeleteBlog(id);
            if (result == true) return Ok("Blog deleted successfully");
            else return NotFound($"No blog with id {id}");
        }

        [HttpPut("Follow-blog/{id}")]
        public async Task<IActionResult> FollowBlog(Guid id)
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var blog = await blogRepo.getBlog(id);
            await blogRepo.followBlog(id);
            Log.Debug($"{user} has just followed blog {blog.blogName}");
            return Ok("following blog");
        }

        [HttpPut("Unfollow-blog/{id}")]
        public async Task<IActionResult> UnfollowBlog(Guid id)
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            await blogRepo.UnfollowBlog(id);
            var blog = await blogRepo.getBlog(id);
            Log.Debug($"{user} has just followed blog {blog.blogName}");
            return Ok("Blog are unfollowing blog");
        }

    }
}
