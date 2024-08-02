using MovieShop.Database;

namespace MovieShop.ViewModels
{
    public class VMUserNotification
    {
        public bool IsRead { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public virtual Notification Notification { get; set; }
        public string NotificationText => Notification?.NotificationText;
        public string NotificationDate => Notification?.DateTime.ToString("yyyy-MM-dd HH:mm");
        public int? MovieID => Notification?.MovieID;
    }
}
