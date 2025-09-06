using Webport.ERP.Common.Domain.Errors;

namespace Webport.ERP.Common.Infrastructure.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class CustomException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
{
    public CustomException(string requestName, CustomError? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public CustomError? Error { get; }
}
