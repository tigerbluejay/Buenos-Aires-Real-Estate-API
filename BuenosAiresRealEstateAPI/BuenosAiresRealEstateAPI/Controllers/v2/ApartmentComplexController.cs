using BuenosAiresRealEstate.API.Models.DTOs;
using BuenosAiresRealEstate.API.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BuenosAiresRealEstate.API.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ApartmentComplexAPI")]
    [ApiVersion("2.0")]
    public class ApartmentComplexController : ControllerBase
    {
        [HttpGet("GetApartmentComplexes")]
        public IEnumerable<ApartmentComplexDTO> GetApartmentComplexes()
        {
            var list = InMemoryDataStorageApartmentComplexes.inMemoryapartmentComplexList;

            return list;
        }

        [HttpGet("GetApartmentComplex")]
        public ApartmentComplexDTO GetApartmentComplex(int id)
        {
            var list = InMemoryDataStorageApartmentComplexes.inMemoryapartmentComplexList;

            ApartmentComplexDTO apartmentComplex = list.FirstOrDefault(x => x.Id == id);

            return apartmentComplex;
        }
    }
}
