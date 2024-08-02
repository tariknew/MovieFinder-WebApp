using MovieShop.Database;

namespace MovieShop.ViewModels
{
    public class VMActor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime BirthDate { get; set; }
        public int CountryID { get; set; }
        public string? IMDbLink { get; set; }
        public string? Biography { get; set; }
        public Country Country { get; set; }
    }
}
