using System.Collections.Generic;

namespace BDB.ViewModels.Bike
{
    public class BikeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string BrandName { get; set; }
        public string BodyStyle { get; set; }
        public string Category { get; set; }
        public string IsElectricBike { get; set; }

    }
    public class BikeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int BrandId { get; set; }
        public int BodyStyleId { get; set; }
        public int Price { get; set; }
        public int Displacement { get; set; }
        public int CategoryId { get; set; }
        public int DocumentId { get; set; }
        public string IsElectricBike { get; set; }
        public List<int> ColourIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<BikeVariantModel> Variants { get; set; }
        public List<FileUpload> Files { get; set; }

        public ImageModel Document { get; set; }

    }
    
    public class BikeVariantModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }

    }
    public class BikeColourModel
    {
        public int Id { get; set; }
        public int ColourId { get; set; }
        public int BikeId { get; set; }

    }

    public class BikeVariantPriceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string RtoAmount { get; set; }
        public string ExShowRoomAmount { get; set; }
        public string InsuranceAmount { get; set; }
        public string TotalAmount { get; set; }
        public string BookingAmount { get; set; }

        public bool? IsMinPrice { get; set; }

    }
}
