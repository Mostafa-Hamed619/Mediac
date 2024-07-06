using MediacApi.Data.Entities;

namespace MediacBack.DTOs.Posts
{
    public class getPostPagingDto
    {
        public Guid Id { get; set; }

        public string PostName { get; set; } = string.Empty;

        public string firstHeader { get; set; } = string.Empty;

        public string firstBody { get; set; } = string.Empty;

        public bool visible { get; set; }

        public string postImage { get; set; } = string.Empty;

        public string AuthorId { get; set; }
    }
}
