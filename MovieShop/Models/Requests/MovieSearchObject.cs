namespace MovieShop.Models.Requests
{
    public class MovieSearchObject
    {
        public string? Title { get; set; }
        public int? CategoryID { get; set; }
        public double? PriceFrom { get; set; }
        public double? PriceTo { get; set; }

        // Server side paging
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 5;

        //Sorting
        public string? SortedColumn { get; set; }
        public bool? IsDescending { get; set; }
    }
}
