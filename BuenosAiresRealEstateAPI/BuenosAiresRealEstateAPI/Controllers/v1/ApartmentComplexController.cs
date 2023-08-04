using AutoMapper;
using BuenosAiresRealEstate.API.Models.Models;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using BuenosAiresRealEstate.API.Models.DTOs;
using Microsoft.AspNetCore.Builder;

namespace BuenosAiresRealEstate.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ApartmentComplexAPI")]
    [ApiVersion("1.0")]
    public class ApartmentComplexController : ControllerBase
    {
        private readonly ILogger<ApartmentComplexController> _logger;
        private readonly IApartmentComplexRepository _dbApartmentComplex;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ApartmentComplexController(ILogger<ApartmentComplexController> logger,
            IApartmentComplexRepository dbApartmentComplex, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbApartmentComplex = dbApartmentComplex;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponse>> GetApartmentComplexes(
            [FromQuery(Name = "filterComplexName")] string? complexName,
            [FromQuery] string? searchAmenities, int pageSize = 0, int pageNumber = 1)
        {
            _logger.LogInformation("Getting Apartment Complexes");

            try
            {
                IEnumerable<ApartmentComplex> apartmentComplexList;

                // if we receive complexName parameter
                if (!string.IsNullOrEmpty(complexName))
                {
                    _logger.LogInformation("Filtering by Complex Name");
                    apartmentComplexList = await _dbApartmentComplex.GetAllAsync(
                        x => x.ComplexName == complexName, pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    apartmentComplexList = await _dbApartmentComplex.GetAllAsync(
                        pageSize: pageSize, pageNumber: pageNumber);
                }

                if (!string.IsNullOrEmpty(searchAmenities))
                {
                    _logger.LogInformation("Filtering by Amenties");

                    string[] searchAmenitiesArray = searchAmenities.Split(',').Select(a => a.Trim()).ToArray();

                    // LINQ query to filter the list based on common amenities
                    apartmentComplexList = apartmentComplexList
                        .Where(complex => complex.Amenities
                            .Split(',')
                            .Select(a => a.Trim())
                            .Intersect(searchAmenitiesArray)
                            .Any());

                }

                // add pagination to response header
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.Result = _mapper.Map<List<ApartmentComplexDTO>>(apartmentComplexList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
                _logger.LogInformation("Unable to Get Apartment Complexes. See response errors");

            }

            return _response;
        }


        [HttpGet("{id:int}", Name = "GetApartmentComplex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ResponseCache(Duration = 30)] // 30 seconds cache. one 30 second cache per id passed in a request
        public async Task<ActionResult<APIResponse>> GetApartmentComplex(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Could Not Get Villa");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var apartmentComplex = await _dbApartmentComplex.GetAsync(u => u.Id == id);

                if (apartmentComplex == null)
                {
                    _logger.LogError("Could Not Get Villa, Villa was Not Found");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _logger.LogInformation("Returning Villa with Id" + id);
                _response.Result = _mapper.Map<ApartmentComplexDTO>(apartmentComplex);
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

        /*** FOR CREATE APARTMENT COMPLEX, WE MUST REMOVE THE ID FIELD FOR IT WILL BE ASSIGNED AUTOMATICALLY ***/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "admin")] // only authorized users with the Role of admin can access this endpoint.
        public async Task<ActionResult<APIResponse>> CreateApartmentComplex(
            [FromBody] ApartmentComplexCreateDTO apartmentComplexCreateDTO)
        {
            try
            {
                ApartmentComplex apartmentComplexIsUnique;

                // here we try to find if the complex name the user entered already exists in db
                apartmentComplexIsUnique = await _dbApartmentComplex.GetAsync(
                    u => u.ComplexName.ToLower() == apartmentComplexCreateDTO.ComplexName.ToLower());

                // if it is not unique, the apartment complex exists already
                if (apartmentComplexIsUnique != null)
                {
                    _logger.LogError("Trying to Create an Apartement Complex that already Exists. Error!");
                    ModelState.AddModelError("ErrorMessages", "Apartment Complex Already Exists");
                    return BadRequest(ModelState);
                }

                if (apartmentComplexCreateDTO == null)
                {
                    _logger.LogError("Must Provide Information to Create Apartment Complex");
                    return BadRequest(apartmentComplexCreateDTO);
                }

                ApartmentComplex apartmentComplex = _mapper.Map<ApartmentComplex>(apartmentComplexCreateDTO);

                await _dbApartmentComplex.CreateAsync(apartmentComplex);

                _logger.LogInformation("Created Apartment Complex");
                _response.Result = _mapper.Map<ApartmentComplexDTO>(apartmentComplex);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;

                return CreatedAtRoute("GetApartmentComplex", new { id = apartmentComplex.Id }, _response); // this is code 201

            }
            catch (Exception ex)
            {
                _logger.LogError("An Error Occured. See Response Errors for Details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteApartmentComplex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "admin")] // only authorized users with the Role of admin can access this endpoint.
        public async Task<ActionResult<APIResponse>> DeleteApartmentComplex(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Could Not Delete Villa with Id '0'");
                    return BadRequest();
                }

                var apartmentComplex = await _dbApartmentComplex.GetAsync(u => u.Id == id);

                if (apartmentComplex == null)
                {
                    _logger.LogError("Could Not Find a Villa with that Id to Delete");
                    return NotFound();
                }

                await _dbApartmentComplex.RemoveAsync(apartmentComplex);

                _logger.LogInformation("Removed Operation Performed");
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
                //return NoContent(); // typically when we delete we don't return anything

            }
            catch (Exception ex)
            {
                _logger.LogError("An Error Occured. See Response Errors for Details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpPut("{id:int}", Name = "UpdateApartmentComplex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "admin")] // only authorized users with the Role of admin can access this endpoint.
        public async Task<ActionResult<APIResponse>> UpdateApartmentComplex(int id, 
            [FromBody] ApartmentComplexUpdateDTO apartmentComplexUpdateDTO)
        {
            try
            {
                // if we receive no object as parameter
                // or if the id we receive does not match the id of the object we receive
                // - so we should populate id and the object with the same id on the Request
                if (apartmentComplexUpdateDTO == null || id != apartmentComplexUpdateDTO.Id)
                {
                    _logger.LogError("Error. No information received or Id Mismatch");
                    return BadRequest();
                }

                ApartmentComplex apartmentComplex = _mapper.Map<ApartmentComplex>(apartmentComplexUpdateDTO);

                await _dbApartmentComplex.UpdateAsync(apartmentComplex);

                _logger.LogInformation("Apartment Complex Information Updated");
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An Error Occurred. See the response Errors for details");
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;

        }
    }
}
