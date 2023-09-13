using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeFeatureType: BaseEntity
    {
        public int Id { get; set; }
        public int BikeId { get; set; }
        public int TypeId { get; set; }
    }
}
