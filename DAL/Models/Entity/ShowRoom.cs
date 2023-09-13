using DAL.Models.Idenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{

    public class ShowRoom:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string UrlLink { get; set; }
        public int Status { get; set; }
        public string AuthorizedBy { get; set; }
        public DateTime AuthorizeDate { get; set; }
        public string Comments { get; set; }
        public int AreaId { get; set; }
        public Area Area { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
