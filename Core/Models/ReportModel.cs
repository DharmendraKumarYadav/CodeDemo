using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BookingFilter
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CityId { get; set; }
        public string DealerId { get; set; }
        public int ShowRoomId { get; set; }

    }
    public class ReportBookingModel
    {
        public string BookingId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BikeName { get; set; }
        public string BookingAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string BookingDate { get; set; }


    }
    public class ReportDropDown
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
