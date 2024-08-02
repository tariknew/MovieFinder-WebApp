using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserChangeEmailRequest
    {
        [Required(ErrorMessage = "IdentityUserId is a Required field")]
        public string IdentityUserId { get; set; }
        [Required(ErrorMessage = "Email is a Required field")]
        public string Email { get; set; }
    }
}
