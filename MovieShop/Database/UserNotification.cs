using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShop.Database
{
    public class UserNotification
    {
        [Key]
        public int Id { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("Notification")]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
