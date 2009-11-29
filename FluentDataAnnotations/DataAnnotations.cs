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
using System.Linq.Expressions;
using FluentDataAnnotations.AttributeBuilders;

namespace FluentDataAnnotations
{
    /// <summary>
    /// Provides a base class for all DataAnnotation classes
    /// </summary>
    public abstract class DataAnnotations
    {
        protected IDictionary<string, IList<IAttributeBuilder>> _annotations;

        public virtual IDictionary<string, IList<IAttributeBuilder>> Annotations
        {
            get
            {
                return _annotations;
            }

            set
            {
                _annotations = value;
            }
        }

        internal DataAnnotations()
        {
            Annotations = new Dictionary<string, IList<IAttributeBuilder>>();
        }

        public IEnumerable<Attribute> GetAttributes(string member)
        {
            return _annotations[member].SelectMany(builder => builder.Attributes);
        }
    }

    /// <summary>
    /// Provides a generic interface for adding strongly typed Attributes from the <see cref="System.ComponentModel.DataAnnotations"/> namespace.
    /// </summary>
    /// <typeparam name="TModel">The model type to add attributes to.</typeparam>
    public class DataAnnotations<TModel> : DataAnnotations
    {
        /// <summary>
        /// Annotates the specified property with an attribute.
        /// </summary>
        /// <typeparam name="TProperty">The property to attribute.</typeparam>
        /// <param name="member">The access expression for the property.</param>
        public BaseDataAnnotationInterface AnnotateProperty<TProperty>(Expression<Func<TModel, TProperty>> member)
        {
            string memberName = ExtractMemberNameFromExpression(member);

            if (!Annotations.ContainsKey(memberName))
            {
                Annotations[memberName] = new List<IAttributeBuilder>();
            }

            var bdai = new BaseDataAnnotationInterface();
            Annotations[memberName].Add(bdai);

            return bdai;
        }

        private static string ExtractMemberNameFromExpression<TProperty>(Expression<Func<TModel, TProperty>> member)
        {
            return ((MemberExpression)member.Body).Member.Name;
        }
    }
}