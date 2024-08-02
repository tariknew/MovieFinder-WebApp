using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShop.Database
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public double Score { get; set; }
        public string Comment { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        public Movie Movie { get; set; }
    }
}
