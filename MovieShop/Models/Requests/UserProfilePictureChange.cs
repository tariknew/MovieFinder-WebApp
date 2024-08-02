using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserProfilePictureChange
    {
        [Required(ErrorMessage = "UserID is a Required field")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "ProfileImageString is a Required field")]
        public string ProfileImageString { get; set; }
        [DefaultValue(null)]
        public byte[]? ProfileImage { get; set; }
    }
}
