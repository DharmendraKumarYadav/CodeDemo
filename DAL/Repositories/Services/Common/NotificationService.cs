using Core.Enums;
using Core.Models;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using DAL.Repositories.Interfaces.Common;
using DAL.Repositories.Services.SignalIR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Common
{
    public class NotificationService : Repository<Notification>, INotificationService

    {
        IHubContext<BroadcastHub, IHubClient> _hubContext;
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public NotificationService(DbContext context, IHubContext<BroadcastHub, IHubClient> hubContext) : base(context)
        {
            _hubContext = hubContext;
        }

        public async Task<NotificationViewModel> GetNotifications(Guid userId)
        {
            NotificationViewModel notificationViewModel = new NotificationViewModel();
            IQueryable<Notification> query = _appContext.Notifications.OrderBy(u => u.CreatedDate).Where(m => m.UserId == userId);

            var list = await query.ToListAsync();
            notificationViewModel.Count = query.Count();

            foreach (var item in list)
            {
                notificationViewModel.Notifications.Add(new NotificationModel
                {
                    Description = item.Description,
                    Id = item.Id,
                    Name = item.Name,
                    Status = item.Status,
                    Type = item.Type,
                    Url = item.Url,
                    UserId = item.UserId,
                    CreatedDate=item.CreatedDate
                });

            }

            return notificationViewModel;
        }

        public async Task CreateNotification(Notification notification)
        {
            await _appContext.Notifications.AddAsync(notification);
            await _appContext.SaveChangesAsync();
            await _hubContext.Clients.All.BroadcastMessage();
        }
        public async Task ClearNotification(Guid userId)
        {
            var notifications=await _appContext.Notifications.Where(m=> m.UserId == userId).ToListAsync();
            _appContext.Notifications.RemoveRange(notifications);
            await _appContext.SaveChangesAsync();
            await _hubContext.Clients.All.BroadcastMessage();
        }
    }
}
