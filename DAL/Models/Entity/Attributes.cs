using DAL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class Attributes: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
   
    }
}
