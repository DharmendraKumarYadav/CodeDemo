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
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public CategoryController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<CategoryController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("category/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<CategoryModel>))]
        public async Task<IActionResult> GetCategory(int pageNumber, int pageSize)
        {
            var dataList = await _unitOfWork.CategoryService.GetCategory(pageNumber, pageSize);
            return Ok(_mapper.Map<List<CategoryModel>>(dataList));
        }



        [HttpPost("category")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


              var data = _mapper.Map<Category>(model);

                _unitOfWork.CategoryService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("category/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting attribute id in parameter and model data");



                var dataObject = await _unitOfWork.CategoryService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);


                _mapper.Map<CategoryModel,Category>(model, dataObject);

                _unitOfWork.CategoryService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("category/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var dataObject = await _unitOfWork.CategoryService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.CategoryService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }
    }
}
