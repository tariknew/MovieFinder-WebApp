using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MoviesShop.Controllers
{
    public class CategoryController : BaseController<VMCategory, object, CategoryInsertRequest, CategoryUpdateRequest>
    {
        public CategoryController(ICategoryService service) : base(service)
        {
        }
    }
}
