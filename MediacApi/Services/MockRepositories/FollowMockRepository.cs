using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Account;
using MediacApi.DTOs.Followers;
using MediacApi.Services.IRepositories;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MediacApi.Services.MockRepositories
{
    public class FollowMockRepository : IFollowRepository
    {
        private readonly ihttpAccessor context;
        private readonly MediacDbContext db;

        public FollowMockRepository(ihttpAccessor context, MediacDbContext _db)
        {
            this.context = context;
            db = _db;
        }
        public async Task<bool> FollowAsync(string authorEmail)
        {
            var userId = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var author = await db.Users.FirstOrDefaultAsync(x => x.Email == authorEmail);

            if(await db.followers.AnyAsync(x => x.FollowerUserId == userId && x.FolloweeUserId == author.Id))
            {
                return false;
            }
            else
            {
                var newFollower = new Followers()
                {
                    FollowerUserId = userId,
                    FolloweeUserId = author.Id
                };

                author.FollowerCount += 1;
                await db.followers.AddAsync(newFollower);
                await db.SaveChangesAsync();

                return true;
            }
          
        }

        public async Task UnFollowAsync(string authorEmail)
        {
            var userId = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var author = await db.Users.FirstOrDefaultAsync(x => x.Email == authorEmail);

            author.FollowerCount -= 1;

            var follow = await db.followers.FirstOrDefaultAsync(x => x.FollowerUserId == userId && x.FolloweeUserId == author.Id);

            db.followers.Remove(follow);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<getFollowerDto>> getLogedInFollowersAsync()
        {
            var userId = context.GetContext().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var usersDto = new List<getFollowerDto>();
            var followers =await db.followers.Where(f => f.FolloweeUserId == userId).Select(f => f.FollowerUser).ToListAsync();

            foreach(var follower in followers)
            {
                var userDto = new getFollowerDto()
                {
                    Email = follower.Email,
                    PhotoImage = follower.PhotoImage,
                    UserName = follower.UserName,
                    Id = userId
                };

                usersDto.Add(userDto);
            }
            return usersDto;
        }
    }
}
