using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserRefreshTokenRequest
    {
        [Required(ErrorMessage = "UserName is a Required field")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "RefreshToken is a Required field")]
        public string RefreshToken { get; set; }
    }
}
