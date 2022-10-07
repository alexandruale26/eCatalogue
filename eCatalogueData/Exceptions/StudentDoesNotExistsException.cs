
namespace Data.Exceptions
{
    public class StudentDoesNotExistsException : Exception
    {
        public readonly string message = "";

        public StudentDoesNotExistsException(int id)
        {
            this.message = string.Format("Student with ID {0} does not exists", id);
        }
    }
}