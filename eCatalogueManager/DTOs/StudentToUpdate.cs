using System.ComponentModel.DataAnnotations;

namespace EStudentsManager.DTOs
{
    public class StudentToUpdate
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Range(15, 100, ErrorMessage = "Age cannot be less than 15 or greater than 100")]
        public int? Age { get; set; }
    }
}
