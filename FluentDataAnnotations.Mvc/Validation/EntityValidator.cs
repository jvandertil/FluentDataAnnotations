/*  Copyright (C) 2009 Jos van der Til
    
    This file is part of the Fluent DataAnnotations Library.

    The Fluent DataAnnotations Library is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    The Fluent Metadata Library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with the Fluent DataAnnotations Library.  If not, see <http://www.gnu.org/licenses/>.
 */
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentDataAnnotations.Validation;

namespace FluentDataAnnotations.Mvc.Validation
{
    public class EntityValidator : IEntityValidator
    {
        #region IEntityValidator Members

        public IValidationState Validate(object entity)
        {
            var result = new ValidationState();
            var validationContext = new ValidationContext(entity, null, null);
            var provider = new DataAnnotationsTypeDescriptionProvider(TypeDescriptor.GetProvider(entity));

            //Get all properties for TEntity
            IEnumerable<PropertyDescriptor> properties =
                from prop in provider.GetTypeDescriptor(entity.GetType(), entity)
                                        .GetProperties().Cast<PropertyDescriptor>()
                select prop;

            foreach (PropertyDescriptor property in properties)
            {
                object propertyValue = property.GetValue(entity);

                //Get ValidationAttributes for property
                IEnumerable<ValidationAttribute> attributes = property.Attributes.OfType<ValidationAttribute>();

                foreach (ValidationAttribute attr in from attr in attributes
                                                     let validationResult =
                                                         attr.GetValidationResult(propertyValue, validationContext)
                                                     where validationResult != ValidationResult.Success
                                                     select attr)
                {
                    result.Add(property.Name, attr.FormatErrorMessage(property.Name));
                }
            }

            return result;
        }

        #endregion
    }
}