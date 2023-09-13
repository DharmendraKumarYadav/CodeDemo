using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeNews: BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescrption { get; set; }
        public string LongDescrption { get; set; }
        public int ImageId { get; set; }
    }
}
