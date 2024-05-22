using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        internal static void MedelValiadtion(object obj)
        {
            ValidationContext validationContext = new(obj);

            List<ValidationResult> validationResults = new();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                var firstErrorMessage = validationResults.FirstOrDefault()?.ErrorMessage;
                throw new ArgumentException(firstErrorMessage);
            }
        }
    }
}
