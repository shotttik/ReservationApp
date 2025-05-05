using Application.Common.ResultsErrors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Application.Common.Results
{

    public class CustomValidationProblemDetails :ValidationProblemDetails
    {
        public new List<Error> Errors { get; set; } = new List<Error>();

        public CustomValidationProblemDetails(ModelStateDictionary modelState)
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
            Title = "One or more validation errors occurred.";
            Status = 400;

            foreach (var error in modelState)
            {
                var field = error.Key;
                foreach (var message in error.Value.Errors)
                {
                    Errors.Add(Error.Validation(ValidationErrorCodeMapper.GetErrorCode(field, message.ErrorMessage), message.ErrorMessage));
                }
            }
        }
    }
}