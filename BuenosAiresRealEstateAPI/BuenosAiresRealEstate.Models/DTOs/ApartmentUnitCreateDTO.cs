using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.DTOs
{
    public class ApartmentUnitCreateDTO
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
        public ApartmentComplex ApartmentComplex { get; set; }


    }
}
