using System.Net;

using Lime.Application.Common.Errors;

namespace Lime.Application.Authentication.Commands.Errors;

public class DuplicateEmailException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string Title => "User with given email already exists.";
}