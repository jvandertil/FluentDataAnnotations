using System;
using System.ComponentModel.DataAnnotations;

namespace FluentDataAnnotations.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DenyClientModificationAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format("You are not allowed to modify field {0}.", name);
        }
    }
}