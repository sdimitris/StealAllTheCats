namespace StealAllTheCats.Domain.Common;

public class Result<T>
{
    
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; set; }
    public Error? Error { get; set; }

    public Result(T? value, bool? isSuccess,Error? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Ok() => new Result(true);
    public static Result Failure(Error error) => new Result(false, error);
}