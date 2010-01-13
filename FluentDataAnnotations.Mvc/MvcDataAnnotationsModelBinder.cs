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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using FluentDataAnnotations.Mvc.Validation;

namespace FluentDataAnnotations.Mvc
{
    public class MvcDataAnnotationsModelBinder : DefaultModelBinder
    {
        protected static IDictionary<Type, IPropertyBinder> PropertyBinders = new Dictionary<Type, IPropertyBinder>();
        protected static DataAnnotationsTypeDescriptionProvider TypeDescriptionProvider;

        protected override bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            var propertyName = propertyDescriptor.Name;
            var modelType = propertyDescriptor.ComponentType;

            return IsBindable(modelType, propertyName)
                       ? base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value)
                       : false;
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (PropertyBinders.ContainsKey(propertyDescriptor.PropertyType))
            {
                //Replace form value with value we got from the PropertyBinder
                value = PropertyBinders[propertyDescriptor.PropertyType].BindProperty(controllerContext, bindingContext, propertyDescriptor.Name);
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var validator = new EntityValidator();

            var result = validator.Validate(bindingContext.Model);

            result.AddToModelState(bindingContext.ModelState);
        }

        private bool IsBindable(Type modelType, string propertyName)
        {
            if(TypeDescriptionProvider == null)
                TypeDescriptionProvider = new DataAnnotationsTypeDescriptionProvider(TypeDescriptor.GetProvider(modelType));

            var descriptor = TypeDescriptionProvider.GetTypeDescriptor(modelType);

            var properties = descriptor.GetProperties();
            var attributes = properties[propertyName].Attributes.OfType<DenyClientModificationAttribute>();

            //if(attributes.Count > 0) there is a DenyClientModificationAttribute so not bindable.
            return !(attributes.Count() > 0);
        }
    }
}