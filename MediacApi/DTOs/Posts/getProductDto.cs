

using MediacApi.Data.Entities;

namespace MediacBack.DTOs.Posts
{
    public class getProductDto
    {
        public List<Post> Posts { get; set; } = new List<Post>();

        public int Pages { get; set; }

        public int CurrentPage { get; set; }
    }
}
