namespace MovieShop.Models.Requests
{
    public class ReviewSearchByMovieIdRequest
    {
        public int MovieID { get; set; }
        public string? UserName { get; set; }

        // Server side paging
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 3;
    }
}
