namespace MovieShop.Filters
{
    public class UserException : Exception
    {
        public UserException(string message, int status = 400) : base(message)
        {
            Status = status;
        }
        public int Status { get; set; }
    }
}
