namespace MovieShop.Security
{
    public class AuthResult
    {
        public int? UserId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
