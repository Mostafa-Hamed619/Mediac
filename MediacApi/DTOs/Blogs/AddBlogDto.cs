namespace MediacBack.DTOs.Blogs
{
    public class AddBlogDto
    {
        public string blogName { get; set; } = string.Empty;

        public string blogDescription { get; set; } = string.Empty;

        public IFormFile blogImage { get; set; }

    }
}
