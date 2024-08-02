using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class CategoryService : BaseService<VMCategory, Category, object, CategoryInsertRequest, CategoryUpdateRequest>, ICategoryService
    {
        public CategoryService(MWSContext context, IMapper mapper) : base(context, mapper) { }
        public override VMCategory Insert(CategoryInsertRequest request)
        {
            var newCategory = _mapper.Map<Category>(request);

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return _mapper.Map<VMCategory>(newCategory);
        }
        public override IEnumerable<VMCategory> Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                throw new UserException($"No category found with given '{id}'!", 404);
            }

            var movieCategories = _context.MovieCategories.Where(x => x.MovieID == id);
            _context.MovieCategories.RemoveRange(movieCategories);
            _context.SaveChanges();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            var allCategories = _context.Categories.ToList();

            return _mapper.Map<List<VMCategory>>(allCategories);
        }
        public override VMCategory Update(int id, CategoryUpdateRequest request)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                throw new UserException($"No category found with given '{id}'!", 404);
            }

            _mapper.Map(request, category);
            _context.SaveChanges();

            return _mapper.Map<VMCategory>(category);
        }
    }
}
