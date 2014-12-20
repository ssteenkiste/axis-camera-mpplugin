#region Copyright (C) 2005-2014 Team MediaPortal

// Copyright (C) 2005-2014 Team MediaPortal
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
using System.Windows;
using System.Windows.Input;

namespace AxisCameras.Mvvm.Behaviors
{
    /// <summary>
    /// This class is used to attach the view lifetime events to ICommand implementations.
    /// It allows a ViewModel to hook into the lifetime of the view (when necessary) 
    /// through simple XAML tags. Supported events are Loaded and Unloaded. For the
    /// Closing/Closed event, the CanExecute handler is invoked in response to the Closing
    /// event - if it returns true, then the Closed event is allowed and the Execute
    /// handler is called in response.
    /// 
    /// http://cinch.codeplex.com/
    /// </summary>
    /// <example>
    /// <![CDATA[  <Button Cinch:LifetimeEvent.Onloaded="{Binding CloseCommand}" />  ]]>
    /// </example>
    public static class LifetimeBehaviors
    {
        #region Loaded

        /// <summary>
        /// Dependency property which holds the ICommand for the Loaded event
        /// </summary>
        public static readonly DependencyProperty LoadedProperty =
            DependencyProperty.RegisterAttached(
                "Loaded",
                typeof(ICommand),
                typeof(LifetimeBehaviors),
                new UIPropertyMetadata(null, OnLoadedEventInfoChanged));

        /// <summary>
        /// Attached Property getter to retrieve the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>ICommand</returns>
        public static ICommand GetLoaded(DependencyObject source)
        {
            return (ICommand)source.GetValue(LoadedProperty);
        }

        /// <summary>
        /// Attached Property setter to change the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="command">ICommand</param>
        public static void SetLoaded(DependencyObject source, ICommand command)
        {
            source.SetValue(LoadedProperty, command);
        }

        /// <summary>
        /// This is the property changed handler for the Loaded property.
        /// </summary>
        /// <param name="sender">Dependency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnLoadedEventInfoChanged(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Loaded -= OnViewLoaded;
                if (e.NewValue != null)
                {
                    frameworkElement.Loaded += OnViewLoaded;
                    // Workaround: depending on the properties of the element, it's possible the Loaded event was already raised
                    // This happens when the View is created before the ViewModel is applied to the DataContext.  In this
                    // case, raise the Loaded event as soon as we know about it.
                    if (frameworkElement.IsLoaded)
                        OnViewLoaded(sender, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// This is the handler for the Loaded event to raise the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnViewLoaded(object sender, EventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand loadedCommand = GetLoaded(dpo);
            if (loadedCommand != null)
                loadedCommand.Execute(GetCommandParameter(dpo));
        }

        #endregion

        #region Unloaded

        /// <summary>
        /// Dependency property which holds the ICommand for the Unloaded event
        /// </summary>
        public static readonly DependencyProperty UnloadedProperty =
            DependencyProperty.RegisterAttached(
                "Unloaded",
                typeof(ICommand),
                typeof(LifetimeBehaviors),
                new UIPropertyMetadata(null, OnUnloadedEventInfoChanged));

        /// <summary>
        /// Attached Property getter to retrieve the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>ICommand</returns>
        public static ICommand GetUnloaded(DependencyObject source)
        {
            return (ICommand)source.GetValue(UnloadedProperty);
        }

        /// <summary>
        /// Attached Property setter to change the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="command">ICommand</param>
        public static void SetUnloaded(DependencyObject source, ICommand command)
        {
            source.SetValue(UnloadedProperty, command);
        }

        /// <summary>
        /// This is the property changed handler for the Unloaded property.
        /// </summary>
        /// <param name="sender">Dependency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnUnloadedEventInfoChanged(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Unloaded -= OnViewUnloaded;
                if (e.NewValue != null)
                    frameworkElement.Unloaded += OnViewUnloaded;
            }
        }

        /// <summary>
        /// This is the handler for the Unloaded event to raise the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnViewUnloaded(object sender, EventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand deactivatedCommand = GetUnloaded(dpo);
            if (deactivatedCommand != null)
                deactivatedCommand.Execute(GetCommandParameter(dpo));
        }

        #endregion

        #region CommandParameter

        /// <summary>
        /// Parameter for the ICommand
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached(
                "CommandParameter",
                typeof(object),
                typeof(LifetimeBehaviors),
                new UIPropertyMetadata(null));

        /// <summary>
        /// This retrieves the CommandParameter used for the command.
        /// </summary>
        /// <param name="source">Dependency object</param>
        /// <returns>Command Parameter passed to ICommand</returns>
        public static object GetCommandParameter(DependencyObject source)
        {
            return source.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// This changes the CommandParameter used with this command.
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="value">New Value</param>
        public static void SetCommandParameter(DependencyObject source, object value)
        {
            source.SetValue(CommandParameterProperty, value);
        }

        #endregion
    }
}
