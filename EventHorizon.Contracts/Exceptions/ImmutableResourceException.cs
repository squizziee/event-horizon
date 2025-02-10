namespace EventHorizon.Contracts.Exceptions
{
    public class ImmutableResourceException : Exception
    {
        public ImmutableResourceException()
        {
        }

        public ImmutableResourceException(string message)
            : base(message)
        {
        }

        public ImmutableResourceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
