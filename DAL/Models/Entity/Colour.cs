using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class Colour: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ImageId { get; set; }
        public string Description { get; set; }


        [ForeignKey("ImageId")]
        public Document Document { get; set; }
    }
}
