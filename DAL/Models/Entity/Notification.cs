using DAL.Models.Idenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class Notification:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public Guid UserId { get; set; }
        public User Users { get; set; }
    }
}
