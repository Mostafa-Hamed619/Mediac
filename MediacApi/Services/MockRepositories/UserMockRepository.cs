using MediacApi.Data;
using MediacApi.DTOs.Account;
using MediacApi.DTOs.Subscribes;
using MediacApi.Services.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediacApi.Services.MockRepositories
{
    public class UserMockRepository : iUserRepository
    {
        private readonly MediacDbContext _db;

        public UserMockRepository(MediacDbContext db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<getUsersDto>> getAllUsers()
        {
            var users = await _db.Users.Include(u => u.subscribes).ToListAsync();

            var UsersDto = new List<getUsersDto>();

            foreach(var user  in users)
            {
                var userDto = new getUsersDto()
                {
                    Id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PhotoImage = user.PhotoImage,
                    FollowersCount = user.FollowerCount
                };

                userDto.subscribes =await _db.subscribes.Where(u => u.FollowerId == user.Id)
                    .Join(
                    _db.Blogs,
                    s => s.BlogId,
                    b => b.Id,
                    (s, b) => new SubscribeBlogDto()
                    {
                        BlogId = b.Id,
                        BlogName = b.blogName,
                        BlogImage = b.blogImage,
                        BlogDescription = b.blogDescription
                    }).ToListAsync();
                UsersDto.Add(userDto);
            }

            return UsersDto;
        }
    }
}
