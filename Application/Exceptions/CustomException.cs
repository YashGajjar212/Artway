namespace Artway.Application.Exceptions
{
    public abstract class CustomException : Exception
    {
        public abstract int StatusCode { get; }
        public abstract string ErrorCode { get; }

        protected CustomException(string message) : base(message) { }
    }
}