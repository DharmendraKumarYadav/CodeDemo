using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class FeaturedBike: BaseEntity
    {
        public int Id { get; set; }
        public int BikeId { get; set; }
        public int FeatureType { get; set; }
        public Bike Bike { get; set; }
    }
}
