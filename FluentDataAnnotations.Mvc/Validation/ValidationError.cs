using FluentDataAnnotations.Validation;

namespace FluentDataAnnotations.Mvc.Validation
{
    public class ValidationError : IValidationError
    {
        public string Property { get; private set; }

        public string Message { get; private set; }

        public ValidationError(string property, string message)
        {
            Property = property;
            Message = message;
        }
    }
}