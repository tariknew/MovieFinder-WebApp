using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.IServices
{
    public interface IMovieService : IBaseService<VMMovie, MovieSearchObject, MovieInsertRequest, MovieUpdateRequest>
    {
        double GetMovieLastPrice();
        int GetMoviesCount(MovieSearchObject? searchObject = null);
    }
}
