using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class UserNotificationInsertRequest
    {
        [Required(ErrorMessage = "NotificationId is a Required field")]
        public int NotificationId { get; set; }
        [Required(ErrorMessage = "UserId is a Required field")]
        public int UserId { get; set; }
        public bool IsRead { get; set; }

    }
}
