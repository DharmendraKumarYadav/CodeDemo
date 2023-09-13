using AutoMapper;
using BDB.Helpers;
using BDB.ViewModels.Account;
using BDB.ViewModels.Attribute;
using BDB.ViewModels.BikeAttribute;
using DAL;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BodyStyleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public BodyStyleController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<BodyStyleController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("bodystyle/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<BodyStyleModel>))]
        public async Task<IActionResult> GetBodyStyle(int pageNumber, int pageSize)
        {
            var dataList = await _unitOfWork.BodyStyleService.GetBodyStyle(pageNumber, pageSize);
            var mapedObject = _mapper.Map<List<BodyStyleModel>>(dataList);
            foreach (var item in mapedObject)
            {
                item.Images = await _unitOfWork.DocumentService.GetDocumentImage(item.ImageId);
            }
            return Ok(mapedObject);
        }



        [HttpPost("bodystyle")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBodyStyle([FromBody] BodyStyleModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


                var documentId = _unitOfWork.DocumentService.UploadDocument(model.Files).FirstOrDefault();
                var data = _mapper.Map<BodyStyle>(model);
                data.ImageId= documentId;
                _unitOfWork.BodyStyleService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("bodystyle/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBodyStyle(int id, [FromBody] BodyStyleModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting attribute id in parameter and model data");



                var dataObject = await _unitOfWork.BodyStyleService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);


                _mapper.Map<BodyStyleModel,BodyStyle>(model, dataObject);

                if (model.Files.Count > 0)
                {
                    var docObject = await _unitOfWork.DocumentService.GetAsync(dataObject.ImageId);
                    if (docObject != null)
                    {
                        _unitOfWork.DocumentService.UpdateDocument(model.Files.FirstOrDefault(), docObject.Id);
                    }
                    else
                    {
                        var documentId = _unitOfWork.DocumentService.UploadDocument(model.Files).FirstOrDefault();
                        dataObject.ImageId = documentId;
                    }
                }
                _unitOfWork.BodyStyleService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("bodystyle/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBodyStyle(int id)
        {
            var dataObject = await _unitOfWork.BodyStyleService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.BodyStyleService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }
    }
}
