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
using FluentDataAnnotations.Validation;

namespace FluentDataAnnotations.Mvc.Validation
{
    public class ValidationState : IValidationState
    {
        public ICollection<IValidationError> Errors { get; private set; }

        public void Add(string property, string message)
        {
            Errors.Add(new ValidationError(property, message));
        }

        public bool IsValid { get { return Errors.Count == 0; } }

        public ValidationState()
        {
            Errors = new List<IValidationError>();
        }
    }
}