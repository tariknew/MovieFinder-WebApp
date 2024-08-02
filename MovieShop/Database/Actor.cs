using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShop.Database
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime BirthDate { get; set; }
        [ForeignKey("Country")]
        public int CountryID { get; set; }
        public Country Country { get; set; }
        public string? IMDbLink { get; set; }
        public string? Biography { get; set; }
    }
}
