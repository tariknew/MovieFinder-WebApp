using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.IServices
{
    public interface ICategoryService : IBaseService<VMCategory, object, CategoryInsertRequest, CategoryUpdateRequest>
    {
    }
}
