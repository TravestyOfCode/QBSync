using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace QBSync.API;

public abstract class Error
{
    public int StatusCode { get; set; }
    public required string StatusMessage { get; set; }
    public ModelStateDictionary ModelState { get; } = new ModelStateDictionary();

    public static BadRequestError BadRequest() => new BadRequestError();
    public static BadRequestError BadRequest(string key, string error) => new BadRequestError(key, error);
    public static ForbiddenError Forbidden() => new ForbiddenError();
    public static NotFoundError NotFound() => new NotFoundError();
    public static ServerError ServerError() => new ServerError();
}

public class BadRequestError : Error
{
    [SetsRequiredMembers]
    public BadRequestError() : this("Bad Request") { }

    [SetsRequiredMembers]
    public BadRequestError(string errorMessage)
    {
        StatusCode = 400;
        StatusMessage = errorMessage;
    }

    [SetsRequiredMembers]
    public BadRequestError(string property, string propertyError) : this("Bad Request")
    {
        ModelState.AddModelError(property, propertyError);
    }
}

public class ForbiddenError : Error
{
    [SetsRequiredMembers]
    public ForbiddenError() : this("Forbidden") { }

    [SetsRequiredMembers]
    public ForbiddenError(string errorMessage)
    {
        StatusCode = 403;
        StatusMessage = errorMessage;
    }
}

public class NotFoundError : Error
{
    [SetsRequiredMembers]
    public NotFoundError() : this("Not Found") { }

    [SetsRequiredMembers]
    public NotFoundError(string errorMessage)
    {
        StatusCode = 404;
        StatusMessage = errorMessage;
    }

    [SetsRequiredMembers]
    public NotFoundError(string key, string message) : this("Not Found")
    {
        ModelState.AddModelError(key, message);
    }
}

public class ServerError : Error
{
    [SetsRequiredMembers]
    public ServerError() : this("Server Error") { }

    [SetsRequiredMembers]
    public ServerError(string errorMessage)
    {
        StatusCode = 500;
        StatusMessage = errorMessage;
    }
}
