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
using System.Linq;

namespace FluentDataAnnotations.AttributeBuilders.Validation
{
    public class ValidationAttributeBuilder : IAttributeBuilder
    {
        public IEnumerable<Attribute> Attributes
        {
            get
            {
                return _builders.SelectMany(builder => builder.Attributes);
            }
        }

        private IList<IAttributeBuilder> _builders;

        public ValidationAttributeBuilder()
        {
            _builders = new List<IAttributeBuilder>();
        }

        public StringLengthAttributeBuilder StringLength
        {
            get
            {
                var builder = new StringLengthAttributeBuilder();
                _builders.Add(builder);

                return builder;
            }
        }
        public RangeAttributeBuilder Range
        {
            get
            {
                var builder = new RangeAttributeBuilder();
                _builders.Add(builder);

                return builder;
            }
        }

        public RegExAttributeBuilder ByRegularExpression(string pattern)
        {
            var builder = new RegExAttributeBuilder(pattern);

            _builders.Add(builder);

            return builder;
        }
    }
}