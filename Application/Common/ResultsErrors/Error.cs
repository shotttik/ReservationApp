namespace Application.Common.ResultsErrors
{
    public sealed record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);
        //public static implicit operator Result(Error error) => Result.Failure(error);
        //public Result ToResult() => Result.Failure(this);

        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            Type = errorType;
        }
        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        public static Error NotFound(string code, string description) =>
            new Error(code, description, ErrorType.NotFound);
        public static Error Validation(string code, string description) =>
            new Error(code, description, ErrorType.Validation);
        public static Error Conflict(string code, string description) =>
            new Error(code, description, ErrorType.Conflict);
        public static Error Failure(string code, string description) =>
            new Error(code, description, ErrorType.Failure);
    };

    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3
    }
}
