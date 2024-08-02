using MovieShop.Database;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;
using System.Threading.Tasks;

namespace MovieShop.Services
{
    public interface IFavoriteMoviesService
    {
        Task<VMMovieFavourites> AddMovieToFavourites(AddToFavoritesRequest request);
        Task<IEnumerable<VMMovie>> GetFavouritesMovies(int userId);
        Task<IEnumerable<VMMovieFavourites>> RemoveFromFavourites(int userId, int movieId);
        Task<bool> IsMovieFavourite(int userId, int movieId);
    }
}
