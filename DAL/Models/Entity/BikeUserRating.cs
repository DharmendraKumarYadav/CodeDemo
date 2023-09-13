using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeUserRating: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Review { get; set; }
        public string Rating { get; set; }
        public int BikeId { get; set; }
        public Bike Bike { get; set; }
        public bool IsPublished { get; set; }
    }
}
