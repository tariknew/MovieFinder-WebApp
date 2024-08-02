using Microsoft.AspNetCore.Identity;

namespace MovieShop.Security
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? RefreshToken { get; set; }
    }
}
