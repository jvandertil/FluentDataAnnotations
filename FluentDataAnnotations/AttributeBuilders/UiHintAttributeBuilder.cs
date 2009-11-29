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

namespace FluentDataAnnotations.AttributeBuilders
{
    public class UiHintAttributeBuilder : IAttributeBuilder
    {
        private bool? _autoGenerateField;
        private bool? _autoGenerateFilter;
        private string _displayName;
        private string _displayDescription;

        public IEnumerable<Attribute> Attributes
        {
            get { yield return InnerAttribute; }
        }

        protected Attribute InnerAttribute
        {
            get
            {
                var da = new DisplayAttribute();

                if (_autoGenerateField.HasValue)
                    da.AutoGenerateField = _autoGenerateField.Value;

                if (_autoGenerateFilter.HasValue)
                    da.AutoGenerateFilter = _autoGenerateFilter.Value;

                if (!string.IsNullOrEmpty(_displayName))
                    da.Name = _displayName;

                if (!string.IsNullOrEmpty(_displayDescription))
                    da.Description = _displayDescription;

                return da;
            }
        }

        public UiHintAttributeBuilder AutoGenerateField(bool autoField)
        {
            _autoGenerateField = autoField;

            return this;
        }

        public UiHintAttributeBuilder AutoGenerateFilter(bool autoFilter)
        {
            _autoGenerateFilter = autoFilter;

            return this;
        }

        public UiHintAttributeBuilder DisplayedName(string name)
        {
            _displayName = name;

            return this;
        }

        public UiHintAttributeBuilder DisplayedDescription(string description)
        {
            _displayDescription = description;

            return this;
        }
    }
}