using AutoMapper;
using BDB.Helpers;
using BDB.ViewModels.Account;
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
    public class AttributeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public AttributeController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<AttributeController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("attribute/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<AttributeModel>))]
        public async Task<IActionResult> GetAttributes(int pageNumber, int pageSize)
        {
            var attributes = await _unitOfWork.AttributeService.GetAttributes(pageNumber, pageSize);
            return Ok(_mapper.Map<List<AttributeModel>>(attributes));
        }



        [HttpPost("attributes")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAttribute([FromBody] AttributeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                DAL.Models.Entity.Attributes bikeAttribute = _mapper.Map<DAL.Models.Entity.Attributes>(model);
             
                _unitOfWork.AttributeService.Add(bikeAttribute);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("attributes/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAttribute(int id, [FromBody] AttributeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting attribute id in parameter and model data");



                var bikeAttribute = await _unitOfWork.AttributeService.GetAsync(id);

                if (bikeAttribute == null)
                    return NotFound(id);


                _mapper.Map<AttributeModel, DAL.Models.Entity.Attributes>(model, bikeAttribute);

                _unitOfWork.AttributeService.Update(bikeAttribute);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("attribute/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAttribute(int id)
        {
            var bikeAttribute = await _unitOfWork.AttributeService.GetAsync(id);

            if (bikeAttribute == null)
                return NotFound(id);


            _unitOfWork.AttributeService.Remove(bikeAttribute);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }


    }
}
