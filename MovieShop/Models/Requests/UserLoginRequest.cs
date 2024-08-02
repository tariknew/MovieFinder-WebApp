using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Username is a Required field")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is a Required field")]
        public string Password { get; set; }
    }
}
