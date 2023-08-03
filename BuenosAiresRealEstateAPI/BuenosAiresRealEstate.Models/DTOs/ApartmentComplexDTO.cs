using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.DTOs
{
    public class ApartmentComplexDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string ComplexName { get; set; }
        [Required]
        [MaxLength(40)]
        public string Address { get; set; }
        public string Amenities { get; set; }
        public string Owner { get; set; }
        public string ImageUrl { get; set; }
    }
}
