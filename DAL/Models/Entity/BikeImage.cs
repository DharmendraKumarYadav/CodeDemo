using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeImage: BaseEntity
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int BikeId { get; set; }
        public Bike Bike { get; set; }

 
        public Document Documents { get; set; }
    }
}
