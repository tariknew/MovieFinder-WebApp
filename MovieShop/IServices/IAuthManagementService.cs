using MovieShop.Models.Requests;
using MovieShop.Models.Responses;
using MovieShop.Security;
using MovieShop.ViewModels;

namespace MovieShop.IServices
{
    public interface IAuthManagementService
    {
        Task<bool> Register(UserRegistrationRequest request);
        Task<AuthResult> Login(UserLoginRequest request);
        Task<UserForgetPasswordResponse> ForgetPassword(string email);
        Task<UserForgetPasswordResponse> ResetPassword(UserResetPasswordRequest model);
        Task<bool> ConfirmEmail(ConfirmEmailRequest request);
        Task<UserTokenResponse> RefreshToken(UserRefreshTokenRequest request);
    }
}
