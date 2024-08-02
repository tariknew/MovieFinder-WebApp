using MovieShop.Database;
using System.Collections.Generic;

namespace MovieShop.Models.Responses
{
    public class GetFavoritesResponse
    {
        public IEnumerable<Movie> FavoriteMovies { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}

