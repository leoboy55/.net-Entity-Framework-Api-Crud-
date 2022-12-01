using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ApiCase.Models
{
    public class Addres
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Street name is required")]
        [StringLength(100, ErrorMessage = "Street lenght can be more then 100 characters.")]
        public string Street { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Housenumber has to be between 1 and 1000.")]
        public int HouseNumber { get; set; }
        
        [Required (ErrorMessage = "Postal number is required")]
        public string PostalNumber { get; set; }
        
        [Required (ErrorMessage = "City is required")]
        public string City { get; set; }
        
        [Required (ErrorMessage = "Country is required")]
        public string Country { get; set; }
        
    }
}
