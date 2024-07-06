using MediacApi.Data.Entities;
using MediacApi.DTOs.Subscribes;

namespace MediacApi.DTOs.Account
{
    public class getUsersDto
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string Email {  get; set; }

        public string PhotoImage { get; set; }

        public string PhoneNumber { get; set; }

        public int FollowersCount { get; set; }
        
        public ICollection<SubscribeBlogDto> subscribes { get; set; } = new List<SubscribeBlogDto>();
    }
}
