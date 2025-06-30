using System.ComponentModel.DataAnnotations;
namespace Shared
{

    public class DecimalPrecisionAttribute :ValidationAttribute
    {
        public int Precision { get; }
        public int Scale { get; }

        public DecimalPrecisionAttribute(int precision, int scale)
        {
            Precision = precision;
            Scale = scale;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is decimal decimalValue)
            {
                // Convert decimal to string and split on the decimal point
                string decimalString = decimalValue.ToString("G");
                int decimalPointIndex = decimalString.IndexOf(".");

                int integerDigits = decimalPointIndex == -1 ? decimalString.Length : decimalPointIndex;
                int fractionDigits = decimalPointIndex == -1 ? 0 : decimalString.Length - decimalPointIndex - 1;

                // Check if precision and scale are within bounds
                if (integerDigits + fractionDigits > Precision || fractionDigits > Scale)
                {
                    return new ValidationResult($"The value of {validationContext.DisplayName} exceeds the allowed precision and scale.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid decimal value.");
        }
    }

}
