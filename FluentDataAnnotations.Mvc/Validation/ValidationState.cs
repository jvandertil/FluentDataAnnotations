using System.Collections.Generic;
using FluentDataAnnotations.Validation;

namespace FluentDataAnnotations.Mvc.Validation
{
    public class ValidationState : IValidationState
    {
        public ICollection<IValidationError> Errors { get; private set; }

        public void Add(string property, string message)
        {
            Errors.Add(new ValidationError(property, message));
        }

        public bool IsValid { get { return Errors.Count == 0; } }

        public ValidationState()
        {
            Errors = new List<IValidationError>();
        }
    }
}