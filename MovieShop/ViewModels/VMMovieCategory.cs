using MovieShop.Database;

namespace MovieShop.ViewModels
{
    public class VMMovieCategory
    {
        public int Id { get; set; }
        public int MovieID { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
