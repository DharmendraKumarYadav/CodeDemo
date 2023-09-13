using DAL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class Area: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string PinCode { get; set; }
        public City City { get; set; }
    }
}
