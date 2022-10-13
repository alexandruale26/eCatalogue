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
    }
}
