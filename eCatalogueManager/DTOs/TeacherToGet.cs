
namespace ECatalogueManager.DTOs
{
    public class TeacherToGet
    {
        public string FullName { get; set; }
        public string Rank { get; set; }
        public SubjectToGet Subject { get; set; }
        public AddressToGet Address { get; set; }
    }
}
