using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("GetReviewsByMovieId")]
        public Task<IEnumerable<VMReview>> GetReviewsByMovieId([FromQuery] ReviewSearchByMovieIdRequest searchObject)
        {
            return _service.GetReviewsByMovieId(searchObject);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("PostReview")]
        public VMReview InsertReview([FromBody] ReviewInsertRequest request)
        {
            return _service.InsertReview(request);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("GetReviewsCount")]
        public Task<int> GetReviewsCountByMovieId([FromQuery] ReviewSearchByMovieIdRequest searchObject)
        {
            return _service.GetReviewsCountByMovieId(searchObject);
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        public IEnumerable<VMReview> DeleteReview(int id)
        {
            return _service.DeleteReview(id);
        }
    }
}
