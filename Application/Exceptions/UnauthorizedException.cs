namespace Artway.Application.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public override int StatusCode => StatusCodes.Status401Unauthorized;

        public override string ErrorCode => "User_UnAuthorized";

        public UnauthorizedException(string message) : base(message) { }
    }
}