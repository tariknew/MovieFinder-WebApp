using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.IServices
{
    public interface IActorService
    {
        VMActor InsertActor(ActorInsertRequest request);
        VMActor UpdateActor(ActorUpdateRequest request);
        IEnumerable<VMActor> DeleteActor(int id);
    }
}
