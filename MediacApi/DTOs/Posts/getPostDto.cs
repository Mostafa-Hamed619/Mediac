using Microsoft.Identity.Client;

namespace MediacApi.DTOs.Posts
{
    public class getPostDto
    {
        public Guid Id { get; set; }

        public string PostName { get; set; }

        public string PostImage {  get; set; }

        public string AuthorId { get; set; }

        public bool Visible { get; set; }
    }
}
