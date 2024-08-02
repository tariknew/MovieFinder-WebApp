using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.Security;
using MovieShop.ViewModels;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class ReviewService : IReviewService
    {
        private readonly MWSContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public ReviewService(UserManager<ApplicationUser> userManager, MWSContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }
        public VMReview InsertReview(ReviewInsertRequest request)
        {
            var newReview = _mapper.Map<Review>(request);

            _context.Reviews.Add(newReview);
            _context.SaveChanges();

            CalculateAverageScore(request.MovieID);

            return _mapper.Map<VMReview>(newReview);
        }
        public IEnumerable<VMReview> DeleteReview(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                throw new UserException($"'Review' with id '{id}' doesn't exist", 404);
            }

            var movieId = review.MovieID;

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            CalculateAverageScore(movieId);

            var allReviews = _context.Reviews.ToList();

            return _mapper.Map<List<VMReview>>(allReviews);
        }

        public async Task<IEnumerable<VMReview>> GetReviewsByMovieId(ReviewSearchByMovieIdRequest searchObject)
        {
            var reviewsQuery = _context.Reviews.Where(x => x.MovieID == searchObject.MovieID);

            if (!string.IsNullOrEmpty(searchObject?.UserName))
            {
                // Pronalazimo korisnika na osnovu korisničkog imena
                var user = await _userManager.FindByNameAsync(searchObject.UserName);


                // Ako je korisnik pronađen, filtriramo recenzije koje pripadaju tom korisniku
                if (user != null)
                {
                    var currentUser = _context.Users.FirstOrDefault(x => x.IdentityUserID == user.Id);
                    reviewsQuery = reviewsQuery.Where(r => r.UserID == currentUser.Id);

                }
                else
                {
                    // Ako korisnik nije pronađen, vraćamo praznu listu recenzija
                    return Enumerable.Empty<VMReview>();
                }
            }

            var reviews = await reviewsQuery.OrderByDescending(x => x.ReviewDate)
                                    .ToListAsync();

            var vmreview = _mapper.Map<List<VMReview>>(reviews);

            foreach (var review in vmreview)
            {
                // Pronađi korisnika u bazi podataka da bismo dobili ProfileImage i UserName
                var user = _context.Users.Find(review.UserID);
                var currentUser = await _userManager.FindByIdAsync(user.IdentityUserID.ToString());
                if (user != null && currentUser != null)
                {
                    // Ažuriraj ProfileImage i UserName u VMReview modelu
                    review.UserName = currentUser.UserName;
                    review.ProfileImage = user.ProfileImage;
                }
            }

            //Server-side paging
            var itemsToSkip = (searchObject.pageNumber - 1) * searchObject.pageSize;

            return vmreview.Skip(itemsToSkip).Take(searchObject.pageSize);
        }
        public async Task<int> GetReviewsCountByMovieId(ReviewSearchByMovieIdRequest searchObject)
        {
            var reviewsQuery = _context.Reviews.Where(x => x.MovieID == searchObject.MovieID);

            if (!string.IsNullOrEmpty(searchObject.UserName))
            {
                var user = await _userManager.FindByNameAsync(searchObject.UserName);

                if (user != null)
                {
                    var currentUser = _context.Users.FirstOrDefault(x => x.IdentityUserID == user.Id);
                    reviewsQuery = reviewsQuery.Where(r => r.UserID == currentUser.Id);
                }
                else
                {
                    return 0;
                }
            }
            int reviewsCount = reviewsQuery.Count();

            return reviewsCount;
        }
        private void CalculateAverageScore(int movieID)
        {
            var movie = _context.Movies.Find(movieID);

            if (movie == null)
            {
                throw new UserException($"'Movie' with id '{movieID}' doesn't exist", 404);
            }
            // Ako film ima bar jednu recenziju onda racunaj prosjek
            if (_context.Reviews.Any(x => x.MovieID == movieID))
            {
                double? averageScore = _context.Reviews
                .Where(x => x.MovieID == movieID)
                .Select(x => x.Score)
                .Average();

                movie.AverageScore = averageScore.HasValue ? Math.Round(averageScore.Value, 2) : 0;
            }
            else
            {
                movie.AverageScore = 0;
            }
            _context.SaveChanges();
        }
    }
}
