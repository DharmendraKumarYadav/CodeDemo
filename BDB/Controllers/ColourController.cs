using AutoMapper;
using BDB.Helpers;
using BDB.ViewModels.Account;
using BDB.ViewModels.Attribute;
using BDB.ViewModels.BikeAttribute;
using DAL;
using DAL.Models.Entity;
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
    public class ColourController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public ColourController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<ColourController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("colour/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<ColourModel>))]
        public async Task<IActionResult> GetColour(int pageNumber, int pageSize)
        {
            var dataList = await _unitOfWork.ColourService.GetColour(pageNumber, pageSize);
            var resultList = _mapper.Map<List<ColourModel>>(dataList);
            foreach (var item in resultList)
            {
                item.Images = await _unitOfWork.DocumentService.GetDocumentImage(item.ImageId);
            }

            return Ok(resultList);
        }



        [HttpPost("colour")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateColour([FromBody] ColourModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var documentId = _unitOfWork.DocumentService.UploadDocument(model.Files).FirstOrDefault();
                var data = _mapper.Map<Colour>(model);
                data.ImageId=documentId;
                _unitOfWork.ColourService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("colour/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateColour(int id, [FromBody] ColourModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting attribute id in parameter and model data");


                var dataObject = await _unitOfWork.ColourService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);


                if (model.Files?.Count > 0)
                {
                    var docObject =await _unitOfWork.DocumentService.GetAsync(dataObject.ImageId);
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
                dataObject.Name = model.Name;
                dataObject.Description = model.Description;
                _unitOfWork.ColourService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("colour/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteColour(int id)
        {
            var dataObject = await _unitOfWork.ColourService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.ColourService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }
    }
}
