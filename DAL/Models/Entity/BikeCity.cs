using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeCity
    {
        public int Id { get; set; }
        public int BikeId { get; set; }
        public int CityId { get; set; }
        public Bike Bikes { get; set; }
        public City City { get; set; }
    }
}
