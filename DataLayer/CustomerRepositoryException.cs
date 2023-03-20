namespace DataLayer
{
    public class CustomerRepositoryException : Exception
    {
        public CustomerRepositoryException(string? message) : base(message)
        {
        }

        public CustomerRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
