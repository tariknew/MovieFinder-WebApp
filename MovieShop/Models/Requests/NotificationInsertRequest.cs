using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class NotificationInsertRequest
    {
        [Required(ErrorMessage = "NotificationText is a Required field")]
        public string NotificationText { get; set; }
        [Required(ErrorMessage = "CreatorId is a Required field")]
        public int CreatorId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "MovieID is a Required field")]
        public int MovieID { get; set; }
    }
}
