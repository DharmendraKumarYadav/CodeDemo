using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class BikeBooking : BaseEntity
    {
        public int Id { get; set; }
        public string BookingNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public int SaleBikeId { get; set; }
        public SaleBike SaleBikes { get; set; }
        //public string BookingAmount { get; set; }

        //Payment Info

        public string Amount { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentMethodSystemName { get; set; }
        public string BookedBy { get; set; }
        public int BookingStatusId { get; set; }
        public string UserId { get; set; }
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
    public class BikeBookingEntityTypeConfiguration : ICustomEntityTypeConfiguration<BikeBooking>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="typeBuilder">Type builder</param>
        /// <param name="modelBuilder">Model builder</param>
        public void Configure(EntityTypeBuilder<BikeBooking> typeBuilder, ModelBuilder modelBuilder)
        {
            if (typeBuilder == null)
            {
                throw new ArgumentNullException(nameof(typeBuilder));
            }
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            // Table
            typeBuilder.ToTable("BikeBookings");

            // Keys, Unique Keys and Indices
            typeBuilder.HasKey(m => m.Id);
            typeBuilder.Property(t => t.BookingNumber)
           .HasComputedColumnSql("N'DBD'+ RIGHT('00000'+CAST(Id AS VARCHAR(5)),5)");
        }
    }
}
