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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Linq;
using System.Threading.Tasks;
using UnitechMedical.Helpers;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashBoardController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;


        public DashBoardController(IAccountManager accountManager, ILogger<DashBoardController> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _accountManager = accountManager;

        }

        [HttpGet("detail")]
        [ProducesResponseType(200, Type = typeof(DashboardViewModel))]
        public async Task<IActionResult> GetCount()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();

            string userId = null;
            var IsDealer = this.User.IsInRole(AppUserRoles.Dealer) || this.User.IsInRole(AppUserRoles.Broker); 
            if (IsDealer)
            {
                userId = Utilities.GetUserId(this.User);
            }
            //Customer
            dashboardViewModel.CountModel.CustomerCount = await _accountManager.GetUserCount();
            dashboardViewModel.MonthlyCustomer = await _accountManager.GetMonthlyUser(DateTime.Now.Year);

            //Booking

            dashboardViewModel.CountModel.BookingCount = await _unitOfWork.BikeBookingService.GetCountBooking(DateTime.Now.Year, userId);
            dashboardViewModel.MonthlyBooking = await _unitOfWork.BikeBookingService.GetMonthlyBooking(DateTime.Now.Year, userId);

            //Bikes:-
            dashboardViewModel.CountModel.BikeCount = await _unitOfWork.BikeService.GetBikeCount();
            dashboardViewModel.CountModel.SaleBikeCount = await _unitOfWork.BikeService.GetSalesBikeCount(userId);

            return Ok(dashboardViewModel);
        }

    }
}
