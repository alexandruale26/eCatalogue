using System.ComponentModel.DataAnnotations;

namespace ECatalogueManager.DTOs
{
    public class SubjectToCreate
    {
        [Required(ErrorMessage = "Subject's name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Teacher's ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ID cannot be less than 1 or greater than 1000")]
        public int TeacherId { get; set; }
    }
}
