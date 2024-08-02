namespace MovieShop.Database
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int? StatusCode { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string? StackTrace { get; set; }
    }
}
