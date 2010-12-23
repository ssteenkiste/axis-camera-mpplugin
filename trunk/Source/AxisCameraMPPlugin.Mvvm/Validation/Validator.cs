using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using AxisCameraMPPlugin.Mvvm.Extensions.System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;

namespace AxisCameraMPPlugin.Mvvm.Validation
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
			if (nameExpression == null) throw new ArgumentNullException("nameExpression");
			if (validationRule == null) throw new ArgumentNullException("validationRule");
			
			rules.Add(new ValidationData(nameExpression, validationRule));
		}


		/// <summary>
		/// Validates a specific property on a ViewModel.
		/// </summary>
		/// <param name="propertyName">The property to validate.</param>
		/// <returns>The error message for the property. The default is an empty string ("").</returns>
		public string Validate(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentException("Property name cannot be null or empty.", "propertyName");
			
			IEnumerable<ValidationData> relevantRules = rules.Where(r => r.Name == propertyName);

			foreach (ValidationData relevantRule in relevantRules)
			{
				if (!relevantRule.Rule.Validate(relevantRule.Property()))
				{
					return relevantRule.Rule.ErrorMessage;
				}
			}

			return string.Empty;
		}


		/// <summary>
		/// Validates all added validation rules.
		/// </summary>
		/// <returns>true if validation succeeds; otherwise false.</returns>
		public bool ValidateAll()
		{
			return rules.Aggregate(
				true,
				(success, rule) => success && string.IsNullOrEmpty(Validate(rule.Name)));
		}


		/// <summary>
		/// Class acting as data carrier for a validation information.
		/// </summary>
		private class ValidationData
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Validator"/> class.
			/// </summary>
			/// <param name="nameExpression">The expression pointing to the property.</param>
			/// <param name="rule">The rule.</param>
			public ValidationData(Expression<Func<object>> nameExpression, IValidationRule rule)
			{
				Name = nameExpression.GetName();
				Property = nameExpression.Compile();
				Rule = rule;
			}


			/// <summary>
			/// Gets the name of the property.
			/// </summary>
			public string Name { get; private set; }


			/// <summary>
			/// Gets the name expression to validate.
			/// </summary>
			public Func<object> Property { get; private set; }


			/// <summary>
			/// Gets the rule.
			/// </summary>
			public IValidationRule Rule { get; private set; }
		}
	}
}
