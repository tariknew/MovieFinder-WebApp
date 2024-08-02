using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;

        public ProfileController(IProfileService service)
        {
            _service = service;
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("ChangeProfilePicture")]
        public VMUser ChangeProfilePicture([FromBody] UserProfilePictureChange request)
        {
            return _service.ChangeProfilePicture(request);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("ChangePassword")]
        public async Task<bool> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            return await _service.ChangePassword(request);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        [Route("ChangeEmail")]
        public async Task<bool> ChangeEmail([FromBody] UserChangeEmailRequest request)
        {
            return await _service.ChangeEmail(request);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        public async Task<VMUser> GetUserByIdentityUserId(string identityUserId)
        {
            return await _service.GetUserByIdentityUserId(identityUserId);
        }
    }
}
