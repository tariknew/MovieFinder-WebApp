using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.IServices
{
    public interface INotificationService
    {
        VMNotification InsertNotification(NotificationInsertRequest request);
        VMUserNotification InsertUserNotification(UserNotificationInsertRequest request);
        VMUserNotification UpdateNotification(UserNotificationUpdateRequest request);
        IEnumerable<VMUserNotification> GetUserNotifications(int userId);
        IEnumerable<VMUserNotification> UpdateAllNotifications(int userId);
    }
}
