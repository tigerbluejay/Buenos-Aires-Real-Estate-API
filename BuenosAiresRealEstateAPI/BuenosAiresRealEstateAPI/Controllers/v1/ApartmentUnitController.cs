using AutoMapper;
using BuenosAiresRealEstate.API.Models.DTOs;
using BuenosAiresRealEstate.API.Models.Models;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;

namespace BuenosAiresRealEstate.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ApartmentUnitAPI")]
    [ApiVersion("1.0")]
    public class ApartmentUnitController : ControllerBase
    {
        private readonly ILogger<ApartmentUnitController> _logger;
        private readonly IApartmentUnitRepository _dbApartmentUnit;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        // we inject the ApartmentComplex repository because we'll need to
        // perform a check in the Update Method
        private readonly IApartmentComplexRepository _dbApartmentComplex;

        public ApartmentUnitController(ILogger<ApartmentUnitController> logger,
            IApartmentUnitRepository dbApartmentUnit, IMapper mapper, 
            IApartmentComplexRepository dbApartmentComplex)
        {
            _logger = logger;
            _dbApartmentUnit = dbApartmentUnit;
            _mapper = mapper;
            _response = new APIResponse();
            _dbApartmentComplex = dbApartmentComplex;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "Default30")] // Cache the response for 30 seconds
        public async Task<ActionResult<APIResponse>> GetApartmentUnits()
        {
            try
            {
                IEnumerable<ApartmentUnit> apartmentUnitList = await _dbApartmentUnit.GetAllAsync(includeProperties: "ApartmentComplex");
       
                _response.Result = _mapper.Map<List<ApartmentUnitDTO>>(apartmentUnitList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("An Error Occured. See Response Errors for Details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpGet("{id}", Name = "GetApartmentUnit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetApartmentUnit(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogError("You must provide an Id for the Apartment Unit");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "You must provide an Id for the Apartment Unit" };
                    return BadRequest(_response);
                }

                // obtain relevant apartment unit
                var apartmentUnit = await _dbApartmentUnit.GetAsync(x => x.ApartmentUnitId == id);

                // if it retrieves nothing
                if (apartmentUnit == null)
                {
                    _logger.LogError("Apartment Unit Could Not be Found");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // map to DTO
                _response.Result = _mapper.Map<ApartmentUnitDTO>(apartmentUnit);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _logger.LogError("An Error Occured. See Response Errors for Details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateApartmentUnit(
            [FromBody] ApartmentUnitCreateDTO apartmentUnitCreateDTO)
        {
            try
            {
                // Input validation
                if (apartmentUnitCreateDTO == null)
                {
                    _logger.LogError("You must provide apartment unit information to create the apartment unit.");
                    return BadRequest(apartmentUnitCreateDTO);
                }

                ApartmentUnit apartmentUnitExists;
                // here we try to find if the unit name the user entered already exists in db
                apartmentUnitExists = await _dbApartmentUnit.GetAsync(
                    u => u.ApartmentUnitId.ToLower() == apartmentUnitCreateDTO.ApartmentUnitId.ToLower());

                // if we can retrieve it, the apartment unit exists already
                if (apartmentUnitExists != null)
                {
                    _logger.LogError("You are trying to create an apartment unit that already exists");
                    ModelState.AddModelError("Error Messages", "Apartment Unit Already Exists");
                    return BadRequest(ModelState);
                }

                ApartmentUnit apartmentUnit = _mapper.Map<ApartmentUnit>(apartmentUnitCreateDTO);
                await _dbApartmentUnit.CreateAsync(apartmentUnit);

                _response.Result = _mapper.Map<ApartmentUnitDTO>(apartmentUnit);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetApartmentUnit", new { id = apartmentUnit.ApartmentUnitId }, _response);
            }
            catch (Exception ex)
            {

                _logger.LogError("An Error Occured. See Response Errors for Details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id}", Name = "DeleteApartmentUnit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteApartmentUnit(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogError("Provide an Id to Delete the apartment Unit");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                // try to obtain the requested ApartmentUnit
                var apartmentUnit = await _dbApartmentUnit.GetAsync(x => x.ApartmentUnitId == id);

                if (apartmentUnit == null)
                {
                    _logger.LogError("The apartment Unit you are trying to delete does not exist");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // delete it
                await _dbApartmentUnit.RemoveAsync(apartmentUnit);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return NoContent();


            }
            catch (Exception ex)
            {
                _logger.LogError("An Error Occured. See Response Errors for Details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id}", Name = "UpdateApartmentUnit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateApartmentUnit(string id,
            [FromBody] ApartmentUnitUpdateDTO apartmentUnitUpdateDTO)
        {
            try
            {
                // Input validation
                if (apartmentUnitUpdateDTO == null)
                {
                    _logger.LogError("You must provide information to update the apartment Unit");
                    return BadRequest(apartmentUnitUpdateDTO);
                }

                // if the id in the URL we receive doesnt match with the id of the DTO
                if ( id != apartmentUnitUpdateDTO.ApartmentUnitId)
                {
                    _logger.LogError("The apartment Unit Id and the Apartment Unit Id in the update information do not match");
                    return BadRequest(apartmentUnitUpdateDTO);
                }

                // check if the ApartmentComplexId we receive on the apartmentUnitUpdateDTO exists in the ApartmentComplex
                var apartmentComplexExists = await _dbApartmentComplex.GetAsync(
                    x => x.Id == apartmentUnitUpdateDTO.ApartmentComplexId);
                // if it doesnt exist
                if (apartmentComplexExists == null)
                {
                    _logger.LogError("The Apartment Complex you specified does not exist");
                    ModelState.AddModelError("Error Messages", "Apartment Complex Id is Invalid");
                    return BadRequest(ModelState);
                }
                
                // Fetch the existing apartment unit from the database
                var existingApartmentUnit = await _dbApartmentUnit.GetAsync(x => x.ApartmentUnitId == id);

                // If the apartment unit with the specified id doesn't exist
                if (existingApartmentUnit == null)
                {
                    _logger.LogError("The apartment unit you are trying to update does not exist.");
                    return NotFound("Apartment unit not found.");
                }
                // Map the update DTO properties to the existing apartment unit model
                _mapper.Map(apartmentUnitUpdateDTO, existingApartmentUnit);

                // Update the apartment unit in the database
                await _dbApartmentUnit.UpdateAsync(existingApartmentUnit);

                _logger.LogInformation("Apartment Unit Updated.");
                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError("An Error Occured. See Response Errors for Details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

    }
}
