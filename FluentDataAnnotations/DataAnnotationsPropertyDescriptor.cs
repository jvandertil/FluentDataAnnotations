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
using FluentDataAnnotations.AttributeBuilders;

namespace FluentDataAnnotations
{
    internal class DataAnnotationsPropertyDescriptor : CustomTypeDescriptor
    {
        private readonly IDictionary<string, IList<IAttributeBuilder>> _fluentAnnotations;

        public DataAnnotationsPropertyDescriptor(IDictionary<string, IList<IAttributeBuilder>> fluentAnnotations, ICustomTypeDescriptor parent)
            : base(parent)
        {
            _fluentAnnotations = fluentAnnotations;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection baseProperties = base.GetProperties();
            var list = new List<PropertyDescriptor>();
            bool hasCustomAttributes = false;

            foreach (PropertyDescriptor item in baseProperties)
            {
                if (_fluentAnnotations.ContainsKey(item.Name))
                {
                    IList<IAttributeBuilder> fluentAttributeCollection = _fluentAnnotations[item.Name];
                    if (fluentAttributeCollection.Count > 0)
                    {
                        hasCustomAttributes = true;
                        var attributes = new List<Attribute>();
                        foreach (IAttributeBuilder t in fluentAttributeCollection)
                        {
                            attributes.AddRange(t.Attributes);
                        }

                        list.Add(new PropertyDescriptorWrapper(item, attributes.ToArray()));
                    }
                    else
                    {
                        //No Fluent attributes
                        list.Add(item);
                    }
                }
                else
                {
                    list.Add(item);
                }
            }

            if (hasCustomAttributes)
            {
                return new PropertyDescriptorCollection(list.ToArray(), true);
            }
            else
            {
                return baseProperties;
            }
        }
    }

    internal class PropertyDescriptorWrapper : PropertyDescriptor
    {
        private readonly PropertyDescriptor _descriptor;

        public PropertyDescriptorWrapper(PropertyDescriptor descriptor, Attribute[] attributes)
            : base(descriptor, attributes)
        {
            _descriptor = descriptor;
        }

        public override Type ComponentType
        {
            get { return _descriptor.ComponentType; }
        }

        public override bool IsReadOnly
        {
            get { return _descriptor.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return _descriptor.PropertyType; }
        }

        public override bool CanResetValue(object component)
        {
            return _descriptor.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return _descriptor.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            _descriptor.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            _descriptor.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return _descriptor.ShouldSerializeValue(component);
        }
    }
}
