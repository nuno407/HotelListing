using System;
using System.ComponentModel.DataAnnotations;
namespace HotelListing.Models
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 255, ErrorMessage = "Hotel name is to long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 255, ErrorMessage = "Hotel Address is to long")]
        public string Address { get; set; }
        [Required]
        [Range(1,5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }
        public CreateHotelDTO Country { get; set; }
    }
}
