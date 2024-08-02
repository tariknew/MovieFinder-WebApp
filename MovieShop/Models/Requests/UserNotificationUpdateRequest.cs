using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserNotificationUpdateRequest
    {
        [Required(ErrorMessage = "NotificationId is a Required field")]
        public int NotificationId { get; set; }
        [Required(ErrorMessage = "UserId is a Required field")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "IsRead is a Required field")]
        public bool IsRead { get; set; }
    }
}
