using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeSimilar: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SimilarBikeId { get; set; }
        public int BikeId { get; set; }
        public Bike Bike { get; set; }
    }
}
