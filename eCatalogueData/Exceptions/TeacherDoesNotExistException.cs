
namespace Data.Exceptions
{
    public class TeacherDoesNotExistException : Exception
    {
        public readonly string message = "";


        public TeacherDoesNotExistException(int id)
        {
            this.message = string.Format("Teacher with ID {0} does not exists", id);
        }
    }
}
