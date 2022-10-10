
namespace ECatalogueManager.DTOs
{
    public class TeacherToGet
    {
        public string FullName { get; set; }
        public string Rank { get; set; }
        public string Subject { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? StreetNumber { get; set; }
    }
}
