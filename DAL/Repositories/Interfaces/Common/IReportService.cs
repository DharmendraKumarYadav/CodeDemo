using Core.Models;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IReportService 
    {
        Task<List<ReportBookingModel>> GetBikeBooking(BookingFilter bookingFilter,string dealerId);
    }
}
