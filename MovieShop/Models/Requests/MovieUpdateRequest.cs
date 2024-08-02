using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class MovieUpdateRequest
    {
        [Required(ErrorMessage = "Movie Title is a Required field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Release Date is a Required field")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "Duration is a Required field")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "DirectorID is a Required field")]
        public int DirectorID { get; set; }
        [Required(ErrorMessage = "Categories is a Required field")]
        public List<int> Categories { get; set; }
        [Required(ErrorMessage = "Actors is a Required field")]
        public List<Tuple<int, string>> Actors { get; set; }
        [Required(ErrorMessage = "CountryID is a Required field")]
        public int CountryID { get; set; }
        [Required(ErrorMessage = "TrailerLink is a Required field")]
        public string? TrailerLink { get; set; }
        [Required(ErrorMessage = "ImageString is a Required field")]
        public string ImageString { get; set; }
        [DefaultValue(null)]
        public byte[]? Image { get; set; }
        [Required(ErrorMessage = "StoryLine is a Required field")]
        public string StoryLine { get; set; }
        [Required(ErrorMessage = "Price is a Required field")]
        public double Price { get; set; }
    }
}
