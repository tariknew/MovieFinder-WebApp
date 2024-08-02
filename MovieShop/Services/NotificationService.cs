using AutoMapper;
using InventoryManagementSoftware.Service.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MovieShop.Database;
using MovieShop.Filters;
using MovieShop.IServices;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Services
{
    [ServiceFilter(typeof(ErrorFilter))]
    public class NotificationService : INotificationService
    {
        private readonly MWSContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(MWSContext context, IHubContext<NotificationHub> hubContext, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        public VMNotification InsertNotification(NotificationInsertRequest request)
        {
            Notification notification = _mapper.Map<Notification>(request);
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            foreach (var u in _context.Users.Where(x => x.IdentityUserID != notification.CreatorId).ToList())
            {
                InsertUserNotification(new UserNotificationInsertRequest
                {
                    NotificationId = notification.Id,
                    UserId = u.Id,
                    IsRead = false
                });
            }
            _hubContext.Clients.All.SendAsync("sendToUser");
            return _mapper.Map<VMNotification>(notification);
        }
        public VMUserNotification InsertUserNotification(UserNotificationInsertRequest request)
        {
            var userNotification = _mapper.Map<UserNotification>(request);

            _context.UserNotifications.Add(userNotification);
            _context.SaveChanges();

            return _mapper.Map<VMUserNotification>(userNotification);
        }
        public IEnumerable<VMUserNotification> GetUserNotifications(int userId)
        {
            var list = _context.UserNotifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .Include(x => x.Notification)
                .OrderByDescending(x => x.Notification.DateTime);

            return _mapper.Map<List<VMUserNotification>>(list.ToList());
        }
        public VMUserNotification UpdateNotification(UserNotificationUpdateRequest request)
        {
            var notification = _context.UserNotifications
                .Where(x => x.NotificationId == request.NotificationId && x.UserId == request.UserId)
                .FirstOrDefault();

            if (notification == null)
            {
                throw new UserException($"'Notification' with id '{request.NotificationId}' doesn't exist", 404);
            }

            notification.IsRead = request.IsRead;
            _context.SaveChanges();

            return _mapper.Map<VMUserNotification>(notification);
        }
        public IEnumerable<VMUserNotification> UpdateAllNotifications(int userId)
        {
            var notification = _context.UserNotifications
                .Where(x => x.UserId == userId && !x.IsRead);

            if (notification == null)
            {
                throw new UserException($"'Notification' for userId '{userId}' not found", 404);
            }

            foreach (var notificationItem in notification)
            {
                notificationItem.IsRead = true;
            }

            _context.SaveChanges();

            return _mapper.Map<List<VMUserNotification>>(notification.ToList());
        }
    }
}
