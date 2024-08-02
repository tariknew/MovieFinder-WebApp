using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.Helpers;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class MovieService : BaseService<VMMovie, Movie, MovieSearchObject, MovieInsertRequest, MovieUpdateRequest>, IMovieService
    {
        public MovieService(MWSContext context, IMapper mapper) : base(context, mapper) { }
        public override IEnumerable<VMMovie> Get(MovieSearchObject? searchObject = null)
        {
            //SELECT * FROM MOVIES, NIJE IZVRSEN UPIT NAD BAZOM, NAKON TOLIST CE SE IZVRSITI. NISU KREIRANI OBJEKTI, SPAJA SE UPIT KOJI SE KASNIJE IZVRSI.
            var movies = _context.Movies
                .Include(x => x.Director)
                .Include(x => x.Country)
                .Include(x => x.MovieReviews)
                .Include(x => x.MovieCategories).ThenInclude(x => x.Category)
                .Include(x => x.MovieActors).ThenInclude(x => x.Actor).AsQueryable();

            if (!string.IsNullOrEmpty(searchObject?.Title))
            {
                movies = movies.Where(x => x.Title.Contains(searchObject.Title)); // SELECT * FROM MOVIES WHERE Title LIKE '%%'
            }
            if (searchObject?.CategoryID != null && searchObject?.CategoryID != 0)
            {
                movies = movies.Where(x => x.MovieCategories.Any(x => x.CategoryID == searchObject.CategoryID));
            }
            if (searchObject?.PriceFrom != null)
            {
                movies = movies.Where(x => x.Price >= searchObject.PriceFrom);
            }
            if (searchObject?.PriceTo != null)
            {
                movies = movies.Where(x => x.Price <= searchObject.PriceTo);
            }
            //Sorting
            if (searchObject?.SortedColumn != null)
            {
                switch (searchObject.SortedColumn)
                {
                    case "LastAddedMovies": movies = SortByLastAddedMovies(movies, searchObject.IsDescending.Value); break;
                    case "AverageScore": movies = SortByAverageScore(movies, searchObject.IsDescending.Value); break;
                }
            }
            //Server-side paging
            var itemsToSkip = (searchObject.pageNumber - 1) * searchObject.pageSize;

            var result = _mapper.Map<List<VMMovie>>(movies.Skip(itemsToSkip).Take(searchObject.pageSize).ToList());

            return result;
        }
        public double GetMovieLastPrice()
        {
            var result = _context.Movies.OrderByDescending(x => x.Price).FirstOrDefault();

            return result?.Price ?? 0.0;
        }
        public override VMMovie GetById(int id)
        {
            var movie = _context.Movies
                .Include(x => x.Director)
                .Include(x => x.Country)
                .Include(x => x.MovieCategories).ThenInclude(x => x.Category)
                .Include(x => x.MovieActors).ThenInclude(x => x.Actor).FirstOrDefault(x => x.Id == id);

            if (movie == null)
            {
                throw new UserException($"'Movie' with id '{id}' doesn't exist", 404);
            }

            return _mapper.Map<VMMovie>(movie);
        }
        public override VMMovie Insert(MovieInsertRequest request)
        {
            byte[] photoBytes = ImageHelper.ConvertImageToBytes(request.ImageString);

            request.Image = photoBytes;

            var newMovie = _mapper.Map<Movie>(request); // MAPIRAMO REQUEST U MOVIE, KLASE MORAJU IMATI IDENTICNE NAZIVE DA BI MOGAO MAPIRATI, LISTU GENDERS NECE MAPIRATI.

            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            foreach (var category in request.Categories) // Nakon sto dodamo film onda rucno dodamo id kategorije i tako cemo za ostale
            {
                var movieCategory = new MovieCategory
                {
                    CategoryID = category,
                    MovieID = newMovie.Id
                };
                _context.MovieCategories.Add(movieCategory);
            }
            foreach (var actor in request.Actors)
            {
                var actorId = actor.Key;
                var characterName = actor.Value;

                var movieActor = new MovieActor
                {
                    ActorID = actorId,
                    MovieID = newMovie.Id,
                    CharacterName = characterName
                };
                _context.MovieActors.Add(movieActor);
            }
            _context.SaveChanges();

            return _mapper.Map<VMMovie>(newMovie);
        }
        public override IEnumerable<VMMovie> Delete(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                throw new UserException($"'Movie' with id '{id}' doesn't exist", 404);
            }

            var movieCategories = _context.MovieCategories.Where(x => x.MovieID == id);
            _context.MovieCategories.RemoveRange(movieCategories);
            var movieActors = _context.MovieActors.Where(x => x.MovieID == id);
            _context.MovieActors.RemoveRange(movieActors);
            _context.SaveChanges();

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            var allMovies = _context.Movies.ToList();

            return _mapper.Map<List<VMMovie>>(allMovies);
        }
        public override VMMovie Update(int id, MovieUpdateRequest request)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                throw new UserException($"'Movie' with id '{id}' doesn't exist", 404);
            }

            byte[] photoBytes = ImageHelper.ConvertImageToBytes(request.ImageString);

            request.Image = photoBytes;

            _mapper.Map(request, movie);
            _context.SaveChanges();

            var movieCategories = _context.MovieCategories.Where(x => x.MovieID == id);
            // PRI UPDATE IZBRISEM STARE ZANROVE ZA ODREDJENI FILM I DODAM NOVI.  
            _context.MovieCategories.RemoveRange(movieCategories);
            var movieActors = _context.MovieActors.Where(x => x.MovieID == id);
            _context.MovieActors.RemoveRange(movieActors);
            // Nakon sto dodamo film onda rucno dodamo id kategorije i tako cemo za ostale
            foreach (var category in request.Categories)
            {
                var movieCategory = new MovieCategory
                {
                    CategoryID = category,
                    MovieID = movie.Id
                };
                _context.MovieCategories.Add(movieCategory);
            }
            foreach (var actor in request.Actors)
            {
                var movieActor = new MovieActor
                {
                    ActorID = actor.Item1,
                    MovieID = movie.Id,
                    CharacterName = actor.Item2
                };
                _context.MovieActors.Add(movieActor);
            }
            _context.SaveChanges();

            return _mapper.Map<VMMovie>(movie);
        }
        private IQueryable<Movie> SortByLastAddedMovies(IQueryable<Movie> query, bool isDescending)
        {
            if (isDescending)
                return query.OrderByDescending(x => x.CreationDate);
            else
                return query.OrderBy(x => x.CreationDate);
        }
        private IQueryable<Movie> SortByAverageScore(IQueryable<Movie> query, bool isDescending)
        {
            if (isDescending)
                return query.OrderByDescending(x => x.AverageScore);
            else
                return query.OrderBy(x => x.AverageScore);
        }

        public int GetMoviesCount(MovieSearchObject? searchObject = null)
        {
            var movies = _context.Movies
                .Include(x => x.MovieCategories).ThenInclude(x => x.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchObject?.Title))
            {
                movies = movies.Where(x => x.Title.Contains(searchObject.Title));
            }
            if (searchObject?.CategoryID != null && searchObject?.CategoryID != 0)
            {
                movies = movies.Where(x => x.MovieCategories.Any(x => x.CategoryID == searchObject.CategoryID));
            }
            if (searchObject?.PriceFrom != null)
            {
                movies = movies.Where(x => x.Price >= searchObject.PriceFrom);
            }
            if (searchObject?.PriceTo != null)
            {
                movies = movies.Where(x => x.Price <= searchObject.PriceTo);
            }
            //Sorting
            if (searchObject?.SortedColumn != null)
            {
                switch (searchObject.SortedColumn)
                {
                    case "LastAddedMovies": movies = SortByLastAddedMovies(movies, searchObject.IsDescending.Value); break;
                    case "AverageScore": movies = SortByAverageScore(movies, searchObject.IsDescending.Value); break;
                }
            }
            int moviesCount = movies.Count();

            return moviesCount;
        }
    }
}

