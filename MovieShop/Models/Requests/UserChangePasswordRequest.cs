using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserChangePasswordRequest
    {
        [Required(ErrorMessage = "IdentityUserId is a Required field")]
        public string IdentityUserId { get; set; }
        [Required(ErrorMessage = "CurrentPassword is a Required field")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "NewPassword is a Required field")]
        [StringLength(100, ErrorMessage = "The NewPassword must be at least 8 characters long", MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
