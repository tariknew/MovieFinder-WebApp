using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class ReviewInsertRequest
    {
        [Required(ErrorMessage = "UserID is a Required field")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "MovieID is a Required field")]
        public int MovieID { get; set; }
        [Required(ErrorMessage = "Comment is a Required field")]
        [StringLength(81, ErrorMessage = "Comment must be at least 4 characters long", MinimumLength = 4)]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Score is a Required field")]
        public double Score { get; set; }
    }
}
