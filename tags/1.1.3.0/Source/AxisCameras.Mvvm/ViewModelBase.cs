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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AxisCameras.Core.Contracts;
using AxisCameras.Mvvm.Behaviors;
using AxisCameras.Mvvm.Validation;

namespace AxisCameras.Mvvm
{
    /// <summary>
    /// Base class for all ViewModels.
    /// </summary>
    public abstract class ViewModelBase : IViewModelBase, IDataErrorInfo
    {
        private readonly PropertyCache propertyCache;
        private readonly Lazy<ICommand> lazyLoadedCommand;
        private readonly Lazy<ICommand> lazyUnloadedCommand;
        private Validator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            propertyCache = new PropertyCache();
            lazyLoadedCommand = new Lazy<ICommand>(() => new RelayCommand(_ => OnLoaded()));
            lazyUnloadedCommand = new Lazy<ICommand>(() => new RelayCommand(_ => OnUnloaded()));
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets value indicating whether view is loaded.
        /// </summary>
        /// <value>
        /// true if view is loaded, false is view is unloaded or null if indetermined.
        /// </value>
        public bool? IsLoaded
        {
            get { return GetValue<bool?>(); }
            private set { SetValue(value); }
        }

        /// <summary>
        /// Gets the command executed when view is loaded.
        /// </summary>
        public ICommand LoadedCommand
        {
            get { return lazyLoadedCommand.Value; }
        }

        /// <summary>
        /// Gets the command executed when view is unloaded.
        /// </summary>
        public ICommand UnloadedCommand
        {
            get { return lazyUnloadedCommand.Value; }
        }

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
            get
            {
                if (validator == null)
                {
                    // If no validator exist, no error exists
                    return string.Empty;
                }

                if (lazyLoadedCommand.IsValueCreated && IsLoaded != true)
                {
                    // If view isn't loaded, no need to display errors
                    return string.Empty;
                }

                IEnumerable<string> errorMessages = validator.GetErrorMessagesFor(columnName);
                return string.Join(Environment.NewLine, errorMessages);
            }
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

            bool isValid = validator.Validate();

            // Get invalid properties
            string[] propertyNamesWithErrors = validator.InvalidPropertyNames.ToArray();

            // Send events about invalid properties
            OnPropertiesChanged(propertyNamesWithErrors);

            return isValid;
        }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid
        {
            get { return validator == null || validator.IsValid; }
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
            Requires.NotNull(e);

            VerifyPropertyName(e.PropertyName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Sends a property changed notification on specified property of this view model.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="nameExpression">The expression pointing to the property.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected void OnPropertyChanged<T>(Expression<Func<T>> nameExpression)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameExpression.GetName()));
        }

        /// <summary>
        /// Sends a property changed notification on specified property of this view model.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sends a property changed notification on specified properties of this view model.
        /// </summary>
        /// <param name="propertyNames">The properties to send notifications on.</param>
        protected void OnPropertiesChanged(IEnumerable<string> propertyNames)
        {
            Requires.NotNull(propertyNames);

            foreach (string propertyName in propertyNames)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Sends a property changed notification on all properties of this view model.
        /// </summary>
        protected void OnAllPropertiesChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Returns the value of specified property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of specified property.</returns>
        protected T GetValue<T>([CallerMemberName] string propertyName = "")
        {
            VerifyPropertyName(propertyName);

            return propertyCache.GetValue<T>(propertyName);
        }

        /// <summary>
        /// Sets the value of specified property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">
        /// Is not used since it is marked with <see cref="CallerMemberNameAttribute"/>.
        /// </param>
        /// <returns>true if property value was changed; otherwise false.</returns>
        protected bool SetValue<T>(T value, [CallerMemberName] string propertyName = "")
        {
            VerifyPropertyName(propertyName);

            bool isChanged = propertyCache.SetValue(propertyName, value);
            if (isChanged)
            {
                OnPropertyChanged(propertyName);
            }

            return isChanged;
        }

        /// <summary>
        /// Called when view is loaded.
        /// </summary>
        /// <remarks>
        /// Make sure view binds <see cref="WindowLifetimeBehaviors.LoadedProperty"/> or
        /// <see cref="LifetimeBehaviors.LoadedProperty"/> to <see cref="LoadedCommand"/>
        /// in order for this method to be called.
        /// </remarks>
        protected virtual void OnLoaded()
        {
            IsLoaded = true;
        }

        /// <summary>
        /// Called when view is unloaded.
        /// </summary>
        /// <remarks>
        /// Make sure view binds <see cref="WindowLifetimeBehaviors.UnloadedProperty"/> or
        /// <see cref="LifetimeBehaviors.UnloadedProperty"/> to <see cref="UnloadedCommand"/>
        /// in order for this method to be called.
        /// </remarks>
        protected virtual void OnUnloaded()
        {
            IsLoaded = false;
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
    }
}