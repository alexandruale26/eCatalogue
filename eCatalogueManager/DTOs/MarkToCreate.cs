using System.ComponentModel.DataAnnotations;

namespace ECatalogueManager.DTOs
{
    public class MarkToCreate
    {
        [Required(ErrorMessage = "Value is required")]
        [Range(1, 10, ErrorMessage = "Value cannot be less than 1 or greater than 10")]
        public int Value { get; set; }

        [Required(ErrorMessage = "Student's ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ID cannot be less than 1")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Subject's ID is required")]
        [Range(1, 1000, ErrorMessage = "ID cannot be less than 1 or greater than 1000")]
        public int SubjectId { get; set; }
    }
}
