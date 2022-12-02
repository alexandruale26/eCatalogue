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

        public async Task<Student> CreateStudent(Student newStudent)
        {
            context.Students.Add(newStudent);
            await context.SaveChangesAsync();
            return newStudent;
        }

        public async Task RemoveStudent(int studentId)
        {
            if (!await context.Students.AnyAsync(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student existingStudent = await context.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.StudentId == studentId);
            RemoveAddress(existingStudent);

            context.Students.Remove(existingStudent);
            await context.SaveChangesAsync();
        }

        public async Task<Student> UpdateStudentData(int studentId, Student student)
        {
            if (!await context.Students.AnyAsync(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student studentToModify = await context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);

            studentToModify.FirstName = student.FirstName;
            studentToModify.LastName = student.LastName;
            studentToModify.Age = student.Age;

            await context.SaveChangesAsync();
            return studentToModify;
        }

        public async Task<Student> UpdateStudentAddress(int studentId, Address newAddress)
        {
            if (!await context.Students.AnyAsync(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student existingStudent = await context.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.StudentId == studentId);
            RemoveAddress(existingStudent);
            
            existingStudent.Address = newAddress;
            await context.SaveChangesAsync();
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
                return context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == studentId).Marks;
            }
            return context.Marks.Where(m => m.StudentId == studentId).Where(m => m.SubjectId == subjectId).ToList();
        }

        public void RemoveSubject(int subjectId)
        {
            if (!context.Subjects.Any(s => s.SubjectId == subjectId))
            {
                throw new SubjectDoesNotExistException(subjectId);
            }

            RemoveSubject(context.Subjects.FirstOrDefault(s => s.SubjectId == subjectId));
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

            Teacher existingTeacher = context.Teachers.Include(t => t.Address).Include(t => t.Subject).FirstOrDefault(t => t.TeacherId == teacherId);
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

            Teacher existingTeacher = context.Teachers.Include(t => t.Address).Include(t => t.Subject).FirstOrDefault(t => t.TeacherId == teacherId);
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

            Teacher existingTeacher = context.Teachers.Include(t => t.Subject).FirstOrDefault(t => t.TeacherId == teacherId);
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

            Teacher existingTeacher = context.Teachers.FirstOrDefault(t => t.TeacherId == teacherId);

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
                context.Addresses.Remove(context.Addresses.FirstOrDefault(a => a.AddressId == resident.Address.AddressId));
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
