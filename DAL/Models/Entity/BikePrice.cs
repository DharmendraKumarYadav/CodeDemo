using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikePrice: BaseEntity
    {
        public int Id { get; set; }
        public string ExShowRoomPrice { get; set; }
        public string RTOPrice { get; set; }
        public string InsurancePrice { get; set; }
        public string Price { get; set; }
        public string BookingAmount { get; set; }
        public int CityId { get; set; }
        public int BikeVariantId { get; set; }
        public bool? isMinPrice { get; set; }
        public City City { get; set; }
    }
}
