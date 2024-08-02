using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.IServices;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class BaseService<TEntity, TDbEntity, TSearch, TInsert, TUpdate> : IBaseService<TEntity, TSearch, TInsert, TUpdate> where TEntity : class where TDbEntity : class where TSearch : class where TInsert : class where TUpdate : class
    {
        protected readonly MWSContext _context;
        protected readonly IMapper _mapper;

        public BaseService(MWSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public virtual IEnumerable<TEntity> Get(TSearch searchObject = null)
        {
            var list = _context.Set<TDbEntity>();

            return _mapper.Map<List<TEntity>>(list.ToList());
        }
        public virtual TEntity Insert(TInsert request)
        {
            var set = _context.Set<TDbEntity>(); // Vraca mi DBSet<Movie>
            TDbEntity entity = _mapper.Map<TDbEntity>(request);
            set.Add(entity);
            _context.SaveChanges();
            return _mapper.Map<TEntity>(entity);
        }
        public virtual IEnumerable<TEntity> Delete(int id)
        {
            var set = _context.Set<TDbEntity>();
            TDbEntity entity = _context.Set<TDbEntity>().Find(id);
            set.Remove(entity);
            _context.SaveChanges();

            if (entity == null)
            {
                throw new UserException($"Object with specified '{id}' does not exist!", 404);
            }
            return _mapper.Map<List<TEntity>>(entity);
        }
        public virtual TEntity Update(int id, TUpdate request)
        {
            var entity = _context.Set<TDbEntity>().Find(id);
            _mapper.Map(request, entity);
            _context.SaveChanges();
            return _mapper.Map<TEntity>(entity);
        }
        public virtual TEntity GetById(int id)
        {
            var item = _context.Set<TDbEntity>().Find(id);

            if (item == null)
            {
                throw new UserException($"Object with specified '{id}' does not exist!", 404);
            }
            return _mapper.Map<TEntity>(item);
        }
    }
}
