using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Address? Address { get; set; }
        public List<Mark> Marks { get; set; } = new List<Mark>();
    }
}
