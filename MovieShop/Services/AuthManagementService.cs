using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.Models.Responses;
using MovieShop.Security;
using MovieShop.ViewModels;
using System.Text;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class AuthManagementService : IAuthManagementService
    {
        private readonly MWSContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthManagementService(UserManager<ApplicationUser> userManager, MWSContext context, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<AuthResult> Login(UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new UserException("Incorrect username or password!", 404);
            }

            var isCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isCorrect)
            {
                throw new UserException("Incorrect username or password!", 404);
            }

            if (!user.EmailConfirmed)
            {
                throw new UserException("Email not confirmed!", 404);
            }

            var tokenResponse = await GetAccessToken(request.UserName, request.Password);
            var token = tokenResponse.AccessToken;

            user.RefreshToken = tokenResponse.RefreshToken;
            await _userManager.UpdateAsync(user);

            // Dobavimo IdentityUserId tog korisnika
            var IdentityUserId = user.Id;

            var currentUser = _context.Users.Where(x => x.IdentityUserID == IdentityUserId).FirstOrDefault();

            var result = new AuthResult
            {
                UserId = currentUser?.Id,
                Token = token,
                RefreshToken = user.RefreshToken
            };

            return result;
        }
        public async Task<bool> Register(UserRegistrationRequest request)
        {
            var existingUserByUsername = await _userManager.FindByNameAsync(request.UserName);
            if (existingUserByUsername != null)
            {
                throw new UserException("Username already exists", 404);
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                throw new UserException("Email already exists", 404);
            }

            var newUser = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var isCreated = await _userManager.CreateAsync(newUser, request.Password);

            await _userManager.AddToRoleAsync(newUser, "User");

            if (isCreated.Succeeded)
            {

                var user = new User
                {
                    IdentityUserID = newUser.Id,
                };
                _context.Users.Add(user);

                await _context.SaveChangesAsync();


                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);


                string emailConfirmationLink = $"{_configuration["AppUrl"]}/auth/confirmEmail?userName={newUser.UserName}&token={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken))}";


                var emailSubject = "Confirm Your Email";
                var emailContent = $"Hello {newUser.UserName},";
                emailContent += "Thank you for registering with us! To complete your registration and confirm your email, ";
                emailContent += $"please click the following link: <a href='{emailConfirmationLink}' style='font-size: larger;'>Click here</a><br>";
                emailContent += "If you did not register on our platform, you can ignore this email.<br>";
                emailContent += "Best regards,<br>Your MovieShop";

                await _emailService.SendEmailAsync(newUser.Email, emailSubject, emailContent);

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<UserTokenResponse> GetAccessToken(string username, string password)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7244");

            var tokenRequest = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "openid offline_access",
                UserName = username,
                Password = password
            };
            var response = await client.RequestPasswordTokenAsync(tokenRequest);

            if (response.IsError)
            {
                throw new UserException("Incorrect username or password", 404);
            }

            return new UserTokenResponse
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken
            };
        }

        public async Task<UserTokenResponse> RefreshToken(UserRefreshTokenRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new UserException($"'User' with username '{request.UserName}' doesn't exist", 404);
            }

            //Ako RefreshToken token iz respons-a nije isti kao onaj u bazi, baci exception
            if (user.RefreshToken != request.RefreshToken)
            {
                throw new UserException("Invalid access token or refresh token", 404);
            }

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7244");

            var tokenRequest = new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "openid offline_access",
                RefreshToken = user.RefreshToken
            };

            var response = await client.RequestRefreshTokenAsync(tokenRequest);

            user.RefreshToken = response.RefreshToken;
            await _userManager.UpdateAsync(user);

            var result = new UserTokenResponse
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken
            };

            return result;
        }

        public async Task<UserForgetPasswordResponse> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new UserForgetPasswordResponse
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["AppUrl"]}/auth/restartPass?token={validToken}"; //link na angular stranicu za reset passworda

            await _emailService.SendEmailAsync(email, "Reset Password", $"Dear {user.UserName},<br>You have requested to reset your password on MovieShop because you have forgotten your password. If you did not request this, please ignore it."
                + "It will expire and become useless in 24 hours time."
                + $"<p><span style='font-size: larger;'>To reset your password, please visit the following page: </span><a href='{url}' style='font-size: larger;'>Click here</a></p>"
                + "All the best,<br>MovieShop");

            return new UserForgetPasswordResponse
            {
                IsSuccess = true,
                Message = "Reset password URL has been sent to the email successfully!",
                Email = email
            };
        }
        public async Task<UserForgetPasswordResponse> ResetPassword(UserResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserForgetPasswordResponse
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new UserForgetPasswordResponse
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new UserForgetPasswordResponse
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new UserForgetPasswordResponse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        public async Task<bool> ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new UserException($"'User' with username '{request.UserName}' doesn't exist", 404);
            }

            var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
            var normalToken = Encoding.UTF8.GetString(decodedToken);

            var confirmResult = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (confirmResult.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

