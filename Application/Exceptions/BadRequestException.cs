namespace Artway.Application.Exceptions
{
    public class BadRequestException : CustomException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;

        public override string ErrorCode
        {
            get
            {
                return "Bad_Request";
            }
        }

        public BadRequestException(string message) : base(message) { }
    }
}