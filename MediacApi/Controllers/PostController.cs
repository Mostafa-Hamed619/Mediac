using MediacApi.Data.Entities;
using MediacBack.DTOs.Posts;
using MediacBack.HelperClasses;
using MediacBack.Services.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Reflection.Metadata.Ecma335;

namespace MediacBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository postRepo;
        private readonly IFileRespository fileRepo;

        public PostController(IPostRepository postRepo,IFileRespository fileRepo)
        {
            this.postRepo = postRepo;
            this.fileRepo = fileRepo;
        }
        [HttpGet("get-posts/{page}")]
        public async Task<IActionResult> Posts(int page)
        {
            
            var posts = await postRepo.Paginationposts(page);
            
            //var result = await postRepo.getAllPosts();
            
            if (posts != null) return Ok(posts);
            else return Ok("No posts yet");
        }

        [HttpGet("get-post/{id}")]
        [ActionName("GetPost")]
        public async Task<ActionResult<Post>> GetPost(Guid Id)
        {
            var result = await postRepo.getPostAsync(Id);
            if (result != null) return Ok(result);
            else return NotFound($"No item with Id {Id}" );
        }

        [HttpDelete("delete-post/{id}")]
        public async Task<IActionResult> DeletePost(Guid Id) 
        {
            await postRepo.DeletePostAsync(Id);
            return Ok("Post has been deleted");
        }

        [HttpPost("Add-post")]
        public async Task<IActionResult> Post([FromForm]addPostDto model)
        {
            if(!ModelState.IsValid) return BadRequest("Invalied process");
            else
            {
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

                await postRepo.AddPostAsync(newPost,BlogId);
                //  return CreatedAtAction(nameof(GetPost), new { id = newPost.Id }, newPost);
                return Ok(newPost);
            }
        }
    }
}
