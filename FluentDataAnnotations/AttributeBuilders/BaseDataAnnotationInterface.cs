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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentDataAnnotations.AttributeBuilders.Validation;

namespace FluentDataAnnotations.AttributeBuilders
{
    public class BaseDataAnnotationInterface : IAttributeBuilder
    {
        protected IList<IAttributeBuilder> AttributeBuilders;
        protected IList<Attribute> _attributes;

        public IEnumerable<Attribute> Attributes
        {
            get
            {
                foreach (var attr in _attributes)
                {
                    yield return attr;
                }

                foreach (var attr in AttributeBuilders.SelectMany(builder => builder.Attributes))
                {
                    yield return attr;
                }
            }
        }

        public BaseDataAnnotationInterface()
        {
            AttributeBuilders = new List<IAttributeBuilder>();
            _attributes = new List<Attribute>();
        }

        protected void AddAttribute(Attribute attr)
        {
            _attributes.Add(attr);
        }

        public ValidationAttributeBuilder AddValidation
        {
            get
            {
                var builder = new ValidationAttributeBuilder();
                AttributeBuilders.Add(builder);

                return builder;
            }
        }

        public DataTypeAttributeBuilder SetDataType
        {
            get
            {
                var builder = new DataTypeAttributeBuilder();

                AttributeBuilders.Add(builder);

                return builder;
            }
        }

        public UiHintAttributeBuilder SetUiHint()
        {
            var builder = new UiHintAttributeBuilder();

            AttributeBuilders.Add(builder);

            return builder;
        }

        /// <summary>
        /// Adds the given attribute to the internal collection for this property.
        /// </summary>
        /// <param name="attribute">A raw attribute value.</param>
        public void AddRawAttribute(Attribute attribute)
        {
            AddAttribute(attribute);
        }

        /// <summary>
        /// Marks the property as Required.
        /// 
        /// For more details see <see cref="RequiredAttribute"/>.
        /// </summary>
        /// <returns>this</returns>
        public BaseDataAnnotationInterface SetRequired()
        {
            AddRawAttribute(new RequiredAttribute());

            return this;
        }

        /// <summary>
        /// Marks the property as Key.
        /// 
        /// For more details see <see cref="KeyAttribute"/>.
        /// </summary>
        /// <returns>this</returns>
        public BaseDataAnnotationInterface SetKey()
        {
            AddRawAttribute(new KeyAttribute());

            return this;
        }

    }
}