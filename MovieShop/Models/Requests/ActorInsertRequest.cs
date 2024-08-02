using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class ActorInsertRequest
    {
        [Required(ErrorMessage = "First Name is a Required field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is a Required field")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "PhotoString is a Required field")]
        public string PhotoString { get; set; }
        [DefaultValue(null)]
        public byte[]? Photo { get; set; }
        [Required(ErrorMessage = "BirthDate is a Required field")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "CountryID is a Required field")]
        public int CountryID { get; set; }
        [Required(ErrorMessage = "IMDbLink is a Required field")]
        public string? IMDbLink { get; set; }
        [Required(ErrorMessage = "Biography is a Required field")]
        [StringLength(683, ErrorMessage = "Biography can have a maximum of 683 characters")]
        public string? Biography { get; set; }
    }
}
