using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class FavoriteMoviesService : IFavoriteMoviesService
    {
        private readonly MWSContext _context;
        private readonly IMapper _mapper;

        public FavoriteMoviesService(MWSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VMMovieFavourites> AddMovieToFavourites(AddToFavoritesRequest request)
        {
            var existingFavorite = _context.FavouriteMovies
                .FirstOrDefault(f => f.UserId == request.UserId && f.MovieId == request.MovieId);

            if (existingFavorite != null)
            {
                throw new UserException($"'Movie' with id '{request.MovieId}' is already in favourites", 404);
            }

            var newFavouriteMovie = _mapper.Map<FavouriteMovie>(request);

            _context.FavouriteMovies.Add(newFavouriteMovie);
            await _context.SaveChangesAsync();

            return _mapper.Map<VMMovieFavourites>(newFavouriteMovie);
        }


        public async Task<IEnumerable<VMMovie>> GetFavouritesMovies(int userId)
        {
            var favouriteMovies = await _context.FavouriteMovies
                .Where(fm => fm.UserId == userId)
                .Select(fm => fm.Movie).ToListAsync();

            return _mapper.Map<List<VMMovie>>(favouriteMovies);
        }



        public async Task<IEnumerable<VMMovieFavourites>> RemoveFromFavourites(int userId, int movieId)
        {
            var favoriteMovie = _context.FavouriteMovies
            .FirstOrDefault(f => f.UserId == userId && f.MovieId == movieId);

            if (favoriteMovie == null)
            {
                throw new UserException($"'Movie' with id '{movieId}' dont exist in favourites", 404);
            }

            _context.FavouriteMovies.Remove(favoriteMovie);
            await _context.SaveChangesAsync();

            var allfavouriteMovies = await _context.FavouriteMovies.ToListAsync();

            return _mapper.Map<List<VMMovieFavourites>>(allfavouriteMovies);
        }


        public async Task<bool> IsMovieFavourite(int userId, int movieId)
        {
            var favouriteMovie = await _context.FavouriteMovies
                .FirstOrDefaultAsync(fm => fm.UserId == userId && fm.MovieId == movieId);

            if (favouriteMovie != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
