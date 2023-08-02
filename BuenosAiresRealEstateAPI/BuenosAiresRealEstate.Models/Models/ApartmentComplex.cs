using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.Models
{
    public class ApartmentComplex
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ComplexName { get; set; }
        [Required]
        public string Address { get; set; }
        public string[] Amenities { get; set; }
        public string Owner { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
