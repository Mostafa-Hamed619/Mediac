using System.Text.Json.Serialization;

namespace MediacApi.Data.Entities
{
    public class Post
    {
        public Guid Id { get; set; }

        public string PostName { get; set; } = string.Empty;

        public string firstHeader { get; set; } = string.Empty;

        public string firstBody {  get; set; } = string.Empty;

        public string secondHeader { get; set; } = string.Empty;

        public string secondBody {  get; set; } = string.Empty;

        public bool visible { get; set; }

        public string postImage {  get; set; } = string.Empty;

        public List<string> Refrences {  get; set; } = new List<string>();
        
        public Guid BlogNumber { get; set; }

        [JsonIgnore]
        public Blog Blog { get; set; }

        public string AuthorId { get; set; }

      
    }
}