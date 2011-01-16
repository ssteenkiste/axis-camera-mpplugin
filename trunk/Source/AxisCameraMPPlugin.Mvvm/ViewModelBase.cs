#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using AxisCameraMPPlugin.Mvvm.Extensions.System.Linq.Expressions;
using AxisCameraMPPlugin.Mvvm.Validation;

namespace AxisCameraMPPlugin.Mvvm
{
	/// <summary>
	/// Base class for all ViewModels.
	/// </summary>
	public abstract class ViewModelBase : IViewModelBase, INotifyPropertyChanged, IDataErrorInfo
	{
		private readonly Dictionary<string, PropertyItem> properties;
		private Validator validator;
		private List<object> propertyObservers;


		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelBase"/> class.
		/// </summary>
		protected ViewModelBase()
		{
			properties = new Dictionary<string, PropertyItem>();
		}


		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;


		#region IDataErrorInfo Members

		/// <summary>
		/// Gets an error message indicating what is wrong with this object.
		/// </summary>
		public string Error
		{
			get { return string.Empty; }
		}


		/// <summary>
		/// Gets the error message for the property with the given name.
		/// </summary>
		/// <param name="columnName">The name of the property whose error message to get.</param>
		/// <returns>The error message for the property. The default is an empty string ("").</returns>
		public string this[string columnName]
		{
			get { return validator != null ? validator.Validate(columnName) : string.Empty; }
		}

		#endregion


		/// <summary>
		/// Validates all added validation rules.
		/// </summary>
		/// <returns>true if validation succeeds; otherwise false.</returns>
		public virtual bool Validate()
		{
			if (validator == null)
			{
				return true;
			}

			// Get invalid properties
			IEnumerable<string> propertyNamesWithErrors = validator.InvalidPropertyNames;

			// Send events about invalid properties
			OnPropertiesChanged(propertyNamesWithErrors);

			return !propertyNamesWithErrors.Any();
		}


		/// <summary>
		/// Gets a value indicating whether the view model is valid.
		/// </summary>
		public bool IsValid
		{
			get { return validator != null ? validator.IsValid : true; }
		}


		/// <summary>
		/// Registers a property observer on the view model.
		/// </summary>
		/// <typeparam name="T">
		/// The type the property observer is targeting, usually the view model itself.
		/// </typeparam>
		/// <param name="propertyObserver">The property observer.</param>
		public void RegisterPropertyObserver<T>(PropertyObserver<T> propertyObserver)
			where T : INotifyPropertyChanged
		{
			if (propertyObserver == null) throw new ArgumentNullException("propertyObserver");

			if (propertyObservers == null)
			{
				propertyObservers = new List<object>();
			}

			propertyObservers.Add(propertyObserver);
		}


		/// <summary>
		/// Adds a validation rule.
		/// </summary>
		/// <param name="nameExpression">The expression pointing to the property.</param>
		/// <param name="validationRule">The validation rule to add.</param>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected void AddValidator(
			Expression<Func<object>> nameExpression,
			IValidationRule validationRule)
		{
			if (validator == null)
			{
				validator = new Validator();
			}

			validator.Add(nameExpression, validationRule);
		}


		/// <summary>
		/// Represents the method that will handle the PropertyChanged event raised when a property is
		/// changed on a component.
		/// </summary>
		/// <param name="e">A PropertyChangedEventArgs that contains the event data.</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			VerifyPropertyName(e.PropertyName);

			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}


		/// <summary>
		/// Represents the method that will handle the PropertyChanged event raised when a property is
		/// changed on a component.
		/// </summary>
		/// <typeparam name="T">The property type.</typeparam>
		/// <param name="nameExpression">The expression pointing to the property.</param>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected void OnPropertyChanged<T>(Expression<Func<T>> nameExpression)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(nameExpression.GetName()));
		}


		/// <summary>
		/// Represents the method that will handle the PropertyChanged event raised when a property is
		/// changed on a component.
		/// </summary>
		/// <param name="propertyNames"></param>
		protected void OnPropertiesChanged(IEnumerable<string> propertyNames)
		{
			foreach (string propertyName in propertyNames)
			{
				OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
			}
		}


		/// <summary>
		/// Gets the value of a property matching the given expression.
		/// </summary>
		/// <typeparam name="T">The property type.</typeparam>
		/// <param name="nameExpression">The expression pointing to the property.</param>
		/// <returns>The property value if existing; otherwise default.</returns>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected T Property<T>(Expression<Func<T>> nameExpression)
		{
			PropertyItem p;
			if (properties.TryGetValue(nameExpression.ToString(), out p))
			{
				return (T)p.Value;
			}

			return default(T);
		}


		/// <summary>
		/// Sets the value of a property matching the given expression.
		/// </summary>
		/// <typeparam name="T">The property type.</typeparam>
		/// <param name="nameExpression">The expression pointing to the property.</param>
		/// <param name="value">The value to set.</param>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected void Property<T>(Expression<Func<T>> nameExpression, T value)
		{
			// Get the key of the property
			string key = nameExpression.ToString();

			PropertyItem p;
			if (properties.TryGetValue(key, out p))
			{
				// Make sure the property value has changed
				if ((p.Value == null && value == null) || (p.Value != null && p.Value.Equals(value)))
				{
					return;
				}

				// Set the new value
				p.Value = value;
			}
			else
			{
				// Create the new property item
				p = new PropertyItem
				{
					Name = nameExpression.GetName(),
					Value = value
				};

				// Add the new property item
				properties.Add(key, p);
			}

			// Raise property changed event
			OnPropertyChanged(new PropertyChangedEventArgs(p.Name));
		}


		/// <summary>
		/// Warns the developer if this object does not have a public property with the specified name.
		/// This  method does not exist in a Release build.
		/// </summary>
		/// <param name="propertyName">The property name.</param>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		private void VerifyPropertyName(string propertyName)
		{
			if (!(string.IsNullOrEmpty(propertyName) || (GetType().GetProperty(propertyName) != null)))
			{
				throw new ArgumentException("Not a property.", propertyName);
			}
		}


		/// <summary>
		/// Class wrapping up the essential parts of a property.
		/// </summary>
		class PropertyItem
		{
			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the value.
			/// </summary>
			public object Value { get; set; }
		}
	}
}
