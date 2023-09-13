using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class SaleBike : BaseEntity
    {
        public int Id { get; set; }
        public BikeVariant BikeVariants { get; set; }
        public int BikeVariantsId { get; set; }
        public string ChesisNumber { get; set; }
        public string EngineNumber { get; set; }
        public int BikeColurId { get; set; }
        public string ExShowRoomPrice { get; set; }
        public string RTOPrice { get; set; }
        public string InsurancePrice { get; set; }
        public string Price { get; set; }
        public string BookingAmount { get; set; }
        public string Availbility { get; set; }//Redy to Sell ,Wating(No. Of Days)//Radio Button
        public int NoOfDay { get; set; }
        public int ShowRoomId { get; set; }
        public ShowRoom ShowRoom { get; set; }
        public string DealerId { get; set; }
        public int DealerShowRoomId { get; set; }
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
        public int Status { get; set; } //Open=1,Invoiced=2,//Cancelled:0

        [NotMapped]
        public string RequestorName { get; set; }
    }
    public class BikesAccesories : BaseEntity
    {
        public int Id { get; set; }
        public int AccesoriesId { get; set; }
        public int Price { get; set; }
        public int SaleBikeId { get; set; }
        public SaleBike DealerBikes { get; set; }

    }
}
