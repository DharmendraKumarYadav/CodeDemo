using AutoMapper;
using BDB.Authorization;
using BDB.Helpers.Common;
using BDB.ViewModels;
using BDB.ViewModels.Bike;
using Core.Enums;
using Core.Models;
using DAL;
using DAL.Core;
using DAL.Core.Interfaces;
using DAL.Models.Entity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitechMedical.Helpers;
using static IdentityServer4.Models.IdentityResources;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IReportService _reportService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountManager _accountManager;

        public ReportController(ILogger<ReportController> logger, IAccountManager accountManager, IReportService reportService, IUnitOfWork unitOfWork)
        {

            _logger = logger;
            _reportService = reportService;
            _unitOfWork = unitOfWork;
            _accountManager = accountManager;
        }
        [HttpGet("city")]
        [ProducesResponseType(200, Type = typeof(List<ReportDropDown>))]
        public async Task<IActionResult> GetCity()
        {
            List<ReportDropDown> reportDropDowns = new List<ReportDropDown>();
            var data =await _unitOfWork.CityService.GetAllAsync();
            foreach (var item in data)
            {
                reportDropDowns.Add(new ReportDropDown { 
                Id=item.Id.ToString(), Name=item.Name,
                });
            }
            return Ok(reportDropDowns);
        }

        [HttpGet("showroom/{dealerId}")]
        [ProducesResponseType(200, Type = typeof(List<ReportDropDown>))]
        public async Task<IActionResult> GetShowRoom(string dealerId)
        {
            List<ReportDropDown> reportDropDowns = new List<ReportDropDown>();
            var data =  _unitOfWork.ShowRoomService.Find(m=>m.UserId.ToString()== dealerId);
            foreach (var item in data)
            {
                reportDropDowns.Add(new ReportDropDown
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                });
            }
            return Ok(reportDropDowns);
        }

        [HttpGet("dealer")]
        [ProducesResponseType(200, Type = typeof(List<ReportDropDown>))]
        public async Task<IActionResult> GetDealer()
        {
            var data = await _accountManager.GetDealerData();
            return Ok(data);
        }

        [HttpPost("booking")]
        [ProducesResponseType(200, Type = typeof(List<ReportBookingModel>))]
        public async Task<IActionResult> BookingReport(BookingFilter bookingFilter)
        {
            string dealerId = null;
            var IsDealer = this.User.IsInRole(AppUserRoles.Dealer);
            if (IsDealer)
            {
                dealerId = Utilities.GetUserId(this.User);
            }
            var data = await _reportService.GetBikeBooking(bookingFilter,dealerId);
            return Ok(data);
        }

    }
}
