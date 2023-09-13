using AutoMapper;
using BDB.Helpers;
using BDB.Helpers.Common;
using BDB.Helpers.MessageService;
using Core.Models;
using DAL;
using DAL.Repositories.Services.SignalIR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnitechMedical.Helpers;
using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Messages.ReplyRequests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Webhook;

namespace BDB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;
        private readonly IWhatsAppService _whatsAppService;
        private string VerifyToken = "meatyhamhock";
        private List<TextMessage> textMessage;

        public NotificationController(IHubContext<BroadcastHub, IHubClient> hubContext, IOptions<AppSettings> config, IWhatsAppService whatsAppService, IMapper mapper, IUnitOfWork unitOfWork, ILogger<NotificationController> logger, IEmailService emailSender)
        {
            _hubContext = hubContext;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
            _whatsAppService=whatsAppService;
        }

        [AllowAnonymous]
        [Route("notifications")]
        [HttpGet]
        public async Task<ActionResult<NotificationViewModel>> GetNotificationCount()
        {
            var userId = Utilities.GetUserId(this.User);
            var result = await _unitOfWork.NotificationService.GetNotifications(new System.Guid(userId));
            return result;
        }

        [HttpDelete]
        [Route("clearnotifications")]
        public async Task<IActionResult> ClearNotifications()
        {
            var userId = Utilities.GetUserId(this.User);
           await _unitOfWork.NotificationService.ClearNotification(new System.Guid(userId));
            return NoContent();
        }
        [HttpDelete]
        [Route("deletenotifications/{id:int}")]
        public async Task<IActionResult> DeleteNotifications(int id)
        {
            var userId = Utilities.GetUserId(this.User);
            var notifications =await _unitOfWork.NotificationService.GetAsync(id);
            if (notifications != null) {
                 _unitOfWork.NotificationService.Remove(notifications);
                await _unitOfWork.SaveChangesAsync();
            }
        
            return NoContent();
        }
    }
}
