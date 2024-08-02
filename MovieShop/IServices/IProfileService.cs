using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.IServices
{
    public interface IProfileService
    {
        VMUser ChangeProfilePicture(UserProfilePictureChange request);
        Task<bool> ChangePassword(UserChangePasswordRequest request);
        Task<bool> ChangeEmail(UserChangeEmailRequest request);
        Task<VMUser> GetUserByIdentityUserId(string identityUserId);
    }
}
