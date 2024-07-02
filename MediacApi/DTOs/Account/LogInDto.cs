using System.ComponentModel.DataAnnotations;

namespace MediacApi.DTOs.Account
{
    public class LogInDto
    {
        [Required]
        public string Email {  get; set; }
        [Required]
        public string Password { get; set; }
    }
}
