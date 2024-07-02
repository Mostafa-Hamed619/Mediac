using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MediacApi.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhotoImage { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public ICollection<Post> posts { get; set; }

        public ICollection<Subscribe> subscribes { get; set; }
    }
}
