using DAL.Models.Entity;
using System;

namespace BDB.ViewModels.Common
{
    public class DealerViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
    }
    public class DealerModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int AreaId { get; set; }
    }

    public class ShowRoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string UrlLink { get; set; }
        public int Status { get; set; }
        public string Comments { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string DealerId { get; set; }
        public int AreaId { get; set; }
    }
    public class ShowRoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string UrlLink { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string DealerId { get; set; }
        public int AreaId { get; set; }
    }
    public class ShowRoomAuthroizeModel
    {
        public int Id { get; set; }

        public int Status { get; set; }
        public string Comments { get; set; }
    }

    public class DealerBikeViewModel
    {
        public int Id { get; set; }
        public int BikeVariantsId { get; set; }
        public string VariantName { get; set; }
        public string ChesisNumber { get; set; }
        public string EngineNumber { get; set; }
        public int BikeColurId { get; set; }
        public string ExShowRoomPrice { get; set; }
        public string RTOPrice { get; set; }
        public string InsurancePrice { get; set; }
        public string Price { get; set; }
        public string BookingAmount { get; set; }
        public int ShowRoomId { get; set; }
        public string Availbility { get; set; }//Redy to Sell ,Wating(No. Of Days)//Radio Button
        public int NoOfDay { get; set; }
        public int TransferStatus { get; set; }
        public string TransferRequestedBy { get; set; }
        public DateTime TransferRequestedDate { get; set; }
        public string TransferAuthorizeBy { get; set; }
        public DateTime TransferAuthorizeDate { get; set; }
        public string ReturnBy { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Remarks { get; set; }
        public bool IsTransferred { get; set; }
        public bool IsTransferable { get; set; }
        public int Status { get; set; }
        public string RequestorName { get; set; }
    }
    public class DealerBikeModel
    {
        public int Id { get; set; }
        public int BikeVariantsId { get; set; }
        public string ChesisNumber { get; set; }
        public string EngineNumber { get; set; }
        public int BikeColurId { get; set; }
        public string ExShowRoomPrice { get; set; }
        public string RTOPrice { get; set; }
        public string InsurancePrice { get; set; }
        public string Price { get; set; }
        public string BookingAmount { get; set; }
        public int ShowRoomId { get; set; }
        public string Availbility { get; set; }//Redy to Sell ,Wating(No. Of Days)//Radio Button
        public int NoOfDay { get; set; }

        public bool IsTransferable { get; set; }
    }
    public class BrokerBikeRequestModel
    {
        public int Id { get; set; }
        public int TransferStatus { get; set; }
        public string Remarks { get; set; }
    }
    public class DealerBikeAuthroizeModel
    {
        public int Id { get; set; }
        public int TransferStatus { get; set; }
        public string Remarks { get; set; }
    }


}
