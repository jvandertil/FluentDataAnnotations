using System.Web.Mvc;
using FluentDataAnnotations.Validation;

namespace FluentDataAnnotations.Mvc.Validation
{
    public static class ValidationStateExtensions
    {
        public static void AddToModelState(this IValidationState state, ModelStateDictionary modelState)
        {
            foreach(var error in state.Errors)
                modelState.AddModelError(error.Property, error.Message);
        }
    }
}