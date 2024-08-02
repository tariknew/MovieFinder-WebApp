using System.ComponentModel.DataAnnotations;

namespace MovieShop.Database
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string NotificationText { get; set; }
        public DateTime DateTime { get; set; }
        public int CreatorId { get; set; }
        public int MovieID { get; set; }
    }
}
