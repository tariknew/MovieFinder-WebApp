using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = "UserName is a Required field")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Token is a Required field")]
        public string Token { get; set; }
    }
}
