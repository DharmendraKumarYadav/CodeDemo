using System.Collections.Generic;

namespace BDB.ViewModels.Bike
{
    public class BikeVarianFeatureModel
    {
        public int VariantId { get; set; }
        public string Name { get; set; }
        public List<BikeFeatureModel> Features { get; set; }

    }
    public class BikeFeatureModel
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; }
        public int BikeVariantId { get; set; }
    }
}
