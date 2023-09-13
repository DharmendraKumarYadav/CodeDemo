using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AvailableShowRoomBikes
    {
        public int DealerBikeId { get; set; }
        public int VaraintId { get; set; }
        public int CityId { get; set; }
        public int ShowRoomId { get; set; }
        public string VariantName { get; set; }
        public string Specification { get; set; }
        public string Colour { get; set; }
        public int Available { get; set; }
        public string ExShowRoomAmount { get; set; }
        public string RtoAmount { get; set; }
        public string InsuranceAmount { get; set; }
        public string BookingAmount { get; set; }
        public string TotalAmount { get; set; }
        public string ChesisNumber { get; set; }
        public string EngineNumber { get; set; }

        public string ShowRoomName { get; set; }

        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }

        public string Url { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AreaName { get; set; }

        public string PinCode { get; set; }

        public string Availibility { get; set; }
        public string CityName { get; set; }

    }
}
