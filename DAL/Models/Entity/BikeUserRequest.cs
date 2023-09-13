using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeUserRequest: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public string MobileNumber { get; set; }
        public int BikeVariantId { get; set; }
        public BikeVariant BikeVariants { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int? ShowRoomId { get; set; }
        public string Remarks { get; set; }
    }
}
