namespace MediacApi.DTOs.Posts
{
    public class PostBlogSearch
    {
        public Guid BlogId { get; set; }
        public string BlogName { get; set; }
        public string BlogImage { get; set; }
        public string BlogDescription {  get; set; }
        public IList<getPostDto> Posts { get; set; }
        public string PostAuthor { get; set; }
        public string AuthorImage {  get; set; }
    }
}
