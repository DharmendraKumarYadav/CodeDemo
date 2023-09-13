using AutoMapper;
using BDB.Authorization;
using BDB.Helpers;
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
    public class BikeBookingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;
        private readonly IEmailService _emailer;
        private SmtpConfig _config;
        readonly ISmsService _smsService;
        public BikeBookingController(IMapper mapper, IAccountManager accountManager, IOptions<AppSettings> config, ILogger<BikeBookingController> logger, IEmailService emailSender, IUnitOfWork unitOfWork, ISmsService smsService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailer = emailSender;
            _accountManager = accountManager;
            _smsService = smsService;
            _config = config.Value.SmtpConfig;
        }

        [HttpGet("bookings/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeBookingModel>))]
        public async Task<IActionResult> GetBikeBookig(int pageNumber, int pageSize)
        {
            string dealerId = null;
            var IsDealer = this.User.IsInRole(AppUserRoles.Dealer);
            if (IsDealer)
            {
                dealerId = Utilities.GetUserId(this.User);
            }

            var dataList = await _unitOfWork.BikeBookingService.GetBikeBookings(pageNumber, pageSize, dealerId);
            var mappedObject = _mapper.Map<List<BikeBookingModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpPost("bookings")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBikeBooking([FromBody] BikeBookingModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");
                var bikeEntity = _mapper.Map<BikeBooking>(model);
                bikeEntity.PaymentMethodSystemName = "Admin Booking";
                bikeEntity.PaymentStatusId = Convert.ToInt32(PaymentStatus.Pending);
                _unitOfWork.BikeBookingService.Add(bikeEntity);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("booking-create")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> BikeBookingCustomer([FromBody] BookingOrderRequest model)
        {
            var userId = Utilities.GetUserId(this.User);
            if (userId != null)
            {
                var response = new BookingOrderResponse();
                if (ModelState.IsValid)
                {
                    if (model == null)
                        return BadRequest($"{nameof(model)} cannot be null");

                    var dealerSaleBikes = await _unitOfWork.SaleBikeService.GetAsync(model.DealerBikeId);
                    if (dealerSaleBikes != null && dealerSaleBikes.Status == 1)
                    {
                        var bookingObj = new BikeBooking();

                        //Get User Details
                        var user = await _accountManager.GetUserByIdAsync(userId);
                        if (user != null)
                        {
                            bookingObj.FullName = user.FullName;
                            bookingObj.Email = user.Email;
                            bookingObj.PhoneNumber = user.PhoneNumber;
                            bookingObj.Address1 = "";
                            bookingObj.Address2 = "";
                            bookingObj.City = "";
                            bookingObj.State = "";
                            bookingObj.PostalCode = "";
                            bookingObj.BookedBy = userId;
                            bookingObj.UserId=userId;
                            
                        }
                    
                     
                        var bikeVariantDetails = await _unitOfWork.BikeVariantsService.GetAsync(dealerSaleBikes.BikeVariantsId);
                        if (bikeVariantDetails != null)
                        {
                            var bikeDetails = await _unitOfWork.BikeService.GetAsync(bikeVariantDetails.BikeId);
                            if (bikeDetails != null)
                            {
                                bookingObj.BikeName= bikeDetails.Name;
                                bookingObj.BikePrice = bikeDetails.Price.ToString();
                                bookingObj.BikeVariantName = bikeVariantDetails.Name;
                                bookingObj.BikeSpecification = bikeVariantDetails.Specification;
                                bookingObj.IsElectricBike = bikeDetails.IsElectricBike;
                                bookingObj.Displcaement = bikeDetails.Displacement.ToString();
                                bookingObj.ChesisNumber = dealerSaleBikes.ChesisNumber;
                                bookingObj.EngineNumber = dealerSaleBikes.EngineNumber;
                                bookingObj.VariantPrice = dealerSaleBikes.Price.ToString();
                                bookingObj.ExShowRoomPrice = dealerSaleBikes.ExShowRoomPrice.ToString();
                                bookingObj.RTOPrice = dealerSaleBikes.RTOPrice.ToString();
                                bookingObj.InsurancePrice = dealerSaleBikes.InsurancePrice.ToString();
                                bookingObj.BookingPrice = dealerSaleBikes.BookingAmount.ToString();
                                bookingObj.Amount = dealerSaleBikes.BookingAmount.ToString();

                                var colurService = await _unitOfWork.ColourService.GetAsync(dealerSaleBikes.BikeColurId);
                                if (colurService != null) {
                                    bookingObj.BikeColour = colurService.Name;
                                }
                                var categoryService = await _unitOfWork.CategoryService.GetAsync(bikeDetails.CategoryId);
                                if (categoryService != null)
                                {
                                    bookingObj.Category = categoryService.Name;
                                }
                                var brandService = await _unitOfWork.BrandService.GetAsync(bikeDetails.BrandId);
                                if (brandService != null)
                                {
                                    bookingObj.Brand = brandService.Name;
                                }
                                var bodyStyleService = await _unitOfWork.BodyStyleService.GetAsync(bikeDetails.BodyStyleId);
                                if (bodyStyleService != null)
                                {
                                    bookingObj.BodyStyle = bodyStyleService.Name;
                                }

                                //Show Room Details
                                var showRoomService = await _unitOfWork.ShowRoomService.GetAsync(dealerSaleBikes.ShowRoomId);
                                if (showRoomService != null)
                                {
                                    bookingObj.ShowRoomName = showRoomService.Name;
                                    bookingObj.ShowRoomEmail = showRoomService.EmailId;
                                    bookingObj.ShowRoomMobileNumber = showRoomService.MobileNumber;
                                    bookingObj.ShowRoomPhoneNumber = showRoomService.PhoneNumber;
                                    bookingObj.ShowRoomAddressLine1 = showRoomService.AddressLine1;
                                    bookingObj.ShowRoomAddressLine2 = showRoomService.AddressLine2;


                                    var areaService = await _unitOfWork.AreaService.GetAsync(showRoomService.AreaId);
                                    if (areaService != null) {
                                        bookingObj.ShowRoomArea = areaService.Name;
                                        bookingObj.ShowRoomPinCode = areaService.PinCode;
                                        var cityService = await _unitOfWork.CityService.GetAsync(areaService.CityId);
                                        if (cityService != null)
                                        {
                                            bookingObj.ShowRoomCity= cityService.Name;
                                        }
                                        bookingObj.Url = showRoomService.UrlLink;
                                    }

                                    //Get Show Room Delaer/Broker
                                    var dealerOrBroker = await _accountManager.GetUserByIdAsync(showRoomService.UserId.ToString());
                                    if (dealerOrBroker != null)
                                    {
                                        bookingObj.ShowRoomOwnerName = user.FullName;
                                        bookingObj.ShowRoomOwnerEmail = user.Email;
                                        bookingObj.ShowRoomOwnerMobile = user.PhoneNumber;
                                        bookingObj.DelaerOrBrokerId = dealerOrBroker.Id.ToString();
                                    }

                                }

                            }
                        }

                        bookingObj.SaleBikeId = model.DealerBikeId;
                        bookingObj.UserId = Utilities.GetUserId(this.User);
                        bookingObj.PaymentStatusId = Convert.ToInt32(PaymentStatus.Pending);//Chnange //Chnage To pending after payment Integration
                        bookingObj.BookingStatusId = Convert.ToInt32(BookingStatus.Requested);//Chnage To pending after payment Integration
                        _unitOfWork.BikeBookingService.Add(bookingObj);
                        await _unitOfWork.SaveChangesAsync();
                        response.Email = bookingObj.Email;
                        response.Mobile = bookingObj.PhoneNumber;
                        response.BookingNumber = bookingObj.BookingNumber;
                        response.Amount = bookingObj.Amount;
                        response.FirstName = bookingObj.FullName;
                        response.BookingNumber = bookingObj.BookingNumber;

                        //Once Payment gateway Added Remove From here
                        //Start
                        var userDetails = await _accountManager.GetUserByIdAsync(bookingObj.UserId);

                        if (userDetails != null)
                        {
                            //Email:-
                            var dealerBikesEntity = await _unitOfWork.SaleBikeService.GetAsync(bookingObj.SaleBikeId);
                            if (dealerBikesEntity != null)
                            {
                                var showRoom = await _unitOfWork.ShowRoomService.GetAsync(dealerBikesEntity.ShowRoomId);
                                if (showRoom != null)
                                {
                                    var areaname = "";
                                    var cityname = "";
                                    var data = _unitOfWork.BikeVariantsService.Find(m => m.Id == Convert.ToInt32(dealerBikesEntity.BikeVariantsId)).FirstOrDefault();
                                    var area = await _unitOfWork.AreaService.GetAsync(showRoom.AreaId);
                                    if (area != null)
                                    {
                                        areaname = area.Name;
                                        var city = await _unitOfWork.CityService.GetAsync(area.CityId);
                                        if (city != null)
                                        {
                                            cityname = city.Name;
                                        }
                                    }

                                    string customerMessage = OrderTemplate.SendOrderCreateCustomerEmailAsync(bookingObj.BookingNumber.ToString(), dealerSaleBikes.CreatedDate.ToShortDateString(), dealerSaleBikes.BookingAmount, showRoom.Name, (showRoom.AddressLine1 + ',' + showRoom.AddressLine2), (areaname + "," + cityname), "India", showRoom.Area?.PinCode, data.Name);
                                    string adminMessage = OrderTemplate.SendOrderCreateAdminEmailAsync(_config.AdminName, user.FullName, bookingObj.BookingNumber.ToString(), dealerSaleBikes.CreatedDate.ToShortDateString(), dealerSaleBikes.BookingAmount, showRoom.Name, (showRoom.AddressLine1 + ',' + showRoom.AddressLine2), (areaname + "," + cityname), "India", showRoom.Area?.PinCode, data.Name);
                                    await _emailer.SendEmailAsync(_config.AdminName, _config.AdminEmail, " New Booking  " + bookingObj.BookingNumber.ToString() + " recieved.", adminMessage);
                                    await _emailer.SendEmailAsync(user.FullName, user.Email, "Yay! Your Bike " + data.Name.ToString() + " booking " + bookingObj.BookingNumber.ToString() + " is confirmed.", customerMessage);

                                    //Sent Emailto Show Room Dealer
                                    if (showRoom != null)
                                    {

                                        var dealerUser = await _accountManager.GetUserByIdAsync(showRoom.UserId.ToString());
                                        if (user != null)
                                        {
                                            await _emailer.SendEmailAsync(dealerUser.FullName, dealerUser.Email, " New Booking  " + bookingObj.BookingNumber.ToString() + " recieved.", adminMessage);
                                        }

                                    }

                                    //Phone: -


                                    string phonemessage = OrderTemplate.SendOrderCreateCustomerSmsAsync(bookingObj.BookingNumber.ToString(), data.Name, dealerSaleBikes.BookingAmount.ToString());
                                    //await _smsService.SendAsync(user.PhoneNumber, phonemessage, _templateConfig.OrderPlacedTempId);


                                }
                            }

                        }
                        //End

                        //Update Sale Bike Status Booked:
                        dealerSaleBikes.Status = 2;
                        _unitOfWork.SaleBikeService.Update(dealerSaleBikes);
                        _unitOfWork.SaveChanges();
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest($"{nameof(model)} Bike Deatils not found or Not Available for booking");
                    }
                }

                return BadRequest(ModelState);
            }
            else
            {
                return Unauthorized();
            }

        }


        [HttpGet("mybookings")]
        [ProducesResponseType(200, Type = typeof(List<BikeBookingCustomerModel>))]
        public async Task<IActionResult> GetMyBikeBookig()
        {
            var userId = Utilities.GetUserId(this.User);
            if (userId != null)
            {
                var dataList = await _unitOfWork.BikeBookingService.GetMyBikeBookings(userId);
                return Ok(dataList);
            }
            else
            {
                return Unauthorized();
            }

        }
        [HttpGet("bookingdetails/{bookingId}")]
        [ProducesResponseType(200, Type = typeof(List<BikeBookingCustomerModel>))]
        public async Task<IActionResult> GetBikeBookigDetails(string bookingId)
        {
            var data = await _unitOfWork.BikeBookingService.GetBikeBookingDetail(bookingId);
            return Ok(data);
        }

        [HttpDelete("bookings/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var dataObject = await _unitOfWork.BikeBookingService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.BikeBookingService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }


        [HttpPost("booking-cancel")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CancelBooking([FromBody] BookingStatusRequest model)
        {
            var dataObject = await _unitOfWork.BikeBookingService.GetAsync(model.Id);

            if (dataObject == null)
                return NotFound(model.Id);

            var userId = Utilities.GetUserId(User);

            if (dataObject.UserId != userId)
                return BadRequest("Not authroized to cancel");

            if (dataObject.BookingStatusId != (int)(BookingStatus.Requested))
                return BadRequest("You can not cancel your booking, Please contact dealer.");

            dataObject.BookingStatusId = Convert.ToInt32(model.Status);
            _unitOfWork.BikeBookingService.Update(dataObject);

            var dealerBikes = await _unitOfWork.SaleBikeService.GetAsync(dataObject.SaleBikeId);
            if (dealerBikes != null)
            {
                dealerBikes.Status = 1;
                _unitOfWork.SaleBikeService.Update(dealerBikes);
            }

            _unitOfWork.SaveChanges();

            //Send Email To Customer and Dealer reagrding boking cancelled.

            return NoContent();
        }

        [HttpPost("booking-status")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateStatus([FromBody] BookingStatusRequest model)
        {
            var dataObject = await _unitOfWork.BikeBookingService.GetAsync(model.Id);

            if (dataObject == null)
                return NotFound(model.Id);

            if (dataObject.BookingStatusId == (int)(BookingStatus.Requested))
            {
                if (model.Status == (int)(BookingStatus.Cancelled))
                {
                    dataObject.BookingStatusId = model.Status;
                    //Send Email To Customer regarding  to booking cancelled

                    var dealerBikes = await _unitOfWork.SaleBikeService.GetAsync(dataObject.SaleBikeId);
                    if (dealerBikes != null)
                    {
                        dealerBikes.Status = 1;
                        _unitOfWork.SaleBikeService.Update(dealerBikes);
                    }

                    //Realease sales bike table for again booking
                }
                if (model.Status == (int)(BookingStatus.Confirm))
                {
                    dataObject.BookingStatusId = model.Status;
                    //Send Email To Customer regarding  to booking confirm
                }
            }

            if (dataObject.BookingStatusId == (int)(BookingStatus.Confirm))
            {
                if (model.Status == (int)(BookingStatus.Cancelled))
                {
                    dataObject.BookingStatusId = model.Status;
                    var dealerBikes = await _unitOfWork.SaleBikeService.GetAsync(dataObject.SaleBikeId);
                    if (dealerBikes != null)
                    {
                        dealerBikes.Status = 1;
                        _unitOfWork.SaleBikeService.Update(dealerBikes);
                    }
                    //Send Email To Customer regarding  to booking cancelled
                    //Realease sales bike table for again booking
                }
                if (model.Status == (int)(BookingStatus.Invoiced))
                {
                    dataObject.BookingStatusId = model.Status;
                    //Send Email To Customer regarding  to booking cancelled
                    //Realease sales bike table for again booking
                }
            }
            _unitOfWork.BikeBookingService.Update(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent(); ;
        }

       
    }
}
