using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.Helpers;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class ActorService : IActorService
    {
        private readonly MWSContext _context;
        private readonly IMapper _mapper;
        public ActorService(MWSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public VMActor InsertActor(ActorInsertRequest request)
        {
            byte[] photoBytes = ImageHelper.ConvertImageToBytes(request.PhotoString);

            request.Photo = photoBytes;

            var newActor = _mapper.Map<Actor>(request);

            _context.Actors.Add(newActor);
            _context.SaveChanges();

            return _mapper.Map<VMActor>(newActor);
        }
        public VMActor UpdateActor(ActorUpdateRequest request)
        {
            var actor = _context.Actors.Find(request.ActorID);

            if (actor == null)
            {
                throw new UserException($"'Actor' with id '{request.ActorID}' doesn't exist", 404);
            }

            byte[] photoBytes = ImageHelper.ConvertImageToBytes(request.PhotoString);

            request.Photo = photoBytes;

            _mapper.Map(request, actor);
            _context.SaveChanges();

            return _mapper.Map<VMActor>(actor);
        }
        public IEnumerable<VMActor> DeleteActor(int id)
        {
            var actor = _context.Actors.Find(id);

            if (actor == null)
            {
                throw new UserException($"'Actor' with id '{id}' doesn't exist", 404);
            }

            _context.Actors.Remove(actor);
            _context.SaveChanges();

            var allActors = _context.Actors.ToList();

            return _mapper.Map<List<VMActor>>(allActors);
        }
    }
}

