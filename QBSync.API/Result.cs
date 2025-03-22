using System.Diagnostics.CodeAnalysis;

namespace QBSync.API;

public class Result
{
    public int StatusCode { get; set; }
    public required string StatusMessage { get; set; }
    public bool WasSuccessful => StatusCode >= 200 && StatusCode <= 299;
    public List<Error> Errors { get; } = new List<Error>();

    public static Result Ok() => new Result() { StatusCode = 200, StatusMessage = "Ok" };
}

public class Result<T> : Result
{
    private T? _Value;
    public T? Value
    {
        get => WasSuccessful ? _Value : throw new InvalidOperationException("Unable to access the Value of a non successful Result.");
        set
        {
            _Value = value;
        }
    }

    [SetsRequiredMembers]
    internal Result(int statusCode, string message, T? value)
    {
        StatusCode = statusCode;
        StatusMessage = message;
        _Value = value;
    }

    [SetsRequiredMembers]
    internal Result(int statusCode, string message, T? value, Error error) : this(statusCode, message, value)
    {
        Errors.Add(error);
    }

    public static Result<T> Ok(T value) => new Result<T>(200, "Ok", value);
    public static implicit operator Result<T>(T value) => new Result<T>(200, "Ok", value);
    public static implicit operator Result<T>(Error error) => new Result<T>(error.StatusCode, error.StatusMessage, default, error);

}
