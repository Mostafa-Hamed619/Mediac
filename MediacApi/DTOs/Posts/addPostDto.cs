using System.ComponentModel.DataAnnotations;

namespace MediacBack.DTOs.Posts
{
    public class addPostDto
    {
        [Required]
        public string PostName { get; set; } = string.Empty;

        [Required]
        public string firstHeader { get; set; } = string.Empty;

        [Required]
        public string firstBody { get; set; } = string.Empty;

        public string secondHeader { get; set; } = string.Empty;

        public string secondBody { get; set; } = string.Empty;

        [Required]
        public bool visible { get; set; }

       // [Required]
        public IFormFile postImage { get; set; }
        
        public List<string> Refrences { get; set; } = new List<string>();

        [Required]
        public string BlogName { get; set; }

    }
}
