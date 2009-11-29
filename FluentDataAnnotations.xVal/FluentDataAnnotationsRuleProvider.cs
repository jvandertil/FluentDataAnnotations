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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using xVal.RuleProviders;
using xVal.Rules;

namespace FluentDataAnnotations.xVal
{
    namespace FluentMetadata.xValIntegration
    {
        public class FluentDataAnnotationsRuleProvider : CachingRulesProvider
        {
            readonly RuleEmitterList<ValidationAttribute> _ruleEmitters = new RuleEmitterList<ValidationAttribute>();
            private readonly DataAnnotationsTypeDescriptionProvider _provider;

            public FluentDataAnnotationsRuleProvider(DataAnnotationsTypeDescriptionProvider provider)
            {
                _provider = provider;

                _ruleEmitters.AddSingle<StringLengthAttribute>(x => new StringLengthRule(x.MinimumLength, x.MaximumLength));
                _ruleEmitters.AddSingle<RangeAttribute>(ConvertRangeAttribute);
                _ruleEmitters.AddSingle<DataTypeAttribute>(ConvertDataTypeAttribute);
                _ruleEmitters.AddSingle<RequiredAttribute>(x => new RequiredRule());
                _ruleEmitters.AddSingle<RegularExpressionAttribute>(x => new RegularExpressionRule(x.Pattern));
            }

            #region Copied from xVal DataAnnotationsRuleProvider (Property of: Steven Sanderson)
            protected override RuleSet GetRulesFromTypeCore(Type type)
            {
                var typeDescriptor = _provider.GetTypeDescriptor(type);
                var rules = (from prop in typeDescriptor.GetProperties().Cast<PropertyDescriptor>()
                             from rule in GetRulesFromProperty(prop)
                             select new KeyValuePair<string, Rule>(prop.Name, rule));

                return new RuleSet(rules.ToLookup(x => x.Key, x => x.Value));
            }

            protected virtual IEnumerable<Rule> GetRulesFromProperty(PropertyDescriptor propertyDescriptor)
            {
                return from att in propertyDescriptor.Attributes.OfType<ValidationAttribute>()
                       from validationRule in MakeValidationRulesFromAttribute(att)
                       where validationRule != null
                       select validationRule;
            }

            private static readonly Type[] NumericTypes = new[] { typeof(int), typeof(double), typeof(decimal), typeof(float) };

            private Rule ConvertDataTypeAttribute(DataTypeAttribute dt)
            {
                // Is this one that should be handled as a RegEx?
                string regEx = ToRegEx(dt.DataType);
                if (regEx != null)
                    return new RegularExpressionRule(regEx, RegexOptions.IgnoreCase);
                // No, it must be one we have a native type for
                var xValDataType = ToXValDataType(dt.DataType);
                if (xValDataType != DataTypeRule.DataType.Text) // Ignore "text" - nothing to validate
                    return new DataTypeRule(xValDataType);
                return null;
            }

            private string ToRegEx(DataType dataType)
            {
                switch (dataType)
                {
                    case DataType.Time:
                        return RegularExpressionRule.Regex_Time;
                    case DataType.Duration:
                        return RegularExpressionRule.Regex_Duration;
                    case DataType.PhoneNumber:
                        return RegularExpressionRule.Regex_USPhoneNumber;
                    case DataType.Url:
                        return RegularExpressionRule.Regex_Url;
                    default:
                        return null;
                }
            }

            private static DataTypeRule.DataType ToXValDataType(DataType type)
            {
                switch (type)
                {
                    case DataType.DateTime:
                        return DataTypeRule.DataType.DateTime;
                    case DataType.Date:
                        return DataTypeRule.DataType.Date;
                    case DataType.Currency:
                        return DataTypeRule.DataType.Currency;
                    case DataType.EmailAddress:
                        return DataTypeRule.DataType.EmailAddress;
                    case DataType.Custom:
                    case DataType.Text:
                    case DataType.Html:
                    case DataType.MultilineText:
                    case DataType.Password:
                        return DataTypeRule.DataType.Text;
                    default:
                        throw new InvalidOperationException("Unknown data type: " + type.ToString());
                }
            }



            protected IEnumerable<Rule> MakeValidationRulesFromAttribute(ValidationAttribute att)
            {
                var rules = _ruleEmitters.EmitRules(att);
                foreach (var rule in rules)
                    ApplyErrorMessage(att, rule);
                return rules;
            }

            private static void ApplyErrorMessage(ValidationAttribute att, Rule result)
            {
                if (att.ErrorMessage != null)
                    result.ErrorMessage = att.ErrorMessage;
                else
                {
                    result.ErrorMessageResourceType = att.ErrorMessageResourceType;
                    result.ErrorMessageResourceName = att.ErrorMessageResourceName;
                }
            }


            private static Rule ConvertRangeAttribute(RangeAttribute r)
            {
                if (r.OperandType == typeof(string))
                    return new RangeRule(Convert.ToString(r.Minimum), Convert.ToString(r.Maximum));
                else if (r.OperandType == typeof(DateTime))
                    return new RangeRule(r.Minimum == null ? (DateTime?)null : Convert.ToDateTime(r.Minimum), r.Maximum == null ? (DateTime?)null : Convert.ToDateTime(r.Maximum));
                else if (Array.IndexOf(NumericTypes, r.OperandType) >= 0)
                    return new RangeRule(r.Minimum == null ? (decimal?)null : Convert.ToDecimal(r.Minimum), r.Maximum == null ? (decimal?)null : Convert.ToDecimal(r.Maximum));
                else // Can't compare any other type
                    return null;
            }

            #endregion
        }
    }
}
