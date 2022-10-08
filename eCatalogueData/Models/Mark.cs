using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Mark
    {
        [Key]
        public int MarkId { get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
