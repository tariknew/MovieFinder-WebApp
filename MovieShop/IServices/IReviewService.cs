using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.IServices
{
    public interface IReviewService
    {
        Task<IEnumerable<VMReview>> GetReviewsByMovieId(ReviewSearchByMovieIdRequest searchObject);
        Task<int> GetReviewsCountByMovieId(ReviewSearchByMovieIdRequest searchObject);
        IEnumerable<VMReview> DeleteReview(int id);
        VMReview InsertReview(ReviewInsertRequest request);
    }
}
