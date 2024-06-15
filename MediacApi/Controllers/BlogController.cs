using MediacApi.Data.Entities;
using MediacBack.DTOs.Blogs;
using MediacBack.HelperClasses;
using MediacBack.Services.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediacBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository blogRepo;
        private readonly IFileRespository fileRespository;

        public BlogController(IBlogRepository blogRepo,IFileRespository fileRespository)
        {
            this.blogRepo = blogRepo;
            this.fileRespository = fileRespository;
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
                return Ok("Blog created");
            }
        }


        [HttpGet("Get-Blogs")]
        [ActionName("Getblogs")]
        public async Task<ActionResult<IEnumerable<Blog>>> Getblogs()
        {
            var result = await blogRepo.GetAll();

            if (result == null) return NotFound("No Blogs has been Added yet");
            else return Ok(result);
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
            await blogRepo.followBlog(id);
            return Ok("Blog are following blog");
        }

        [HttpPut("Unfollow-blog/{id}")]
        public async Task<IActionResult> UnfollowBlog(Guid id)
        {
            await blogRepo.UnfollowBlog(id);
            return Ok("Blog are unfollowing blog");
        }

    }
}
