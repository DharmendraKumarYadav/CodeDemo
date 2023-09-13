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
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecificationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;

        public SpecificationController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<SpecificationController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpGet("specification/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<SpecificationViewModel>))]
        public async Task<IActionResult> GetSpecification(int pageNumber, int pageSize)
        {
            var attributes = await _unitOfWork.SpecificationService.GetSpecification(pageNumber, pageSize);

            var mappedObject = _mapper.Map<List<SpecificationViewModel>>(attributes);
            foreach (var item in mappedObject)
            {
                item.Images = await _unitOfWork.DocumentService.GetDocumentImage(item.ImageId);
            }

            return Ok(mappedObject);
        }

        [HttpPost("specification")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateSpecification([FromBody] SpecificationModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                

                var documentId = _unitOfWork.DocumentService.UploadDocument(model.Files).FirstOrDefault();

                var specification = _mapper.Map<Specification>(model);
                specification.ImageId = documentId;

                _unitOfWork.SpecificationService.Add(specification);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("specification/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateSpecification(int id, [FromBody] SpecificationModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting attribute id in parameter and model data");



                var dataObject = await _unitOfWork.SpecificationService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);

                //_mapper.Map<SpecificationModel, DAL.Models.Entity.Specification>(model, dataObject);
                if (model.Files.Count > 0)
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




                _unitOfWork.SpecificationService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("specification/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSpecification(int id)
        {
            var specification = await _unitOfWork.SpecificationService.GetAsync(id);

            if (specification == null)
                return NotFound(id);

            var document = await _unitOfWork.DocumentService.GetAsync(specification.ImageId);
            if (document == null)
                _unitOfWork.DocumentService.Remove(document);

            _unitOfWork.SpecificationService.Remove(specification);
            _unitOfWork.SaveChanges();
            return NoContent();
        }
    }
}
