using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Exceptions;
using System.Linq;

namespace Data
{
    public class DataLayer
    {
        private readonly string connectionString;

        public DataLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }


        #region Student

        public List<Student> GetAllStudents()
        {
            using var context = new ECatalogueContextDB(connectionString);
            return context.Students.Include(s => s.Address).ToList();
        }

        public Student GetStudent(int studentId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }
            return context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
        }

        public Student CreateStudent(Student student)
        {
            using var context = new ECatalogueContextDB(connectionString);
            Student newStudent;

            newStudent = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
            };

            context.Students.Add(newStudent);
            context.SaveChanges();
            return student;
        }

        public void RemoveStudent(int studentId, bool removeAddressIfEmpty)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student existingStudent = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
            int existingAddressId = 0;

            if (existingStudent.Address != null)
            {
                existingAddressId = existingStudent.Address.AddressId;
            }

            context.Students.Remove(existingStudent);
            context.SaveChanges();

            if (removeAddressIfEmpty && existingAddressId > 0)
            {
                RemoveAddressIfIsEmpty(existingAddressId);
            }
        }

        public Student ModifyStudentData(int studentId, Student student)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student studentToModify = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);

            studentToModify.FirstName = student.FirstName;
            studentToModify.LastName = student.LastName;
            studentToModify.Age = student.Age;

            context.SaveChanges();
            return studentToModify;
        }

        public Student ModifyStudentAddress(int studentId, bool removeAddressIfEmpty, Address address)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student existingStudent = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);

            int oldAddressId = 0;
            int existingAddressId = GetAddressID(address);

            if (existingStudent.Address != null)
            {
                oldAddressId = existingStudent.Address.AddressId;
            }

            if (existingAddressId != 0)
            {
                existingStudent.Address = context.Addresses.First(a => a.AddressId == existingAddressId);
            }
            else
            {
                existingStudent.Address = CreateAddress(address);
            }
            context.SaveChanges();

            if (removeAddressIfEmpty && oldAddressId > 0) 
            {
                RemoveAddressIfIsEmpty(oldAddressId);
            }
            return existingStudent;
        }

        #endregion

        #region Catalogue

        public Subject AddSubject(Subject newSubject)
        {
            using var context = new ECatalogueContextDB(connectionString);

            int oldAddressId = 0;

            if (context.Subjects.Any(s => s.Name == newSubject.Name))
            {
                Subject existingSubject = context.Subjects.First(s => s.Name == newSubject.Name);

                if (context.Teachers.Any(t => t.TeacherId == newSubject.TeacherId) && (existingSubject.TeacherId != newSubject.TeacherId))
                {
                    context.Subjects.First(s => s.TeacherId == newSubject.TeacherId).TeacherId = null;

                    Teacher existingTeacherToRemove = context.Teachers.Include(t => t.Address).FirstOrDefault(t => t.TeacherId == existingSubject.TeacherId);

                    if (existingTeacherToRemove != null)
                    {
                        if (existingTeacherToRemove.Address != null)
                        {
                            oldAddressId = existingTeacherToRemove.Address.AddressId;
                        }
                        context.Teachers.Remove(existingTeacherToRemove);
                    }
                    existingSubject.TeacherId = newSubject.TeacherId;
                }
                context.SaveChanges();

                if (oldAddressId > 0)
                {
                    RemoveAddressIfIsEmpty(oldAddressId);
                }
                return existingSubject;
            }

            Subject subject = new Subject();
            subject.Name = newSubject.Name;

            if (context.Teachers.Any(t => t.TeacherId == newSubject.TeacherId))
            {
                context.Subjects.First(s => s.TeacherId == newSubject.TeacherId).TeacherId = null;
                subject.TeacherId = newSubject.TeacherId;
            }

            context.Subjects.Add(subject);
            context.SaveChanges();
            return subject;
        }

        public Mark AddMark(Mark newMark)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == newMark.StudentId))
            {
                throw new StudentDoesNotExistsException(newMark.StudentId);
            }

            if (!context.Subjects.Any(s => s.SubjectId == newMark.SubjectId))
            {
                throw new SubjectDoesNotExistException(newMark.SubjectId);
            }
            else
            {
                Subject existingSubject = context.Subjects.First(s => s.SubjectId == newMark.SubjectId);
                Teacher existingTeacher = context.Teachers.FirstOrDefault(t => t.TeacherId == existingSubject.TeacherId);
                
                if (existingTeacher == null)
                {
                    throw new TeacherDoesNotExistException(existingSubject.SubjectId, 0);
                }
                newMark.TeacherId = existingTeacher.TeacherId;
            }

            context.Marks.Add(newMark);
            context.SaveChanges();
            return newMark;
        }

        public List<Mark> GetAllMarks(int studentId, int subjectId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            if (subjectId == 0)
            {
                return context.Students.Include(s => s.Marks).First(s => s.StudentId == studentId).Marks;
            }

            if (!context.Marks.Where(m => m.StudentId == studentId).Any(m => m.SubjectId == subjectId))
            {
                throw new SubjectDoesNotExistException(subjectId);
            }
            return context.Marks.Where(m => m.StudentId == studentId).Where(m => m.SubjectId == subjectId).ToList(); // could be faster this way

        }

        public Student GetAveragesPerSubject(int studentId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            // averages calculation will be made aftrwards on local machine
            // don't want to have DTO's in DataLayer
            return context.Students.Include(s => s.Marks).First(s => s.StudentId == studentId); 
        }

        public List<Student> GetStudentsOrderedByAverages(bool orderByAscending)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (orderByAscending)
            {
                return  context.Students.Include(s => s.Marks).OrderBy(s => s.Marks.Average(m => m.Value)).ToList();
            }
            return context.Students.Include(s => s.Marks).OrderByDescending(s => s.Marks.Average(m => m.Value)).ToList();

        }

        public Subject GetSubject(int subjectId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Subjects.Any(s => s.SubjectId == subjectId))
            {
                throw new SubjectDoesNotExistException(subjectId);
            }
            return context.Subjects.First(s => s.SubjectId == subjectId);
        }

        public Subject RemoveSubject(int subjectId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Subjects.Any(s => s.SubjectId == subjectId))
            {
                throw new SubjectDoesNotExistException(subjectId);
            }

            Subject existingSubject = context.Subjects.First(s => s.SubjectId == subjectId);
            Teacher existingTeacher = context.Teachers.Include(t => t.Address).FirstOrDefault(t => t.TeacherId == existingSubject.TeacherId);
            int existingAddressId = 0;

            context.Marks.RemoveRange(context.Marks.Where(m => m.SubjectId == subjectId));

            if (existingTeacher != null)
            {
                if (existingTeacher.Address != null)
                {
                    existingAddressId = existingTeacher.Address.AddressId;
                }
                context.Teachers.Remove(existingTeacher);
            }

            context.Subjects.Remove(existingSubject);
            context.SaveChanges();

            if (existingAddressId > 0)
            {
                RemoveAddressIfIsEmpty(existingAddressId);
            }
            return existingSubject;
        }

        public List<Mark> GetMarksByTeacher(int teacherId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            return context.Marks.Where(m => m.TeacherId == teacherId).ToList();
        }

        #endregion

        #region Teacher

        public Teacher GetTeacher(int teacherId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            return context.Teachers.Include(t => t.Address).Include(t => t.Subject).First(t => t.TeacherId == teacherId);
        }

        public List<Teacher> GetAllTeachers()
        {
            using var context = new ECatalogueContextDB(connectionString);

            return context.Teachers.Include(t => t.Address).Include(t => t.Subject).ToList();
        }

        public Teacher CreateTeacher(Teacher newTeacher)
        {
            using var context = new ECatalogueContextDB(connectionString);
            int existingSubjectId = newTeacher.Subject.SubjectId;

            if (!context.Subjects.Any(s => s.SubjectId == existingSubjectId))
            {
                throw new SubjectDoesNotExistException(existingSubjectId);
            }

            Subject existingSubject = context.Subjects.First(s => s.SubjectId == existingSubjectId);
            Teacher existingTeacherToRemove = context.Teachers.FirstOrDefault(t => t.TeacherId == existingSubject.TeacherId);
            
            if (existingTeacherToRemove != null)
            {
                context.Teachers.Remove(existingTeacherToRemove);
            }

            newTeacher.Subject = existingSubject;
            context.Teachers.Add(newTeacher);
            context.SaveChanges();
            return newTeacher;
        }

        public void RemoveTeacher(int teacherId, bool removeAddressIfEmpty)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            Teacher existingTeacher = context.Teachers.Include(t => t.Address).First(t => t.TeacherId == teacherId);
            int oldAddressId = 0;

            if (existingTeacher.Address != null)
            {
                oldAddressId = existingTeacher.Address.AddressId;
            }

            Subject existingSubject = context.Subjects.First(s => s.TeacherId == teacherId);
            existingSubject.TeacherId = null;

            context.Teachers.Remove(existingTeacher);
            context.SaveChanges();
            
            if (removeAddressIfEmpty && oldAddressId > 0)
            {
                RemoveAddressIfIsEmpty(oldAddressId);
            }
        }

        public Teacher ModifyTeacherAddress(int teacherId, bool removeAddressIfEmpty, Address address)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            Teacher existingTeacher = context.Teachers.Include(t => t.Address).Include(t => t.Subject).First(t => t.TeacherId == teacherId);

            int oldAddressId = 0;
            int existingAddressId = GetAddressID(address);

            if (existingTeacher.Address != null) 
            {
                oldAddressId = existingTeacher.Address.AddressId;
            }
            
            if (existingAddressId != 0)
            {
                existingTeacher.Address = context.Addresses.First(a => a.AddressId == existingAddressId);
            }
            else
            {
                existingTeacher.Address = CreateAddress(address);
            }
            context.SaveChanges();

            if (removeAddressIfEmpty && oldAddressId > 0) 
            {
                RemoveAddressIfIsEmpty(oldAddressId);
            }
            return existingTeacher;
        }

        public Subject ModifyTeacherSubject(int teacherId, string newSubjectName)
        {
            using var context = new ECatalogueContextDB(connectionString);
            
            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            Subject newSubject = new Subject { Name = newSubjectName, TeacherId = teacherId};
            return AddSubject(newSubject);
        }

        public Rank PromoteTeacher(int teacherId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            Teacher existingTeacher = context.Teachers.First(t => t.TeacherId == teacherId);

            if ((int)existingTeacher.Rank < Enum.GetNames(typeof(Rank)).Length)
            {
                existingTeacher.Rank++;
            }
            context.SaveChanges();
            return existingTeacher.Rank;
        }

        #endregion


        private Address CreateAddress(Address address)
        {
            return new Address
            {
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber,
            };
        }

        private int GetAddressID(Address address)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Addresses
                .Where(a => a.City.ToLower() == address.City.ToLower())
                .Where(a => a.Street.ToLower() == address.Street.ToLower())
                .Any(a => a.StreetNumber == address.StreetNumber))
            {
                return 0;
            }
            return context.Addresses
                .Where(a => a.City.ToLower() == address.City.ToLower())
                .Where(a => a.Street.ToLower() == address.Street.ToLower())
                .First(a => a.StreetNumber == address.StreetNumber)
                .AddressId;
        }

        private void RemoveAddressIfIsEmpty(int addressId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            Address address = context.Addresses.Include(a => a.Students).Include(a => a.Teachers).First(a => a.AddressId == addressId);

            if (address.Students.Count == 0 && address.Teachers.Count == 0)
            {
                context.Addresses.Remove(address);
            }
            context.SaveChanges();
        }
    }
}
