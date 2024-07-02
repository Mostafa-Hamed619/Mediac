using System.ComponentModel.DataAnnotations;

namespace MediacApi.DTOs.Account
{
    public class ResetPasswordDto
    {
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string token { get; set; }

        [Required]
        public string password {  get; set; }
    }
}
