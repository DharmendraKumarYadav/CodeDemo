using System.Collections.Generic;

namespace BDB.ViewModels.Bike
{
    public class BikeVarianPricetModel
    {
        public int VariantId { get; set; }
        public string  Name { get; set; }
        public List<BikePriceModel> Prices { get; set; }

    }
    public class BikePriceModel
    {
        public int Id { get; set; }

        public bool? isMinPrice { get; set; }
        public string ExShowRoomAmount { get; set; }
        public string RtoAmount { get; set; }
        public string InsuranceAmount { get; set; }
        public string TotalAmount { get; set; }
        public string BookingAmount { get; set; }
        public int CityId { get; set; }
        public int VariantId { get; set; }

    }

}
