using Core.Enums;
using Core.Models;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.BikeManagement
{
    public class BikeBookingService : Repository<BikeBooking>, IBikeBookingService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BikeBookingService(DbContext context) : base(context)
        {
        }
        public async Task<List<BikeBooking>> GetBikeBookings(int page, int pageSize,string dealerId)
        {
            var query = _appContext.BikeBookings.Include(m => m.SaleBikes).ThenInclude(m => m.ShowRoom).Where(m =>m.IsActive == true);

            if (!string.IsNullOrEmpty(dealerId))
            {
                query = query.Where(m => m.DelaerOrBrokerId == dealerId);
            }
            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }
        public async Task<List<BikeBookingCustomerModel>> GetMyBikeBookings(string userId)
        {
            List<BikeBookingCustomerModel> bikeBookingCustomerModels = new List<BikeBookingCustomerModel>();
            var query = _appContext.BikeBookings.Where(m => m.UserId == userId).OrderBy(u => u.Id).ToList();

            foreach (var item in query)
            {
                bikeBookingCustomerModels.Add(new BikeBookingCustomerModel
                {
                    Id = item.Id,
                    BookingStatus = ((BookingStatus)item.BookingStatusId).ToString(),
                    BookingAmount = item.BookingPrice.ToString(),
                    BookingDate = item.CreatedDate,
                    BookingNumber = item.BookingNumber.ToString(),
                });
            }

            return bikeBookingCustomerModels;
        }
        public async Task<BikeBookingCustomerDetailsModel> GetBikeBookingDetail(string bookingId)
        {
            BikeBookingCustomerDetailsModel bikeBookingCustomerDetailsModel = new BikeBookingCustomerDetailsModel();
            var query = _appContext.BikeBookings
                 .Include(m => m.SaleBikes)
                    .ThenInclude(m => m.ShowRoom).ThenInclude(m=>m.Area).ThenInclude(m=>m.City)
                .Where(m => m.BookingNumber == bookingId).FirstOrDefault();
            if (query != null) {
                bikeBookingCustomerDetailsModel.Id = query.Id;
                bikeBookingCustomerDetailsModel.BookingStatus = ((BookingStatus)query.BookingStatusId).ToString();
                bikeBookingCustomerDetailsModel.BookingAmount = query.BookingPrice.ToString();
                bikeBookingCustomerDetailsModel.BookingDate = query.CreatedDate;
                bikeBookingCustomerDetailsModel.BookingNumber = query.BookingNumber.ToString();
                bikeBookingCustomerDetailsModel.PaymentStatus = ((PaymentStatus)query.PaymentStatusId).ToString();
                bikeBookingCustomerDetailsModel.VaraintName = query.BikeVariantName;

                bikeBookingCustomerDetailsModel.BillingAddress.FirstName = query.FullName.ToString();
                bikeBookingCustomerDetailsModel.BillingAddress.LastName = "";
                bikeBookingCustomerDetailsModel.BillingAddress.Email = query.Email.ToString();
                bikeBookingCustomerDetailsModel.BillingAddress.Phone = query.PhoneNumber.ToString();
                bikeBookingCustomerDetailsModel.BillingAddress.PostalCode = query.PostalCode?.ToString();
                bikeBookingCustomerDetailsModel.BillingAddress.Address1 = query.Address1?.ToString();
                bikeBookingCustomerDetailsModel.BillingAddress.Address2 = query.Address2?.ToString();
         
                bikeBookingCustomerDetailsModel.BillingAddress.City = query.City?.ToString();
                bikeBookingCustomerDetailsModel.BillingAddress.State = query.State?.ToString();

                bikeBookingCustomerDetailsModel.ShowRoomAddress.ShowRoomId = query.SaleBikes.ShowRoom.Id;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.Name = query.ShowRoomName;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.EmailId = query.ShowRoomEmail;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.PhoneNumber = query.ShowRoomPhoneNumber;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.MobileNumber = query.ShowRoomMobileNumber;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.AddressLine1 = query.ShowRoomAddressLine1;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.AddressLine2 = query.ShowRoomAddressLine2;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.Area = query.ShowRoomArea;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.PinCode = query.ShowRoomPinCode;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.UrlLink = query.Url;
                bikeBookingCustomerDetailsModel.ShowRoomAddress.City = query.ShowRoomCity;
            }
          
            return bikeBookingCustomerDetailsModel;
        }

        public async Task<List<DashboardMonthlyModel>> GetMonthlyBooking(int year,string dealerId)
        {
            List<DashboardMonthlyModel> dashboardMonthlyModels = new List<DashboardMonthlyModel>();
 
            var query = _appContext.BikeBookings.Include(m => m.SaleBikes).ThenInclude(m => m.ShowRoom).Where(m => m.CreatedDate.Year == year && m.IsActive == true);

            if (!string.IsNullOrEmpty(dealerId)) {
                query = query.Where(m => m.SaleBikes.ShowRoom.UserId.ToString() == dealerId);
            }

            var booking = await query.GroupBy(g => g.CreatedDate.Month).Select(g => new DashboardMonthlyModel { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key), Count = g.Count() }).ToListAsync();

            var months = (from month in GetMonths()
                          join bookings in booking
                          on month equals bookings.MonthName
                          into EmployeeAddressGroup
                          from address in EmployeeAddressGroup.DefaultIfEmpty()
                          select new DashboardMonthlyModel { MonthName = month, Count = address != null ? address.Count : 0 }).ToList();

            return months;
        }

        public List<string> GetMonths() {
            List<string> months= new List<string>();
            months.Add("January");
            months.Add("February");
            months.Add("March");
            months.Add("April");
            months.Add("May");
            months.Add("June");
            months.Add("July");
            months.Add("August");
            months.Add("September");
            months.Add("October");
            months.Add("November");
            months.Add("December");
            return months;
        }

        public async Task<int> GetCountBooking(int year, string dealerId)
        {
            var query = _appContext.BikeBookings.Include(m => m.SaleBikes).ThenInclude(m => m.ShowRoom).Where(m => m.CreatedDate.Year == year && m.IsActive == true);

            if (!string.IsNullOrEmpty(dealerId))
            {
                query = query.Where(m => m.SaleBikes.ShowRoom.UserId.ToString() == dealerId);
            }

            return await query.CountAsync();

        }
    }
}
