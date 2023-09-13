using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BikeBookingModel
    {
        public int Id { get; set; }
        public string BookingNumber { get; set; }
        public string Name { get; set; }

        public string BookingDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentStatus { get; set; }

        public int BookingStatusId { get; set; }
        public string BookingStatus { get; set; }
        public int BikeId { get; set; }
        public int CityId { get; set; }
        public string Amount { get; set; }
        public string PaymentMethodSystemName { get; set; }
        public int BikeVariantPriceId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public int SaleBikeId { get; set; }

        public string BookedBy { get; set; }

        public string ChesisNumber { get; set; }
        public string EngineNumber { get; set; }

        //Price Details
        public string BikePrice { get; set; }
        public string VariantPrice { get; set; }
        public string ExShowRoomPrice { get; set; }
        public string RTOPrice { get; set; }
        public string InsurancePrice { get; set; }
        public string BookingPrice { get; set; }

        //Bike Details
        public string BikeName { get; set; }
        public string BikeVariantName { get; set; }
        public string BikeSpecification { get; set; }
        public string BikeColour { get; set; }
        public string Category { get; set; }
        public string BodyStyle { get; set; }
        public string Brand { get; set; }
        public string Displcaement { get; set; }
        public bool IsElectricBike { get; set; }

        //ShowRoom Name:
        public string ShowRoomName { get; set; }
        public string ShowRoomMobileNumber { get; set; }


        public string ShowRoomPhoneNumber { get; set; }
        public string ShowRoomEmail { get; set; }
        public string ShowRoomAddressLine1 { get; set; }
        public string ShowRoomAddressLine2 { get; set; }
        public string ShowRoomArea { get; set; }
        public string ShowRoomCity { get; set; }
        public string ShowRoomState { get; set; }
        public string ShowRoomPinCode { get; set; }
        public string ShowRoomCountry { get; set; }
        public string ShowRoomOwnerName { get; set; }
        public string ShowRoomOwnerEmail { get; set; }
        public string ShowRoomOwnerMobile { get; set; }
        public string DelaerOrBrokerId { get; set; }
        public string Url { get; set; }
    }
    public class CustomerBikeBookingResponse
    {
        public string BookingNumber { get; set; }
    }
    public class CustomerBikeBookingRequest
    {
        public BillingAddress BillingAddress { get; set; }
        public BikeDetails Items { get; set; }
        public string payment { get; set; }
        public string UserId { get; set; }
    }
    public class BillingAddress
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
    public class BikeDetails
    {
        public int BikeId { get; set; }
        public int CityId { get; set; }
        public int BikeVariantPriceId { get; set; }
        public string Amount { get; set; }
    }
    public class BookingOrderRequest
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BookingAmount { get; set; }
        public int DealerBikeId { get; set; }
        public string BikeVariant { get; set; }

    }
    public class BookingOrderResponse
    {
        public string BookingNumber { get; set; }
        public string Hash { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Amount { get; set; }
        public string PayuUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string PayuSucessUrl { get; set; }
        public string PayuFailureUrl { get; set; }
        public string PlanName { get; set; }

        public string MerchantKey { get; set; }
        public string ServiceProvider { get; set; }

    }

    public class BikeBookingCustomerModel
    {
        public int Id { get; set; }
        public string BookingNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingAmount { get; set; }
        public string BookingStatus { get; set; }

    }
    public class BikeBookingCustomerDetailsModel
    {
        public BikeBookingCustomerDetailsModel()
        {
            BillingAddress = new BillingAddress();
            ShowRoomAddress = new BookingShowRoomAddress();
        }
        public int Id { get; set; }
        public string BookingNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string BookingStatus { get; set; }

        public string VaraintName { get; set; }

        public BillingAddress BillingAddress { get; set; }
        public BookingShowRoomAddress ShowRoomAddress { get; set; }
    }

    public class BookingShowRoomAddress
    {
        public int ShowRoomId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string UrlLink { get; set; }
        public int Status { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PinCode { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
    }
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            CountModel = new DashboardCountModel();
            MonthlyBooking = new List<DashboardMonthlyModel>();
            MonthlyCustomer= new List<DashboardMonthlyModel>();
        }
        public DashboardCountModel CountModel { get; set; }

        public List<DashboardMonthlyModel> MonthlyBooking { get; set; }

        public List<DashboardMonthlyModel> MonthlyCustomer { get; set; }
    }
    public class DashboardCountModel
    {
        public int CustomerCount { get; set; }
        public int SaleBikeCount { get; set; }
        public int BookingCount { get; set;}
        public int BikeCount { get; set; }
    }
    public class DashboardMonthlyModel
    {
        public string MonthName { get; set; }
        public int Count { get; set; }
    }
    public class BookingStatusRequest
    {
        public string BookingNumber { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
    }
}
