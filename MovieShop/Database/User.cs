using MovieShop.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShop.Database
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public byte[]? ProfileImage { get; set; }

        [ForeignKey("IdentityUser")]
        public int? IdentityUserID { get; set; }
        public ApplicationUser IdentityUser { get; set; }
    }
}
