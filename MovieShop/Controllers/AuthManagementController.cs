using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.Models.Responses;
using MovieShop.Security;
using MovieShop.ViewModels;
using System.Text;

namespace MovieShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthManagementController : Controller
    {
        private readonly IAuthManagementService _service;


        public AuthManagementController(IAuthManagementService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<bool> Register([FromQuery] UserRegistrationRequest request)
        {
            return await _service.Register(request);
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<UserTokenResponse> RefreshToken([FromBody] UserRefreshTokenRequest request)
        {
            return await _service.RefreshToken(request);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<AuthResult> Login([FromQuery] UserLoginRequest request)
        {
            return await _service.Login(request);
        }
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _service.ForgetPassword(email);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] UserResetPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.ResetPassword(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<bool> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            return await _service.ConfirmEmail(request);
        }

    }
}
