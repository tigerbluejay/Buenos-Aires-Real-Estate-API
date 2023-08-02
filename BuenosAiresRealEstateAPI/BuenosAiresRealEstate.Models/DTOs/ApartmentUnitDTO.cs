using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuenosAiresRealEstate.API.Models.Models;

namespace BuenosAiresRealEstate.API.Models.DTOs
{
    public class ApartmentUnitDTO
    {
        [Required]
        public string ApartmentUnitId { get; set; }
        public int SquareMeters { get; set; }
        public double Rate { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public int Details { get; set; }

        [Required]
        public int ApartmentComplexId { get; set; }

        // since this DTO is used in GET (read) operations, we include the apartment complex
        // so we can retrieve it. This is not the case of the Create and Update DTOs, where
        // the required ApartmentComplexId property suffices.
        public ApartmentComplex ApartmentComplex { get; set; }
    }
}
