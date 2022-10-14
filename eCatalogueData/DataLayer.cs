using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Exceptions;
using Data.Models.Interfaces;

namespace Data
{
    public class DataLayer
    {
        private readonly ECatalogueContextDB context;

        public DataLayer(ECatalogueContextDB context)
        {
            this.context = context;
        } 


        #region Student

        public Student CreateStudent(Student newStudent)
        {
            context.Students.Add(newStudent);
            context.SaveChanges();
            return newStudent;
        }

        public void RemoveStudent(int studentId)
        {
            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student existingStudent = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
            RemoveAddress(existingStudent);

            context.Students.Remove(existingStudent);
            context.SaveChanges();
        }

        public Student UpdateStudentData(int studentId, Student student)
        {
            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student studentToModify = context.Students.First(s => s.StudentId == studentId);

            studentToModify.FirstName = student.FirstName;
            studentToModify.LastName = student.LastName;
            studentToModify.Age = student.Age;

            context.SaveChanges();
            return studentToModify;
        }

        public Student UpdateStudentAddress(int studentId, Address newAddress)
        {
            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student existingStudent = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
            RemoveAddress(existingStudent);
            
            existingStudent.Address = newAddress;
            context.SaveChanges();
            return existingStudent;
        }

        #endregion

        #region Catalogue

        public Mark AddMark(Mark newMark)
        {
            if (!context.Students.Any(s => s.StudentId == newMark.StudentId))
            {
                throw new StudentDoesNotExistsException(newMark.StudentId);
            }

            if (!context.Subjects.Any(s => s.SubjectId == newMark.SubjectId))
            {
                throw new SubjectDoesNotExistException(newMark.SubjectId);
            }

            context.Marks.Add(newMark);
            context.SaveChanges();
            return newMark;
        }

        public List<Mark> GetAllMarksForStudent(int studentId, int subjectId)
        {
            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            if (subjectId == 0)
            {
                return context.Students.Include(s => s.Marks).First(s => s.StudentId == studentId).Marks;
            }
            return context.Marks.Where(m => m.StudentId == studentId).Where(m => m.SubjectId == subjectId).ToList();
        }

        public void RemoveSubject(int subjectId)
        {
            if (!context.Subjects.Any(s => s.SubjectId == subjectId))
            {
                throw new SubjectDoesNotExistException(subjectId);
            }

            RemoveSubject(context.Subjects.First(s => s.SubjectId == subjectId));
            context.SaveChanges();
        }

        #endregion

        #region Teacher

        public Teacher CreateTeacher(Teacher newTeacher)
        {
            context.Teachers.Add(newTeacher);
            context.SaveChanges();
            return newTeacher;
        }

        public void RemoveTeacher(int teacherId)
        {
            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            Teacher existingTeacher = context.Teachers.Include(t => t.Address).Include(t => t.Subject).First(t => t.TeacherId == teacherId);
            RemoveAddress(existingTeacher);
            RemoveSubject(existingTeacher.Subject);
            
            context.Teachers.Remove(existingTeacher);
            context.SaveChanges();
        }

        public Teacher UpdateTeacherAddress(int teacherId, Address newAddress)
        {
            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            Teacher existingTeacher = context.Teachers.Include(t => t.Address).Include(t => t.Subject).First(t => t.TeacherId == teacherId);
            RemoveAddress(existingTeacher);

            existingTeacher.Address = newAddress;
            context.SaveChanges();
            return existingTeacher;
        }

        public Subject AssignTeacherSubject(int teacherId, Subject newSubject)
        {
            if (!context.Teachers.Any(t => t.TeacherId == teacherId))
            {
                throw new TeacherDoesNotExistException(teacherId);
            }

            Teacher existingTeacher = context.Teachers.Include(t => t.Subject).First(t => t.TeacherId == teacherId);
            RemoveSubject(existingTeacher.Subject);

            existingTeacher.Subject = newSubject;
            context.SaveChanges();
            return newSubject;
        }

        public Rank PromoteTeacher(int teacherId)
        {
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


        private void RemoveAddress(IResident resident)
        {
            if (resident.Address != null)
            {
                context.Addresses.Remove(context.Addresses.First(a => a.AddressId == resident.Address.AddressId));
            }
        }

        private void RemoveSubject(Subject subject)
        {
            if (subject != null)
            {
                context.Marks.RemoveRange(context.Marks.Where(m => m.SubjectId == subject.SubjectId));
                context.Subjects.Remove(subject);
            }
        }
    }
}
