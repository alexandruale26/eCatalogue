using System.ComponentModel.DataAnnotations;

namespace ECatalogueManager.DTOs
{
    public class StudentToCreate
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(15, 100, ErrorMessage = "Age cannot be less than 15 or greater than 100")]
        public int Age { get; set; }
    }
}
