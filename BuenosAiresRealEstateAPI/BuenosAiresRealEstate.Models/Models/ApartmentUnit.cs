using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.Models
{
    // each ApartmentComplex has a Number of Units.
    // each ApartmentUnit belongs to single Apartment Complex
    public class ApartmentUnit
    {
        // here we will generate the identifier for the ApartmentUnit
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ApartmentUnitId { get; set; }
        [Required]
        public int SquareMeters { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public int Details { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }


        [ForeignKey("ApartmentComplex")] // this will be the FK referencing the Apartment Complex
        public int ApartmentComplexId { get; set; }
        public ApartmentComplex ApartmentComplex { get; set; } // this is the navigation property

        /* With this setup, the ApartmentComplex property acts as a navigation property, allowing you 
         * to navigate from an ApartmentUnit instance to its related ApartmentComplex instance. 
         * This is a convenient way to work with related data without explicitly handling the foreign key 
         * values and database joins manually. */

        /* Now, if you have an instance of ApartmentUnit, you can access its related ApartmentComplex 
         * using the ApartmentComplex navigation property:

        var apartmentUnit = dbContext.ApartmentUnits.FirstOrDefault(); // Get an ApartmentUnit instance
        var relatedApartmentComplex = apartmentUnit.ApartmentComplex; // Access the related ApartmentComplex
        */
    }
}
