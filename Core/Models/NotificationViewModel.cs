using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class NotificationViewModel
    {
        public NotificationViewModel()
        {
            Notifications = new List<NotificationModel>();
        }
        public int Count { get; set; }

        public List<NotificationModel> Notifications { get; set; }
    }
    public class NotificationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
