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
    public class BrandController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public BrandController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<BrandController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("brand/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<BrandViewModel>))]
        public async Task<IActionResult> GetBrand(int pageNumber, int pageSize)
        {

            var dataList = await _unitOfWork.BrandService.GetBrand(pageNumber, pageSize);
            var mappedObject = _mapper.Map<List<BrandViewModel>>(dataList);
            foreach (var item in mappedObject)
            {
                item.Images = await _unitOfWork.DocumentService.GetDocumentImage(item.ImageId);
            }

            return Ok(mappedObject);
        }



        [HttpPost("brand")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBrand([FromBody] BrandModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var documentId = _unitOfWork.DocumentService.UploadDocument(model.Files).FirstOrDefault();

                var data = _mapper.Map<Brand>(model);
                data.ImageId = documentId;
                _unitOfWork.BrandService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("brand/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting attribute id in parameter and model data");



                var dataObject = await _unitOfWork.BrandService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);

                if (model.Files.Count > 0) {
                    var docObject =await _unitOfWork.DocumentService.GetAsync(dataObject.ImageId);
                    if (docObject != null)
                    {
                        _unitOfWork.DocumentService.UpdateDocument(model.Files.FirstOrDefault(), docObject.Id);
                    }
                    else {
                        var documentId = _unitOfWork.DocumentService.UploadDocument(model.Files).FirstOrDefault();
                        dataObject.ImageId = documentId;
                    }
                }
                dataObject.Name= model.Name;
                dataObject.Description= model.Description;
                _unitOfWork.BrandService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("brand/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var dataObject = await _unitOfWork.BrandService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);


            var document =await _unitOfWork.DocumentService.GetAsync(dataObject.ImageId);
            if (document == null)
                _unitOfWork.DocumentService.Remove(document);

            _unitOfWork.BrandService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }
    }
}
