using System.Collections.Generic;

namespace BDB.ViewModels.Bike
{
    public class BikeVarianSpecificationModel
    {
        public int VariantId { get; set; }
        public string Name { get; set; }
        public List<BikeSpecificationModel> Specifications { get; set; }

    }
    public class BikeSpecificationModel
    {
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; }
        public int BikeVariantId { get; set; }
    }
}
