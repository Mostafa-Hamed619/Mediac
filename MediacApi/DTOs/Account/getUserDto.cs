using MediacApi.Data.Entities;

namespace MediacApi.DTOs.Account
{
    public class getUserDto
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string PhotoImage { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<Subscribe> subscribes { get; set; }
    }
}
