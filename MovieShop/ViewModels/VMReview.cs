using MovieShop.Database;

namespace MovieShop.ViewModels
{
    public class VMReview
    {
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public double Score { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public int UserID { get; set; }
        public int MovieID { get; set; }
        public string FormattedReviewDate => ReviewDate.ToString("MMM dd, yyyy HH:mm:ss");
        public User User { get; set; }
    }
}
