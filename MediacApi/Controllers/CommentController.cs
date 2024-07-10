using MediacApi.Data.Entities;
using MediacApi.DTOs.Comments;
using MediacApi.Hubs;
using MediacApi.Services.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Formats.Asn1;

namespace MediacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IHubContext<CommentHub> _hub;
        private readonly ICommentRepository commentRepo;

        public CommentController(IHubContext<CommentHub> hub, ICommentRepository CommentRepo)
        {
            this._hub = hub;
            commentRepo = CommentRepo;
        }

        [HttpPost("add-Comment")]
        public async Task<IActionResult> AddComment(AddCommentDto model)
        {
            await commentRepo.AddCommentAsync(model);
            await _hub.Clients.All.SendAsync("TransferComment");

            return Ok("Request Commpleted");
        }

        [HttpGet("get-comments/{postId}")]
        public async Task<ActionResult<IEnumerable<Comments>>> getComments(Guid postId)
        {
            var result = await commentRepo.GetPostCommentsAsync(postId);

            return Ok(result);
        }
    }
}
