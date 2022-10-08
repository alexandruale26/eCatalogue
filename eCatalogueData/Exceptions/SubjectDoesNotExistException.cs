
namespace Data.Exceptions
{
    public class SubjectDoesNotExistException : Exception
    {
        public readonly string message = "";

        public SubjectDoesNotExistException(int id)
        {
            this.message = string.Format("Subject with ID {0} does not exists", id);
        }
    }
}