using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("InsertNotification")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator")]
        public VMNotification InsertNotification([FromBody] NotificationInsertRequest request)
        {
            return _service.InsertNotification(request);
        }
        [HttpPut]
        [Route("UpdateNotification")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        public VMUserNotification UpdateNotification([FromBody] UserNotificationUpdateRequest request)
        {
            return _service.UpdateNotification(request);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        public IEnumerable<VMUserNotification> GetUserNotifications(int userId)
        {
            return _service.GetUserNotifications(userId);
        }
        [HttpPut]
        [Route("UpdateAllNotifications")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,User")]
        public IEnumerable<VMUserNotification> UpdateAllNotifications(int userId)
        {
            return _service.UpdateAllNotifications(userId);
        }
    }
}
