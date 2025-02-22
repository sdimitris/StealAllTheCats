using StealAllTheCats.Domain.Common.Enums;

namespace StealAllTheCats.Domain.Common;

public class Error
{
    public string Message { get; set; } = String.Empty;
    public KnownApplicationErrorEnum ApplicationError { get; set; }
    public Exception? Exception { get; set; }

    public Error(string message, KnownApplicationErrorEnum applicationError = KnownApplicationErrorEnum.GenericError,
        Exception? exception = null)
    {
        ApplicationError = applicationError;
        Message = message;
        Exception = exception;
    }
    
    public static Error New(string message, Exception? ex, KnownApplicationErrorEnum errorEnum = KnownApplicationErrorEnum.GenericError) => new(message, errorEnum, ex);
}