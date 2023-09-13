using BDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BikeDetailModel
    {
        public int Id { get; set; }
        public string BikeName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Displacement { get; set; }
        public string Brand { get; set; }
        public string BodyStyle { get; set; }
        public string Category { get; set; }
        public bool IsElectricBike { get; set; }
        public int DocumentId { get; set; }
        public List<CityModelList> City { get; set; }
        public List<BikeVariantsModelList> BikeVariants { get; set; }
        public List<BikeColourModelList> Colours { get; set; }
        public List<BikeImagesList> BikeImages { get; set; }

        public List<BikeDealerModelList> BikeDealers { get; set; }
        public ImageModel Document { get; set; }



    }
    public class BikeImagesList
    {
        public int Id { get; set; }
        public string Image { get; set; }
    }
    public class CityModelList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class BikeVariantsModelList
    {
        public int VariantId { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public List<BikePriceList> BikePrices { get; set; }
        public List<BikeAttributeModelList> BikeFeatures { get; set; }
        public List<BikeSpecificationModelList> BikeSpecifications { get; set; }
    }
    public class BikeColourModelList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
    public class BikeSpecificationModelList
    {
        public int Id { get; set; }
        public string SpecifcaitionName { get; set; }

        public string Image { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class BikeAttributeModelList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class BikeDealerModelList
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Area { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
    }
    public class BikePriceList
    {
        public int Id { get; set; }
        public string ExShowRoomPrice { get; set; }
        public string RTOPrice { get; set; }
        public string InsurancePrice { get; set; }
        public string Price { get; set; }
        public string BookingAmount { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
    }
    //public class BikeSpecificationList
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public List<BikeAtrributeList> BikeAttributes { get; set; }
    //}
    //public class BikeAtrributeList
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Value { get; set; }
    //}
    public class BikeSearchData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SearchType { get; set; }

    }

}
