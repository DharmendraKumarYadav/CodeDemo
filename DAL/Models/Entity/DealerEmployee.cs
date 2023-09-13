using DAL.Models.Idenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class DealerEmployee
    {
        public int Id { get; set; }

        public int DealerId { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
