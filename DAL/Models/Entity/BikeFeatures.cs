using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeFeatures: BaseEntity
    {
        public int Id { get; set; }
        public string AttributeValue { get; set; }
        public int AttributeId { get; set; }
        public int BikeVariantId { get; set; }
        public int BikeId { get; set; }

        [ForeignKey("AttributeId")]
        public Attributes Attributes { get; set; }

    }
}
