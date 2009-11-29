using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FluentDataAnnotations.Validation;
using System.ComponentModel.DataAnnotations;

namespace FluentDataAnnotations.Mvc.Validation
{
    public class EntityValidator : IEntityValidator
    {
        public IValidationState Validate(object entity)
        {
            var result = new ValidationState();
            var validationContext = new ValidationContext(entity, null, null);
            var provider = new DataAnnotationsTypeDescriptionProvider(TypeDescriptor.GetProvider(entity));

            //Get all properties for TEntity
            var properties = from prop in provider.GetTypeDescriptor(entity.GetType(), entity)
                                 .GetProperties().Cast<PropertyDescriptor>()
                             select prop;

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(entity);

                //Get ValidationAttributes for property
                var attributes = property.Attributes.OfType<ValidationAttribute>();

                foreach (var attr in from attr in attributes
                                     let validationResult = attr.GetValidationResult(propertyValue, validationContext)
                                     where validationResult != ValidationResult.Success
                                     select attr)
                {
                    result.Add(property.Name, attr.FormatErrorMessage(property.Name));
                }
            }

            return result;
        }
    }
}