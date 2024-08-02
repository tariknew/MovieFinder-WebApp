using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShop.Database
{
    public class Movie
    {
        [Key]
        public virtual ICollection<MovieCategory> MovieCategories { get; set; }
        public virtual ICollection<Review> MovieReviews { get; set; }
        public virtual ICollection<MovieActor> MovieActors { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int Duration { get; set; }
        [ForeignKey("Director")]
        public int DirectorID { get; set; }
        public Director Director { get; set; }
        [ForeignKey("Country")]
        public int CountryID { get; set; }
        public Country Country { get; set; }
        public string? TrailerLink { get; set; }
        public byte[]? Image { get; set; }
        public string StoryLine { get; set; }
        public double Price { get; set; }
        public double? AverageScore { get; set; }
    }
}
