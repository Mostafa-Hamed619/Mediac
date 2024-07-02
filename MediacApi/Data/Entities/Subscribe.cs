using System.Text.Json.Serialization;

namespace MediacApi.Data.Entities
{
    public class Subscribe
    {
        public string FollowerId { get; set; }
        [JsonIgnore]
        public User user { get; set; }

        public Guid BlogId { get; set; }
        [JsonIgnore]
        public Blog blog { get; set; }
    }
}
