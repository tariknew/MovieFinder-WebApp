using MovieShop.Database;
using System.Globalization;

namespace MovieShop.ViewModels
{
    public class VMMovie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int Duration { get; set; }
        public byte[]? Image { get; set; }
        public string? TrailerLink { get; set; }
        public string StoryLine { get; set; }
        public double Price { get; set; }
        public double? AverageScore { get; set; }
        public string FormattedPrice => FormatPrice(Price);
        public string FormattedReleaseDate => ReleaseDate.ToString("MMM dd, yyyy");
        public string FormattedDuration => CalculateHoursAndMinutes(Duration);
        public string CategoriesNames => string.Join(", ", MovieCategories.Select(x => x?.Category?.CategoryName).ToList().Distinct());
        public string DirectorName => $"{Director?.FirstName} {Director?.LastName}";
        public virtual ICollection<VMMovieCategory> MovieCategories { get; set; }
        public virtual ICollection<VMMovieActor> MovieActors { get; set; }
        public virtual Director Director { get; set; }

        /* Price Format */
        private static string FormatPrice(double value)
        {
            var frenchCulture = new CultureInfo("fr-FR");
            frenchCulture.NumberFormat.CurrencyPositivePattern = 0;
            frenchCulture.NumberFormat.CurrencyNegativePattern = 2;
            frenchCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            return value.ToString("C", frenchCulture);
        }
        /* Duration Format */
        private static string CalculateHoursAndMinutes(int duration)
        {
            int hours = duration / 60;
            int minutes = duration % 60;

            return $"{hours}h {minutes}m";
        }
    }
}
