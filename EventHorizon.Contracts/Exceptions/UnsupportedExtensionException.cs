namespace EventHorizon.Contracts.Exceptions
{
    public class UnsupportedExtensionException : Exception
    {
        public UnsupportedExtensionException()
        {
        }

        public UnsupportedExtensionException(string message)
            : base(message)
        {
        }

        public UnsupportedExtensionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
