using BuenosAiresRealEstate.API.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.Models
{
    public static class InMemoryDataStorageApartmentComplexes
    {
        public static List<ApartmentComplexDTO> inMemoryapartmentComplexList
            = new List<ApartmentComplexDTO>()
            {

                new ApartmentComplexDTO()
                {
                    Id = 1,
                    ComplexName = "Alpha Complex",
                    Address = "Alpha 123",
                    Amenities = "Pool, Laundry Room, Meeting Room",
                    ImageUrl = "string",
                    Owner = "J Iriarte"
                },
                new ApartmentComplexDTO()
                {
                    Id = 2,
                    ComplexName = "Beta Complex",
                    Address = "Beta 123",
                    Amenities = "Pool, Laundry Room",
                    ImageUrl = "string",
                    Owner = "J Iriarte"
                },
                new ApartmentComplexDTO()
                {
                    Id = 3,
                    ComplexName = "Epsilon Complex",
                    Address = "Epsilon 123",
                    Amenities = "Pool",
                    ImageUrl = "string",
                    Owner = "J Iriarte"
                },
                new ApartmentComplexDTO()
                {
                    Id = 4,
                    ComplexName = "Gamma Complex",
                    Address = "Gamma 123",
                    Amenities = "Laundry Room, Meeting Room",
                    ImageUrl = "string",
                    Owner = "J Iriarte"
                },
                new ApartmentComplexDTO()
                {
                    Id = 5,
                    ComplexName = "Iota Complex",
                    Address = "Iota 123",
                    Amenities = "Pool, Meeting Room",
                    ImageUrl = "string",
                    Owner = "J Iriarte"
                },

            };
    }
}
