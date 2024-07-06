using MediacApi.DTOs.Followers;
using MediacApi.Services.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace MediacApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowerController : ControllerBase
    {
        private readonly IFollowRepository followRepo;

        public FollowerController(IFollowRepository FollowRepo)
        {
            followRepo = FollowRepo;
        }

        [HttpPost("Follow-author")]
        public async Task<IActionResult> FollowAuthor(string authorEmail)
        {
            var result = await followRepo.FollowAsync(authorEmail);

            if (result)
            {
                return Ok("Follow process done.");
            }
            return BadRequest($"This user already follows {authorEmail}");
        }

        [HttpDelete("Unfollow-author")]
        public async Task<IActionResult> UnfollowAuthor(string authorEmail)
        {
            await followRepo.UnFollowAsync(authorEmail);

            return Ok($"UnFollow {authorEmail}");
        }

        [HttpGet("get-LogedIn-followers")]
        public async Task<ActionResult<IEnumerable<getFollowerDto>>> getLogedInFollowers()
        {
            var result = await followRepo.getLogedInFollowersAsync();

            return Ok(result);
        }
    }
}
