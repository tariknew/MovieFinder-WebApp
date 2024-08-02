using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.Helpers;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.Security;
using MovieShop.ViewModels;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class ProfileService : IProfileService
    {
        private readonly MWSContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public ProfileService(UserManager<ApplicationUser> userManager, MWSContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }
        public VMUser ChangeProfilePicture(UserProfilePictureChange request)
        {
            var user = _context.Users.Find(request.UserID);
            if (user == null)
            {
                throw new UserException($"'User' with id '{request.UserID}' doesn't exist", 404);
            }
            byte[] photoBytes = ImageHelper.ConvertImageToBytes(request.ProfileImageString);
            request.ProfileImage = photoBytes;

            _mapper.Map(request, user);
            _context.SaveChanges();

            return _mapper.Map<VMUser>(user);
        }
        public async Task<VMUser> GetUserByIdentityUserId(string identityUserId)
        {
            var user = await _userManager.FindByIdAsync(identityUserId);

            if (user == null)
            {
                throw new UserException($"'User' with id '{identityUserId}' doesn't exist", 404);
            }

            var currentUser = _context.Users.Where(x => x.IdentityUserID == int.Parse(identityUserId)).FirstOrDefault();

            return _mapper.Map<VMUser>(currentUser);
        }
        public async Task<bool> ChangePassword(UserChangePasswordRequest request)
        {
            var currentUser = await _userManager.FindByIdAsync(request.IdentityUserId);
            if (currentUser == null)
            {
                throw new UserException($"'User' with id '{request.IdentityUserId}' doesn't exist", 404);
            }
            var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(currentUser, request.CurrentPassword);
            if (!isOldPasswordCorrect)
            {
                throw new UserException("Current password is incorrect", 404);
            }
            if (request.CurrentPassword == request.NewPassword)
            {
                throw new UserException("You already had this current password", 404);
            }
            var changePassword = await _userManager.ChangePasswordAsync(currentUser, request.CurrentPassword, request.NewPassword);

            if (changePassword.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ChangeEmail(UserChangeEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.IdentityUserId);

            if (user == null)
            {
                throw new UserException($"'User' with id '{request.IdentityUserId}' doesn't exist", 404);
            }

            if (user.Email == request.Email)
            {
                throw new UserException("You already had this current email", 404);
            }

            user.Email = request.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
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