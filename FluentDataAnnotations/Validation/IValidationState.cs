using System.Collections.Generic;

namespace FluentDataAnnotations.Validation
{
    public interface IValidationState
    {
        ICollection<IValidationError> Errors { get; }

        bool IsValid { get; }

        void Add(string property, string message);
    }
}