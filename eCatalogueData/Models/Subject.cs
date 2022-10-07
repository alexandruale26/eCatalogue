using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public int? Teacher { get; set; }
    }
}
