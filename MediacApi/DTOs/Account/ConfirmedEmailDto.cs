using System.ComponentModel.DataAnnotations;

namespace MediacApi.DTOs.Account
{
    public class ConfirmedEmailDto
    {
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; }
    }
}
