using Microsoft.AspNetCore.Mvc;
using MovieShop.Models.Requests;
using MovieShop.Services;
using System.Threading.Tasks;
using MovieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace MovieShop.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FavouriteMoviesController : ControllerBase
    {
        private readonly IFavoriteMoviesService _favoriteMoviesService;

        public FavouriteMoviesController(IFavoriteMoviesService favoriteMoviesService)
        {
            _favoriteMoviesService = favoriteMoviesService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("AddMovieToFavourites")]
        public async Task<VMMovieFavourites> AddMovieToFavourites([FromBody] AddToFavoritesRequest request)
        {
            return await _favoriteMoviesService.AddMovieToFavourites(request);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("GetFavouritesMovies")]
        public async Task<IEnumerable<VMMovie>> GetFavouritesMovies([FromQuery] int userId)
        {
            return await _favoriteMoviesService.GetFavouritesMovies(userId);
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        public async Task<IEnumerable<VMMovieFavourites>> RemoveFromFavourites([FromQuery] int userId, int movieId)
        {
            return await _favoriteMoviesService.RemoveFromFavourites(userId, movieId);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        public async Task<bool> IsMovieFavourite([FromQuery] int userId, int movieId)
        {
            return await _favoriteMoviesService.IsMovieFavourite(userId, movieId);
        }



    }
}
