using MediacApi.DTOs.Posts;

namespace MediacApi.DTOs.Blogs
{
    public class getBlogDto
    {
        public Guid Id { get; set; }

        public string BlogName { get; set; }

        public string BlogsDescription { get; set; }

        public string BlogImage { get; set; }

        public bool CheckFollow {  get; set; }

        public int Follower { get; set; }

        public List<getPostDto> Post { get; set; } = new List<getPostDto>();
    }
}
