using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Mark
    {
        [Key]
        public int MarkId { get; set; }
        public int Value { get; set; }

        [ForeignKey("SubjectId")]
        public int SubjectId { get; set; }

        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
