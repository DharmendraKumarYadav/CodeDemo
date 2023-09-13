using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class Bike: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public Category Category { get; set; }
        public int BodyStyleId { get; set; }
        public BodyStyle BodyStyle { get; set; }
        public int BrandId { get; set; }
        public int Displacement { get; set; }
        public bool IsElectricBike { get; set; }

        public int DocumentId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<BikeVariant> BikeVariants { get; set; }
        public ICollection<BikeImage> BikeImages { get; set; }
        public ICollection<SaleBike> SaleBikes { get; set; }
        public ICollection<BikeColour> BikeColours { get; set; }
        public ICollection<BikeCity> BikeCity { get; set; }
        public ICollection<FeaturedBike> FeaturedBikes { get; set; }
    }
}

