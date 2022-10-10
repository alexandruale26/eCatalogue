using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ECatalogueManager.DTOs
{
    public class StudentOrderedToGet
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Average { get; set; }
    }
}
