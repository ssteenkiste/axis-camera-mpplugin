using System.Windows;
using System.Windows.Controls;

namespace AxisCameras.Mvvm.Behaviors
{
    /// <summary>
    /// This class adds binding capabilities to the standard WPF PasswordBox.
    /// Originally from PasswordBox:
    /// http://www.wpftutorial.net/PasswordBox.html
    /// NOTE: this is a workaround since the Password property on PasswordBox is not a 
    /// dependency property, it does decrease security.
    /// <example>
    /// Add this line to the PasswordBox:
    /// behaviors:PasswordHelper.Attach="True"
    /// behaviors:PasswordHelper.Password="{Binding Text, ElementName=plain, Mode=TwoWay}"
    /// </example>
    /// </summary> 
    public static class PasswordBoxBehaviors
    {
        /// <summary>
        /// Password attached dependency property. This is the property that is bound in xaml.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordBoxBehaviors),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// Attach attached dependency property. This is the property that enables this behavior.
        /// </summary>
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
            "Attach",
            typeof(bool),
            typeof(PasswordBoxBehaviors),
            new PropertyMetadata(false, Attach));

        /// <summary>
        /// Private internal attached dependency property that keeps track whether the behavior is
        /// updating the password.
        /// </summary>
        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating",
            typeof(bool),
            typeof(PasswordBoxBehaviors));

        /// <summary>
        /// Gets the Attach property.
        /// </summary>
        public static bool GetAttach(DependencyObject target)
        {
            return (bool)target.GetValue(AttachProperty);
        }

        /// <summary>
        /// Sets the Attach property.
        /// </summary>
        public static void SetAttach(DependencyObject target, bool value)
        {
            target.SetValue(AttachProperty, value);
        }

        /// <summary>
        /// Gets the Password property.
        /// </summary>
        public static string GetPassword(DependencyObject target)
        {
            return (string)target.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Sets the Password property.
        /// </summary>
        public static void SetPassword(DependencyObject target, string value)
        {
            target.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Gets the IsUpdating property.
        /// </summary>
        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        /// <summary>
        /// Sets the IsUpdating property.
        /// </summary>
        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
            else
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;

            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}