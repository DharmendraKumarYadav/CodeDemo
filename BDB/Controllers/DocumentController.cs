using AutoMapper;
using BDB.Helpers;
using BDB.ViewModels;
using BDB.ViewModels.Account;
using BDB.ViewModels.Attribute;
using BDB.ViewModels.BikeAttribute;
using DAL;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public DocumentController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<DocumentController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("document/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetDocument(int id)
        {
            var dataObject = await _unitOfWork.DocumentService.GetAsync(id);

            if (dataObject == null)
                return NotFound();

            return File(dataObject.Data, dataObject.FileType, dataObject.Name);

            //return Ok(_mapper.Map<DocumentModel>(dataObject));
        }

        [HttpGet("image/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetImage(int id)
        {
            var imageObject = await _unitOfWork.DocumentService.GetDocumentImage(id);
            return Ok(imageObject);
        }

        [HttpPost("upload")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadDocument([FromBody] DocumentModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


                var data = _mapper.Map<Document>(model);

                _unitOfWork.DocumentService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("upload/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] DocumentModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting attribute id in parameter and model data");



                var dataObject = await _unitOfWork.DocumentService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);


                _mapper.Map<DocumentModel, Document>(model, dataObject);

                _unitOfWork.DocumentService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("document/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var dataObject = await _unitOfWork.DocumentService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.DocumentService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

     
    }
}
