using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Attributes
{
    public class PasswordComplexityAttribute :ValidationAttribute
    {
        private const string DefaultErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one number, and one special character.";
        private const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?\"":{}|<>]).{8,}$";

        public PasswordComplexityAttribute() : base(DefaultErrorMessage)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string password && Regex.IsMatch(password, PasswordPattern))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
        }
    }
}
