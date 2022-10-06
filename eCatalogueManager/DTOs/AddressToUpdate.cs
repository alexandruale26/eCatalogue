using System.ComponentModel.DataAnnotations;

namespace EStudentsManager.DTOs
{
    public class AddressToUpdate
    {
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Street number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Street number cannot be less than 1")]
        public int StreetNumber { get; set; }
    }
}
