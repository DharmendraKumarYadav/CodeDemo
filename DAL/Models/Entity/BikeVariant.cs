using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeVariant: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public int BikeId { get; set; }
        public Bike Bike { get; set; }
        public ICollection<BikePrice> BikePrices { get; set; }
        public ICollection<BikeSpecifications> BikeSpecifications { get; set; }
        public ICollection<BikeFeatures> BikeFeatures { get; set; }
    }
}
