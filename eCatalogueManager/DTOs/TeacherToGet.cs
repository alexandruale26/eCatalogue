
using Data.Models;

namespace ECatalogueManager.DTOs
{
    public class TeacherToGet
    {
        public string FullName { get; set; }
        public string Rank { get; set; }
        public string Subject { get; set; }
        public Address Address { get; set; }
    }
}
