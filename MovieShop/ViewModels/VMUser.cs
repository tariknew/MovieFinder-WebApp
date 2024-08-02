using MovieShop.Security;

namespace MovieShop.ViewModels
{
    public class VMUser
    {
        public byte[]? ProfileImage { get; set; }
        public int? IdentityUserID { get; set; }
        public ApplicationUser IdentityUser { get; set; }
        public string? UserName => IdentityUser?.UserName;
        public string? Email => IdentityUser?.Email;
    }
}
