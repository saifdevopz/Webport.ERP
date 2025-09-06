using Webport.ERP.Common.Domain.Results;

namespace Webport.ERP.Common.Domain.Errors;

public sealed record ValidationError : CustomError
{
    public ValidationError(CustomError[] errors) : base(
                    "General.Validation",
                    "One or more validation errors occurred",
                    ErrorType.Validation)
    {
        Errors = errors;
    }

    public CustomError[] Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result> results)
    {
        return new([.. results.Where(r => r.IsFailure).Select(r => r.Error!)]);
    }
}

