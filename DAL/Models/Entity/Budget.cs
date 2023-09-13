using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class Budget:BaseEntity
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string Operator { get; set; }
        public string Description { get; set; }
    }
}
