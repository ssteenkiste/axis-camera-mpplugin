#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Mvvm.Validation
{
    /// <summary>
    /// Class responsible for validating a ViewModel.
    /// </summary>
    public class Validator
    {
        private readonly IList<ValidationData> rules;

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class.
        /// </summary>
        public Validator()
        {
            rules = new List<ValidationData>();
        }

        /// <summary>
        /// Adds a validation rule.
        /// </summary>
        /// <param name="nameExpression">The expression pointing to the property.</param>
        /// <param name="validationRule">The validation rule to add.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void Add(Expression<Func<object>> nameExpression, IValidationRule validationRule)
        {
            Requires.NotNull(nameExpression);
            Requires.NotNull(validationRule);

            rules.Add(new ValidationData(nameExpression, validationRule));
        }

        /// <summary>
        /// Validates a specific property on a ViewModel.
        /// </summary>
        /// <param name="propertyName">The property to validate.</param>
        /// <returns>
        /// The error messages for the property. If not errors are found, a empty sequence is
        /// returned.
        /// </returns>
        public IEnumerable<string> GetErrorMessagesFor(string propertyName)
        {
            Requires.NotNullOrEmpty(propertyName);

            ValidationData[] relevantRules = rules
                .Where(rule => rule.PropertyName == propertyName)
                .ToArray();

            // Validate
            foreach (ValidationData relevantRule in relevantRules)
            {
                relevantRule.IsValid = relevantRule.Rule.Validate(relevantRule.Property());
            }

            return relevantRules
                .Where(relevantRule => !relevantRule.IsValid)
                .Select(relevantRule => relevantRule.Rule.ErrorMessage)
                .ToArray();
        }

        /// <summary>
        /// Validates all added validation rules.
        /// </summary>
        /// <returns>true if validation succeeds; otherwise false.</returns>
        public bool Validate()
        {
            // Using Aggregate instead of All since we wish to run through all validators and not
            // stop on the first one failing
            return rules.Aggregate(
                true,
                (success, rule) => success && !GetErrorMessagesFor(rule.PropertyName).Any());
        }

        /// <summary>
        /// Gets a value indicating whether all added validation rules are valid.
        /// </summary>
        public bool IsValid
        {
            get { return rules.All(rule => rule.IsValid); }
        }

        /// <summary>
        /// Gets an IEnumerable that contains all properties that are invalid.
        /// </summary>
        /// <returns>Property names that failed validation.</returns>
        public IEnumerable<string> InvalidPropertyNames
        {
            get
            {
                return rules
                    .Where(rule => !rule.IsValid)
                    .Select(rule => rule.PropertyName)
                    .Distinct()
                    .ToArray();
            }
        }

        /// <summary>
        /// Class acting as data carrier for a validation information.
        /// </summary>
        private class ValidationData
        {
            private readonly string propertyName;
            private readonly Func<object> property;
            private readonly IValidationRule rule;

            /// <summary>
            /// Initializes a new instance of the <see cref="Validator"/> class.
            /// </summary>
            /// <param name="nameExpression">The expression pointing to the property.</param>
            /// <param name="rule">The rule.</param>
            public ValidationData(Expression<Func<object>> nameExpression, IValidationRule rule)
            {
                propertyName = nameExpression.GetName();
                property = nameExpression.Compile();
                this.rule = rule;
            }

            /// <summary>
            /// Gets the name of the property.
            /// </summary>
            public string PropertyName
            {
                get { return propertyName; }
            }

            /// <summary>
            /// Gets the name expression to validate.
            /// </summary>
            public Func<object> Property
            {
                get { return property; }
            }

            /// <summary>
            /// Gets the rule.
            /// </summary>
            public IValidationRule Rule
            {
                get { return rule; }
            }
            
            /// <summary>
            /// Gets or sets a value indicating whether this rule is valid.
            /// </summary>
            public bool IsValid { get; set; }
        }
    }
}