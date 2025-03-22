using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QBSync.API;

public abstract class Error
{
    public int StatusCode { get; set; }
    public required string StatusMessage { get; set; }
    public ModelStateDictionary ModelState { get; } = new ModelStateDictionary();
}

public class BadRequestError : Error
{
    public BadRequestError() : this("Bad Request") { }
    public BadRequestError(string errorMessage)
    {
        StatusCode = 400;
        StatusMessage = errorMessage;
    }
    public BadRequestError(string property, string propertyError) : this("Bad Request")
    {
        ModelState.AddModelError(property, propertyError);
    }
}

public class ForbiddenError : Error
{
    public ForbiddenError() : this("Forbidden") { }
    public ForbiddenError(string errorMessage)
    {
        StatusCode = 403;
        StatusMessage = errorMessage;
    }
}

public class NotFoundError : Error
{
    public NotFoundError() : this("Not Found") { }
    public NotFoundError(string errorMessage)
    {
        StatusCode = 404;
        StatusMessage = errorMessage;
    }
    public NotFoundError(string key, string message) : this("Not Found")
    {
        ModelState.AddModelError(key, message);
    }
}

public class ServerError : Error
{
    public ServerError() : this("Server Error") { }
    public ServerError(string errorMessage)
    {
        StatusCode = 500;
        StatusMessage = errorMessage;
    }
}
