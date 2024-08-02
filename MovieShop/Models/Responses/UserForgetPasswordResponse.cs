namespace MovieShop.Models.Responses
{
    public class UserForgetPasswordResponse
    {
        public string Message { get; set; }
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
