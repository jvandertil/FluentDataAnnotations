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

namespace FluentDataAnnotations.AttributeBuilders.Validation
{
    public class StringLengthAttributeBuilder : IAttributeBuilder
    {
        public IEnumerable<Attribute> Attributes
        {
            get
            {
                StringLengthAttribute attribute;

                if (_maxLength < _minLength)
                    throw new ArgumentException("_maxLength < _minLength");
                if (_minLength != -1)
                    attribute = new StringLengthAttribute(_maxLength) { MinimumLength = _minLength };
                else
                    attribute = new StringLengthAttribute(_maxLength);

                if (!string.IsNullOrEmpty(_error))
                    attribute.ErrorMessage = _error;

                yield return attribute;
            }
        }

        private string _error;
        private int _maxLength;
        private int _minLength = -1;

        public StringLengthAttributeBuilder Minimum(int length)
        {
            _minLength = length;
            return this;
        }

        public StringLengthAttributeBuilder Maximum(int length)
        {
            _maxLength = length;
            return this;
        }

        public StringLengthAttributeBuilder WithErrorMessage(string errorMessage)
        {
            _error = errorMessage;
            return this;
        }

    }
}