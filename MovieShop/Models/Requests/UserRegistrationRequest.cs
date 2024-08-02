using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserRegistrationRequest
    {
        [Required(ErrorMessage = "Username is a Required field")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is a Required field")]
        [StringLength(100, ErrorMessage = "The Password must be at least 8 characters long", MinimumLength = 8)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is a Required field")]
        public string Email { get; set; }
    }
}
