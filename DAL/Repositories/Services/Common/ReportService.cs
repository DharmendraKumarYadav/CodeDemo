using Core.Enums;
using Core.Models;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Common
{
    public class ReportService : IReportService

    {
        private ApplicationDbContext _appContext;
        public ReportService(ApplicationDbContext context)
        {
            _appContext = context;
        }

        public async Task<List<ReportBookingModel>> GetBikeBooking(BookingFilter bookingFilter,string dealerId)
        {
            List<ReportBookingModel> reportBookingModels = new List<ReportBookingModel>();

            var query = _appContext.BikeBookings
                       .Include(m => m.SaleBikes)
                          .ThenInclude(m => m.ShowRoom).ThenInclude(m => m.Area).ThenInclude(m => m.City)
                             .Include(m => m.SaleBikes).ThenInclude(m => m.BikeVariants).ThenInclude(m => m.Bike).AsQueryable();

            if (!string.IsNullOrEmpty(dealerId))
            {
                query = query.Where(m => m.SaleBikes.ShowRoom.UserId.ToString() == dealerId);
            }

            if (bookingFilter != null)
            {
                if (!string.IsNullOrEmpty(bookingFilter.StartDate))
                {
                    query = query.Where(m => m.CreatedDate >= Convert.ToDateTime(bookingFilter.StartDate));
                }
                if (!string.IsNullOrEmpty(bookingFilter.EndDate))
                {
                    query = query.Where(m => m.CreatedDate <= Convert.ToDateTime(bookingFilter.EndDate));
                }
                if (!string.IsNullOrEmpty(bookingFilter.DealerId))
                {
                    query = query.Where(m => m.SaleBikes.ShowRoom.UserId.ToString() == bookingFilter.DealerId);
                }
                if (bookingFilter.CityId != 0)
                {
                    query = query.Where(m => m.SaleBikes.ShowRoom.Area.CityId == bookingFilter.CityId);
                }
                if (bookingFilter.ShowRoomId != 0)
                {
                    query = query.Where(m => m.SaleBikes.ShowRoom.Id == bookingFilter.ShowRoomId);
                }

                var result = await query.ToListAsync();
                foreach (var item in result)
                {
                    reportBookingModels.Add(
                        new ReportBookingModel {
                            BikeName = item.SaleBikes.BikeVariants.Name,
                            BookingAmount = item.BookingPrice,
                            BookingId=item.BookingNumber,
                            Email= item.Email,
                            FirstName= item.FullName,
                            LastName= "",
                            PaymentStatus=((PaymentStatus)item.PaymentStatusId).ToString(),
                            PhoneNumber = item.PhoneNumber,
                            BookingDate=item.CreatedDate.ToShortDateString()
                            
                        });
                }

            }

            return reportBookingModels;
        }
    }
}
