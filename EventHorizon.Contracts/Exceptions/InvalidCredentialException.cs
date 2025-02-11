namespace EventHorizon.Contracts.Exceptions
{
    public class InvalidCredentialException : Exception
    {
        public InvalidCredentialException()
        {
        }

        public InvalidCredentialException(string message)
            : base(message)
        {
        }

        public InvalidCredentialException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
