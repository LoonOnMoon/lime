using System.Net;

namespace Lime.Application.Common.Errors;

public interface IServiceException
{
    public HttpStatusCode StatusCode { get; }

    public string Title { get; }
}