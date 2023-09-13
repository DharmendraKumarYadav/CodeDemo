using Core.Models;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.Common
{
    public interface INotificationService : IRepository<Notification>
    {
        Task<NotificationViewModel> GetNotifications(Guid userId);
        Task ClearNotification(Guid userId);
        Task CreateNotification(Notification notification);
    }
}
