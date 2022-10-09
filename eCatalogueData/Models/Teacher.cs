using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public string FullName { get; set; }
        public Address? Address { get; set; }
        public Rank Rank { get; set; }
        public Subject Subject { get; set; }
    }
}