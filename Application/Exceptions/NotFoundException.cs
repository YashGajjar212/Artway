namespace Artway.Application.Exceptions
{
    public class NotFoundException : CustomException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;

        public override string ErrorCode
        {
            get
            {
                return "Resource_Not_Found";
            }
        }

        public NotFoundException(String message) : base(message) { }
    }
}