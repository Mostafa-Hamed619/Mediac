using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Subscribes;
using MediacApi.Services.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MediacApi.Services.MockRepositories
{
    public class SubscribeMockRepo : ISubscribeRepo
    {
        private MediacDbContext _db;
        public SubscribeMockRepo(MediacDbContext db)
        {
            this._db = db;
        }

        public async Task AddSubscriber(Guid blogId, string userId)
        {
            Subscribe newSubscribe = new Subscribe()
            {
                BlogId = blogId,
                FollowerId = userId
            };
            await _db.subscribes.AddAsync(newSubscribe);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubscribeDetailsDto>> GetAllSubscribers(Guid BlogId)
        {
            var result = await _db.subscribes.Where(x => x.BlogId == BlogId)
                 .Join(
                _db.Users,
                s => s.FollowerId,
                u => u.Id,
                (s, u) => new SubscribeDetailsDto
                {
                    SubscribeId = s.FollowerId,
                    SubscribeEmail = u.Email,
                    SubscribeName = u.UserName,
                    SubscribePhoto = u.PhotoImage
                }
                ).ToListAsync();

            return result;
        }

        public async Task RemoveSubscriber(Guid blogId, string userId)
        {
            var Result =await _db.subscribes.FirstOrDefaultAsync(x => x.FollowerId == userId && x.BlogId == blogId);

            _db.subscribes.Remove(Result);
            await _db.SaveChangesAsync();
        }
    }
}