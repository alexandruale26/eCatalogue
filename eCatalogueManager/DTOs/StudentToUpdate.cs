using System.ComponentModel.DataAnnotations;

namespace EStudentsManager.DTOs
{
    public class StudentToUpdate
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Range(15, 100)]
        public int? Age { get; set; }
    }
}
