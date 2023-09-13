using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeColour: BaseEntity
    {
        public int Id { get; set; }
        public int ColourId { get; set; }
        public int BikeId { get; set; }
        public Colour Colour { get; set; }

      }
}
