namespace Application.Common.ResultsErrors
{
    public class Result
    {
        public Result(bool isSuccess, Error error, SuccessInfo? successInfo = null)
        {
            if (isSuccess && error != Error.None && successInfo == null
                || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid Result", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            SuccessInfo = successInfo;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public SuccessInfo? SuccessInfo { get; }

        public static Result Success() => new(true, Error.None, SuccessInfo.Default);

        public static Result Success(string code, string message = "") =>
            new(true, Error.None, new SuccessInfo(code, message));

        public static Result Success(SuccessInfo successInfo) =>
            new(true, Error.None, successInfo);

        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None, SuccessInfo.Default);

        public static Result<TValue> Success<TValue>(TValue value, string code, string message = "") =>
            new(value, true, Error.None, new SuccessInfo(code, message));

        public static Result<TValue> Success<TValue>(TValue value, SuccessInfo successInfo) =>
            new(value, true, Error.None, successInfo);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }

    public class Result<TValue> :Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error error, SuccessInfo? successInfo = null)
            : base(isSuccess, error, successInfo)
        {
            _value = value;
        }

        public TValue Value =>
            IsSuccess
                ? _value! : throw new InvalidOperationException("No value present");

        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

        public static Result<TValue> ValidationFailure(Error error) =>
            new(default, false, error);
    }

    public sealed record SuccessInfo
    {
        public static readonly SuccessInfo Default = new("Operation.Successful", "Operation completed successfully");

        public SuccessInfo(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }
        public string Message { get; }
    }
}