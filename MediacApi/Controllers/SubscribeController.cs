using MediacApi.Data.Entities;
using MediacApi.DTOs.Followers;
using MediacApi.Services.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediacApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscribeController : ControllerBase
    {
        private readonly ISubscribeRepo subscribeRepo;
        private readonly ihttpAccessor context;

        public SubscribeController(ISubscribeRepo subscribeRepo, ihttpAccessor context)
        {
            this.subscribeRepo = subscribeRepo;
            this.context = context;
        }

        [HttpGet("Get-Subscribers")]
        public async Task<IEnumerable<SubscribeDetailsDto>> getAll(Guid BlogId)
        {
            var result = await subscribeRepo.GetAllSubscribers(BlogId);
            return result;
        }

        [HttpPost("Add-Subscribe")]
        public async Task<IActionResult> addSubscribe(Guid BlogId)
        {
            string userId = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await subscribeRepo.AddSubscriber(BlogId, userId);

            return Ok("Subscribing Added");
        }

        [HttpDelete("Remove-Subscribe")]
        public async Task<IActionResult> removeSubscribe(Guid BlogId)
        {
            string userId = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await subscribeRepo.RemoveSubscriber(BlogId, userId);

            return Ok("Subscribing removed");
        }
    }
}
