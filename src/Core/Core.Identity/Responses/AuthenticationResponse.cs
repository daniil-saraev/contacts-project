using Core.Identity.Models;
using Core.Identity.Exceptions;

namespace Core.Identity.Responses
{
    public class AuthenticationResponse
    {
        public Token AccessToken { get; set; }

        public Token RefreshToken { get; set; }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public ExceptionType ExceptionType { get; set; }

        public bool IsSuccessful { get; set; }
    }

    public enum ExceptionType
    {
        DuplicateEmailsException,
        InvalidRefreshTokenException,
        RegisterErrorException,
        UserLockedOutException,
        UserNotFoundException,
        WrongPasswordException
    }
}
