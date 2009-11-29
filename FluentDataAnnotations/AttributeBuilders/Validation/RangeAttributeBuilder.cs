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
    public class RangeAttributeBuilder : IAttributeBuilder
    {
        public IEnumerable<Attribute> Attributes
        {
            get
            {
                RangeAttribute resultAttr = null;
                
                if (useCustomType())
                    resultAttr = new RangeAttribute(_forType, _minForType, _maxForType);

                if (useDoubles())
                    resultAttr = new RangeAttribute(_minDouble, _maxDouble);

                if (useIntegers())
                    resultAttr = new RangeAttribute(_minInt, _maxInt);

                if(resultAttr == null)
                    throw new InvalidOperationException("No type set properly. Use one type only (Custom Type / Doubles / Ints)");

                if (!string.IsNullOrEmpty(_errorMessage))
                    resultAttr.ErrorMessage = _errorMessage;

                yield return resultAttr;
            }

        }

                private Type _forType;

        private double _maxDouble = double.MinValue;
        private string _maxForType;

        private int _maxInt = int.MinValue;
        private double _minDouble = double.MaxValue;
        private string _minForType;
        private int _minInt = int.MaxValue;

        private string _errorMessage;

        public RangeAttributeBuilder ForType(Type type)
        {
            _forType = type;

            return this;
        }

        public RangeAttributeBuilder Minimum(int length)
        {
            _minInt = length;

            return this;
        }

        public RangeAttributeBuilder Minimum(double length)
        {
            _minDouble = length;

            return this;
        }

        public RangeAttributeBuilder Minimum(string length)
        {
            _minForType = length;

            return this;
        }

        public RangeAttributeBuilder Maximum(int length)
        {
            _maxInt = length;

            return this;
        }

        public RangeAttributeBuilder Maximum(double length)
        {
            _maxDouble = length;

            return this;
        }

        public RangeAttributeBuilder Maximum(string length)
        {
            _maxForType = length;

            return this;
        }

        public RangeAttributeBuilder WithErrorMessage(string message)
        {
            this._errorMessage = message;

            return this;
        }

        #region Helper methods
        private bool useCustomType()
        {
            return _forType != null && (!string.IsNullOrEmpty(_minForType) && !string.IsNullOrEmpty(_maxForType));
        }

        private bool useDoubles()
        {
            return _minDouble != double.MaxValue && _maxDouble != double.MinValue;
        }

        private bool useIntegers()
        {
            return _minInt != int.MaxValue && _maxInt != int.MinValue;
        }
        #endregion
    }
}