using Data.Models;
using ECatalogueManager.DTOs;

namespace ECatalogueManager.Extensions
{
    public static class ExtensionToEntity
    {
        public static Student ToEntity(this StudentToCreate student)
        {
            return new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age
            };
        }

        public static Address ToEntity(this AddressToCreate address)
        {
            return new Address
            {
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber
            };
        }

        public static Subject ToEntity(this SubjectToCreate subject)
        {
            return new Subject
            {
                Name = subject.Name,
                TeacherId = subject.TeacherId
            };
        }

        public static Mark ToEntity(this MarkToCreate mark)
        {
            return new Mark
            {
                Value = mark.Value,
                StudentId = mark.StudentId,
                SubjectId = mark.SubjectId,
                CreateDate = DateTime.Now
            };
        }

        public static Teacher ToEntity(this TeacherToCreate teacher)
        {
            return new Teacher
            {
                FullName = teacher.FullName,
                Rank = teacher.Rank,
            };
        }
    }
}
