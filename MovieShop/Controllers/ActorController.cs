using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MoviesShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _service;
        public ActorController(IActorService service)
        {
            _service = service;
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator")]
        [Route("UpdateActor")]
        public VMActor UpdateActor([FromBody] ActorUpdateRequest request)
        {
            return _service.UpdateActor(request);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator")]
        [Route("InsertActor")]
        public VMActor InsertActor([FromBody] ActorInsertRequest request)
        {
            return _service.InsertActor(request);
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator")]
        public IEnumerable<VMActor> DeleteActor(int id)
        {
            return _service.DeleteActor(id);
        }
    }
}
