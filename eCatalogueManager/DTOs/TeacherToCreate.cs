using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ECatalogueManager.DTOs
{
    public class TeacherToCreate
    {
        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Rank is required")]
        [Range(1, 4, ErrorMessage = "Rank cannot be less than 1 or greater than 4")]
        public Rank Rank { get; set; }

        [Required(ErrorMessage = "Subject's ID is required")]
        [Range(1, 1000, ErrorMessage = "ID cannot be less than 1 or greater than 1000")]
        public int SubjectId { get; set; }
    }
}
