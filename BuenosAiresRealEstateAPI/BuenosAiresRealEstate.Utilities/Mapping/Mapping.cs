using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BuenosAiresRealEstate.API.Data;
using BuenosAiresRealEstate.API.Models.DTOs;
using BuenosAiresRealEstate.API.Models.Identity_DTOs;
using BuenosAiresRealEstate.API.Models.Models;

namespace BuenosAiresRealEstate.API.Utilities
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApartmentComplex, ApartmentComplexDTO>();
            CreateMap<ApartmentComplexDTO, ApartmentComplex>();
            // You can do custom mappings - read the docs

            CreateMap<ApartmentComplex, ApartmentComplexCreateDTO>().ReverseMap();
            CreateMap<ApartmentComplex, ApartmentComplexUpdateDTO>().ReverseMap();

            CreateMap<ApartmentUnit, ApartmentUnitDTO>().ReverseMap();
            CreateMap<ApartmentUnit, ApartmentUnitCreateDTO>().ReverseMap();
            CreateMap<ApartmentUnit, ApartmentUnitUpdateDTO>().ReverseMap();

            // for .NET identity
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
