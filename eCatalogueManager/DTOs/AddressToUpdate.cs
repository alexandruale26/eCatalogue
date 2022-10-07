using System.ComponentModel.DataAnnotations;

namespace EStudentsManager.DTOs
{
    public class AddressToUpdate
    {
        [Required(ErrorMessage = "city is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "street number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "street number cannot be less than 1")]
        public int StreetNumber { get; set; }
    }
}
