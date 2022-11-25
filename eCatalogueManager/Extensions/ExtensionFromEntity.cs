using Data.Models;
using ECatalogueManager.DTOs;

namespace ECatalogueManager.Extensions
{
    public static class ExtensionFromEntity
    {
        public static StudentToGet ToDto(this Student student)
        {
            return new StudentToGet
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                Address = student.Address.ToDto(),
            };
        }

        public static BasicStudentToGet ToDtoBasic(this Student student)
        {
            return new BasicStudentToGet
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
            };
        }

        public static AddressToGet ToDto(this Address address)
        {
            if(address == null)
            {
                return null;
            }

            return new AddressToGet
            {
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber
            };
        }

        public static SubjectToGet ToDto(this Subject subject)
        {
            return new SubjectToGet
            {
                Name = subject.Name,
            };
        }

        public static MarkToGet ToDto(this Mark mark)
        {
            return new MarkToGet
            {
                Value = mark.Value,
                SubjectId = mark.SubjectId,
                CreationDate = mark.CreationDate.ToString(),
            };
        }

        public static MarkByTeacherToGet ToDtoByTeacher(this Mark mark)
        {
            return new MarkByTeacherToGet
            {
                Value = mark.Value,
                StudentId = mark.StudentId,
                CreationDate = mark.CreationDate.ToString(),
            };
        }

        public static List<AveragesPerSubjectToGet> ToDtoByAverage(this List<Mark> marks)
        {
            return marks
                .GroupBy(m => m.SubjectId)
                .Select(m => new AveragesPerSubjectToGet { SubjectId = m.Key, Value = m .Average(m => m.Value) })
                .ToList();
        }

        public static StudentOrderedToGet ToDtoOrdered(this Student student)
        {
            if (student.Marks.Count == 0)
            {
                return new StudentOrderedToGet
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Age = student.Age,
                    Average = 0.0
                };
            }

            return new StudentOrderedToGet
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                Average = Math.Round(student.Marks.Average(s => s.Value),2)
            };
        }

        public static TeacherToGet ToDto(this Teacher teacher)
        {
            if (teacher.Subject == null)
            {
                return new TeacherToGet
                {
                    FullName = teacher.FullName,
                    Rank = teacher.Rank.RankToName(),
                    Subject = null,
                    Address = teacher.Address.ToDto(),
                };
            }

            return new TeacherToGet
            {
                FullName = teacher.FullName,
                Rank = teacher.Rank.RankToName(),
                Subject = teacher.Subject.ToDto(),
                Address = teacher.Address.ToDto()
            };
        }

        public static BasicTeacherToGet ToDtoBasic(this Teacher teacher)
        {
            return new BasicTeacherToGet
            {
                FullName = teacher.FullName,
                Rank = teacher.Rank.RankToName()
            };
        }

    }
}
