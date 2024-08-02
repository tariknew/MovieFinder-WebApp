using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.Security;
using MovieShop.ViewModels;

namespace MoviesShop.Controllers
{
    public class MovieController : BaseController<VMMovie, MovieSearchObject, MovieInsertRequest, MovieUpdateRequest>
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        public MovieController(IMovieService service, INotificationService notificationService, UserManager<ApplicationUser> userManager) : base(service)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }
        public override VMMovie Insert(MovieInsertRequest request)
        {
            var movie = base.Insert(request);

            var notification = _notificationService.InsertNotification(new NotificationInsertRequest
            {
                DateTime = DateTime.Now,
                NotificationText = $"New movie - ({request.Title}) added to the list",
                CreatorId = int.Parse(_userManager.GetUserId(HttpContext.User)),
                MovieID = movie.Id
            });

            return movie;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("GetMovieLastPrice")]
        public double GetMovieLastPrice()
        {
            return ((IMovieService)_service).GetMovieLastPrice();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("GetMoviesCount")]
        public int GetMoviesCount([FromQuery] MovieSearchObject search = null)
        {
            return ((IMovieService)_service).GetMoviesCount(search);
        }
    }
}
