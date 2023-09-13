using AutoMapper;
using BDB.Authorization;
using BDB.Helpers;
using BDB.Helpers.Common;
using BDB.Helpers.MessageService;
using BDB.ViewModels;
using BDB.ViewModels.Account;
using BDB.ViewModels.Attribute;
using BDB.ViewModels.BikeAttribute;
using BDB.ViewModels.Common;
using Core.Enums;
using Core.Models;
using DAL;
using DAL.Core.Interfaces;
using DAL.Models.Entity;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShowRoomController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;
        private readonly IWhatsAppService _whatsAppService;
        private readonly IPdfConverterService _pdfConverterService;
        private readonly IAccountManager _accountManager;


        public ShowRoomController(IAccountManager accountManager, IPdfConverterService pdfConverterService, IWhatsAppService whatsAppService, IConverter converter, IMapper mapper, IUnitOfWork unitOfWork, ILogger<ShowRoomController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _whatsAppService = whatsAppService;
            _pdfConverterService = pdfConverterService;
            _emailSender = emailSender;
            _accountManager = accountManager;
        }

        #region ShowRooms

        [HttpGet("showroomsapproved/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<ShowRoomViewModel>))]
        public async Task<IActionResult> GetShowRooms(int pageNumber, int pageSize)
        {
            string userId = null;
            if (this.User.IsInRole(AppUserRoles.Dealer) || this.User.IsInRole(AppUserRoles.Broker))
            {
                userId = Utilities.GetUserId(this.User);
            }
            var dataList = await _unitOfWork.ShowRoomService.GetShowRoom(pageNumber, pageSize, userId);
            var mappedObject = _mapper.Map<List<ShowRoomViewModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpGet("showrooms/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<ShowRoomViewModel>))]
        public async Task<IActionResult> GetShowRoomsAdmin(int pageNumber, int pageSize)
        {
            string userId = null;
            if (this.User.IsInRole(AppUserRoles.Dealer) || this.User.IsInRole(AppUserRoles.Broker))
            {
                userId = Utilities.GetUserId(this.User);
            }
            var dataList = await _unitOfWork.ShowRoomService.GetShowRoomAdmin(pageNumber, pageSize, userId);
            var mappedObject = _mapper.Map<List<ShowRoomViewModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpPost("showroom")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateShowRoom([FromBody] ShowRoomModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var data = _mapper.Map<ShowRoom>(model);
                data.UserId = new Guid(model.DealerId);
                data.IsActive = true;
                data.Status = Convert.ToInt32(ShowRoomStatusEnum.Pending);
                _unitOfWork.ShowRoomService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                var adminList = await _accountManager.GetAdminstrator();
                foreach (var item in adminList)
                {
                    Notification notification = new Notification()
                    {
                        Name = "Reuest for Show room approval",
                        Description = "Broker or Dealer created show room",
                        Status = 1,
                        Type = "ShowRoomRequest",
                        Url = NotificationUrl.ShowRoomApprove,
                        UserId = item.Id
                    };
                    await _unitOfWork.NotificationService.CreateNotification(notification);
                }

               

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("showroom/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateShowRoom(int id, [FromBody] ShowRoomModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting  id in parameter and model data");

                var dataObject = await _unitOfWork.ShowRoomService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);


                _mapper.Map<ShowRoomModel, ShowRoom>(model, dataObject);
                dataObject.Status = 1;
                _unitOfWork.ShowRoomService.Update(dataObject);
                _unitOfWork.SaveChanges();

                var adminList = await _accountManager.GetAdminstrator();
                foreach (var item in adminList)
                {
                    Notification notification = new Notification()
                    {
                        Name = "Reuest for Show room approval",
                        Description = "Broker or Dealer updated show room",
                        Status = 1,
                        Type = "ShowRoomRequest",
                        Url = NotificationUrl.ShowRoomApprove,
                        UserId = item.Id
                    };
                    await _unitOfWork.NotificationService.CreateNotification(notification);
                }
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpPost("showroomauthorize")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AuthroizeShowRoom([FromBody] ShowRoomAuthroizeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


                var dataObject = await _unitOfWork.ShowRoomService.GetAsync(model.Id);

                if (dataObject == null)
                    return NotFound(model.Id);


                dataObject.AuthorizeDate = DateTime.Now;
                dataObject.AuthorizedBy = Utilities.GetUserId(this.User);
                dataObject.Status = model.Status;
                dataObject.Comments = model.Comments;
                _unitOfWork.ShowRoomService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("showroom/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteShowRoom(int id)
        {
            var dataObject = await _unitOfWork.ShowRoomService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);


            _unitOfWork.ShowRoomService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        #endregion


        #region Broker

        [HttpGet("dealers")]
        [ProducesResponseType(200, Type = typeof(List<DealerBrokerModel>))]
        public async Task<IActionResult> GetDealers()
        {
            string userId = null;
            if (this.User.IsInRole(AppUserRoles.Broker))
            {
                userId = Utilities.GetUserId(this.User);
            }
            var dataList = await _unitOfWork.DealerBrokerService.GetDealers(userId);
            return Ok(dataList);
        }
        [HttpGet("brokers")]
        [ProducesResponseType(200, Type = typeof(List<DealerBrokerModel>))]
        public async Task<IActionResult> GetBrokers()
        {
            string userId = null;
            if (this.User.IsInRole(AppUserRoles.Dealer))
            {
                userId = Utilities.GetUserId(this.User);
            }
            var dataList = await _unitOfWork.DealerBrokerService.GetBrokers(userId);
            return Ok(dataList);
        }

        [HttpGet("dealerbikes/{dealerId}")]
        [ProducesResponseType(200, Type = typeof(List<DealerBikeViewModel>))]
        public async Task<IActionResult> GetShowRoomBike(string dealerId)
        {
            var dataList = await _unitOfWork.SaleBikeService.GetDealerBikeAvailableForSale(dealerId);
            var mappedObject = _mapper.Map<List<DealerBikeViewModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpGet("bikerequest/{id:int}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RequestBike(int id)
        {
            if (ModelState.IsValid)
            {


                var saleBike = await _unitOfWork.SaleBikeService.GetAsync(id);

                if (saleBike == null)
                {
                    return BadRequest($"Bike not found for authrization.");
                }

                if (saleBike.IsTransferred)
                {
                    return BadRequest($"Bike is already transfered to other broker.");
                }
                if (!saleBike.IsTransferable)
                {
                    return BadRequest($"Bike is not eligible for transfer");
                }
                //&& m.Status == 2
                var showRoomCheck = _unitOfWork.ShowRoomService.Find(m => m.UserId.ToString() == Utilities.GetUserId(this.User));

                if (showRoomCheck.Count() == 0)
                {
                    return BadRequest($"Please add showroom details before requesting bike.");
                }
                saleBike.TransferRequestedBy = Utilities.GetUserId(this.User);
                saleBike.TransferRequestedDate = DateTime.Now;
                saleBike.TransferStatus = Convert.ToInt32(TransferStatus.Requested);
                _unitOfWork.SaleBikeService.Update(saleBike);
                await _unitOfWork.SaveChangesAsync();

                Notification notification = new Notification()
                {
                    Name = "Reuest for bike approval",
                    Description = "Broker requested for bike approval to dealer",
                    Status = 1,
                    Type = "BikeRequest",
                    Url = NotificationUrl.DealerBikeApprove,
                    UserId = new Guid(saleBike.DealerId)
                };
                await _unitOfWork.NotificationService.CreateNotification(notification);
               
                _pdfConverterService.CreatePdf("<h1>Testing Data</h1>");
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("returnrequest/{id:int}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReturnRequest(int id)
        {
            if (ModelState.IsValid)
            {
                var saleBike = await _unitOfWork.SaleBikeService.GetAsync(id);
                if (saleBike == null)
                {
                    return BadRequest($"Bike not found for authorization.");
                }
                saleBike.ReturnBy = Utilities.GetUserId(this.User);
                saleBike.ReturnDate = DateTime.Now;
                saleBike.TransferStatus = Convert.ToInt32(TransferStatus.Returned);
                saleBike.IsTransferred = false;
                saleBike.ShowRoomId = saleBike.DealerShowRoomId;
                _unitOfWork.SaleBikeService.Update(saleBike);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        #endregion

        #region DealerShowRoomBike

        [HttpGet("salebikes/{userId}/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<DealerBikeViewModel>))]
        public async Task<IActionResult> GetShowRoomBrokerBike(string userId,int pageNumber, int pageSize)
        {
            var dataList = await _unitOfWork.SaleBikeService.GetSaleBike(pageNumber, pageSize, userId);
            var mappedObject = _mapper.Map<List<DealerBikeViewModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpGet("bookedbikes/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<DealerBikeViewModel>))]
        public async Task<IActionResult> GetShowRoomBookedBikes(int pageNumber, int pageSize)
        {
            string userId = null;
            if (this.User.IsInRole(AppUserRoles.Dealer) || this.User.IsInRole(AppUserRoles.Broker))
            {
                userId = Utilities.GetUserId(this.User);
            }
            var dataList = await _unitOfWork.SaleBikeService.GetBookedBike(pageNumber, pageSize, userId);
            var mappedObject = _mapper.Map<List<DealerBikeViewModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpPut("salebike/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateSaleBike(int id, [FromBody] DealerBikeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting  id in parameter and model data");



                var dataObject = await _unitOfWork.SaleBikeService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);

                if (!(dataObject.ChesisNumber == model.ChesisNumber && dataObject.EngineNumber == model.EngineNumber))
                {
                    var checkChesisandEngine = _unitOfWork.SaleBikeService.Find(m => m.ChesisNumber == model.ChesisNumber || m.EngineNumber == model.EngineNumber).FirstOrDefault();

                    if (checkChesisandEngine != null)
                    {
                        return BadRequest($"Bike chesis number or engine number already exist.");
                    }
                }
                _mapper.Map<DealerBikeModel, SaleBike>(model, dataObject);
                dataObject.Price = (Convert.ToInt32(dataObject.InsurancePrice) + Convert.ToInt32(dataObject.RTOPrice) + Convert.ToInt32(dataObject.ExShowRoomPrice)).ToString();

                _unitOfWork.SaleBikeService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        #endregion
        #region DealerShowRoomBike

        [HttpGet("dealerbikes/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<DealerBikeViewModel>))]
        public async Task<IActionResult> GetShowRoomBike(int pageNumber, int pageSize)
        {
            string dealerId = null;
            var IsDealer = this.User.IsInRole(AppUserRoles.Dealer);
            if (IsDealer)
            {
                dealerId = Utilities.GetUserId(this.User);
            }

            var dataList = await _unitOfWork.SaleBikeService.GetSaleBike(pageNumber, pageSize, dealerId);
            var mappedObject = _mapper.Map<List<DealerBikeViewModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpGet("brokerrequested/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<DealerBikeViewModel>))]
        public async Task<IActionResult> GetBrokerRequestedBikes(int pageNumber, int pageSize)
        {
            string dealerId = null;
            var IsDealer = this.User.IsInRole(AppUserRoles.Dealer);
            if (IsDealer)
            {
                dealerId = Utilities.GetUserId(this.User);
            }

            var dataList = await _unitOfWork.SaleBikeService.GetBrokerRequestedBike(pageNumber, pageSize, dealerId);
            var mappedObject = _mapper.Map<List<DealerBikeViewModel>>(dataList);
            return Ok(mappedObject);
        }


        [HttpPost("authorizerequest")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AuthorizeRequest([FromBody] DealerBikeAuthroizeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var saleBike = await _unitOfWork.SaleBikeService.GetAsync(model.Id);

                if (saleBike == null)
                {
                    return BadRequest($"Bike not found for authrization.");
                }

                saleBike.TransferAuthorizeBy = Utilities.GetUserId(this.User);
                saleBike.TransferAuthorizeDate = DateTime.Now;
                saleBike.Remarks = model.Remarks;
                saleBike.TransferStatus = model.TransferStatus;
                saleBike.IsTransferred = true;
                if (model.TransferStatus == Convert.ToInt32(TransferStatus.Approved))
                {
                    var showRoomCheck = _unitOfWork.ShowRoomService.Find(m => m.UserId.ToString() == saleBike.TransferRequestedBy).OrderByDescending(m => m.CreatedDate);

                    if (showRoomCheck.Count() == 0)
                    {
                        return BadRequest($"Please ask broker to add showroom details.");
                    }
                    else
                    {
                        saleBike.ShowRoomId = showRoomCheck.FirstOrDefault().Id;
                        saleBike.Remarks = "Successfully transferd Bike from Dealer from there Show Room";
                    }
                }
            
                _unitOfWork.SaleBikeService.Update(saleBike);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("dealerbike")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateDealerBike([FromBody] DealerBikeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var checkChesisandEngine = _unitOfWork.SaleBikeService.Find(m => m.ChesisNumber == model.ChesisNumber || m.EngineNumber == model.EngineNumber).FirstOrDefault();

                if (checkChesisandEngine != null)
                {
                    return BadRequest($"Bike chesis number or engine number already exist.");
                }

                var data = _mapper.Map<SaleBike>(model);
                data.Price = (Convert.ToInt32(data.InsurancePrice) + Convert.ToInt32(data.RTOPrice) + Convert.ToInt32(data.ExShowRoomPrice)).ToString();
                data.DealerId = Utilities.GetUserId(this.User);
                data.Status = 1;
                data.DealerShowRoomId = model.ShowRoomId;
                _unitOfWork.SaleBikeService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("dealerbike/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateDealerBike(int id, [FromBody] DealerBikeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting  id in parameter and model data");



                var dataObject = await _unitOfWork.SaleBikeService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);

                if (!(dataObject.ChesisNumber == model.ChesisNumber && dataObject.EngineNumber == model.EngineNumber))
                {
                    var checkChesisandEngine = _unitOfWork.SaleBikeService.Find(m => m.ChesisNumber == model.ChesisNumber || m.EngineNumber == model.EngineNumber).FirstOrDefault();

                    if (checkChesisandEngine != null)
                    {
                        return BadRequest($"Bike chesis number or engine number already exist.");
                    }
                }
                _mapper.Map<DealerBikeModel, SaleBike>(model, dataObject);
                dataObject.Price = (Convert.ToInt32(dataObject.InsurancePrice) + Convert.ToInt32(dataObject.RTOPrice) + Convert.ToInt32(dataObject.ExShowRoomPrice)).ToString();

                _unitOfWork.SaleBikeService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("dealerbike/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteDealerBike(int id)
        {
            var dataObject = await _unitOfWork.SaleBikeService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);


            _unitOfWork.SaleBikeService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        #endregion



    }
}
