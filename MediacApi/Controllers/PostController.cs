using Asp.Versioning;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Posts;
using MediacApi.Services.IRepositories;
using MediacBack.DTOs.Posts;
using MediacBack.HelperClasses;
using MediacBack.Services.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Core;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace MediacBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class PostController : ControllerBase
    {
        private const string PostCacheKey = "PostList";
        private readonly IPostRepository postRepo;
        private readonly IFileRespository fileRepo;
        private readonly ihttpAccessor context;
        private readonly IMemoryCache cache;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public string user { get; set; }

        public PostController(IPostRepository postRepo,IFileRespository fileRepo, ihttpAccessor context,IMemoryCache cache)
        {
            this.postRepo = postRepo;
            this.fileRepo = fileRepo;
            this.context = context;
            this.cache = cache;
        }
        [HttpGet("get-posts/{page}")]
        public async Task<IActionResult> Posts(int page)
        {
            var compositeKey = $"{PostCacheKey}-{page}";
            if(cache.TryGetValue(compositeKey, out IEnumerable<getPostPagingDto>? posts)) 
            {
                Log.Information("Posts found in cache");
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (cache.TryGetValue(compositeKey, out posts))
                    {
                        Log.Information("Posts found in cache");
                    }
                    else
                    {
                        Log.Information("Posts not found in  cahce.Fetching from database.");

                        //throw new Exception("Exception while fetching user's posts from the storage.");
                        posts = await postRepo.Paginationposts(page);

                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromSeconds(60),
                            AbsoluteExpiration = DateTime.Now.AddHours(1),
                            Priority = CacheItemPriority.Normal,
                            Size = 1
                        };

                        cache.Set(compositeKey, posts, cacheEntryOptions);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
                //and to remove the cache use 
               // cache.Remove(PostCacheKey); at any endpoint.
            }
            user = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (posts != null) 
            {
                Log.Debug($"{user} has got to see the posts of page {page}");
                return Ok(posts);
            }
            else return Ok("No posts yet");
        }

        [HttpGet("get-post/{id:guid}")]
        [ActionName("GetPost")]
        public async Task<ActionResult<Post>> GetPost(Guid Id)
        {
            user = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await postRepo.getPostAsync(Id);
            if (result != null)
            {
                Log.Debug($"{user} has see post of name {result.PostName}");
                return Ok(result);
            }
            else return NotFound($"No item with Id {Id}");
        }

        [HttpDelete("delete-post/{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid Id) 
        {
            user = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await postRepo.getPostAsync(Id);
            Log.Debug($"{user} has just deleted post of name {result.PostName}");
            await postRepo.DeletePostAsync(Id);
            return Ok("Post has been deleted");
        }

        [HttpPost("Add-post")]
        public async Task<IActionResult> AddPost([FromForm]addPostDto model)
        {
            if(!ModelState.IsValid) return BadRequest("Invalied process");
            else
            {
                user = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                Guid BlogId = await postRepo.GetBlogId(model.BlogName);
                Post newPost = new Post()
                {
                    PostName = model.PostName,
                    firstBody = model.firstBody,
                    firstHeader = model.firstHeader,
                    secondBody = model.secondBody,
                    secondHeader = model.secondHeader,
                    visible = model.visible,
                    Refrences = model.Refrences,
                    BlogNumber = BlogId,
                    AuthorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                };
                Guid Postid = new Guid();

                newPost.Id = Postid;
                
                if(model.postImage != null)
                {
                    var fileResult = fileRepo.SaveImage(model.postImage, Folder.PostFolder);
                    
                    if(fileResult.Item1 == 1)
                    {
                        newPost.postImage = fileResult.Item2;
                    }
                }

                await postRepo.AddPostAsync(newPost);
                Log.Debug($"{user} has just added new post of name {model.PostName}");
                return CreatedAtAction(nameof(GetPost), new { id = newPost.Id }, newPost);
                
            }
        }

        [HttpGet("Search-Post-Blog")]
        public async Task<ActionResult<Guid>> getId(string Name)
        {
            var result =await postRepo.Getid(Name);
            return Ok(result);
        }

        [HttpGet("Search-Post-Blog-Name")]
        public async Task<IEnumerable<PostBlogSearch>> searches(string search)
        {
            var result = await postRepo.postBlogSearches(search);

            return result;
        }
    }
}
