using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace AxisCameraMPPlugin.Mvvm
{
	/// <summary>
	/// This class adds binding capabilities to the standard WPF PasswordBox.
	/// Originally from PasswordBox:
	/// http://peteohanlon.wordpress.com/2009/06/10/binding-passwords/
	/// NOTE: this is a workaround since the Password property on PasswordBox is not a 
	/// dependency property, it does decrease security.
	/// <example>
	/// Add this line to the PasswordBox:
	/// mvvm:PasswordBoxBehaviors.Password= 
	///		"{Binding Password, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" 
	/// </example>
	/// </summary> 
	public static class PasswordBoxBehaviors
	{
		/// <summary>
		/// Password Attached Dependency Property.
		/// This is the property that is bound in xaml.
		/// </summary>
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.RegisterAttached(
			"Password",
			typeof(string),
			typeof(PasswordBoxBehaviors),
			new UIPropertyMetadata(PasswordPropertyChanged));

		/// <summary>
		/// Initializes static members of the <see cref="PasswordBoxBehaviors"/> class.
		/// Used to hookup all password changed events from all PasswordBoxes.
		/// </summary>
		[SuppressMessage(
			"Microsoft.Performance",
			"CA1810:InitializeReferenceTypeStaticFieldsInline",
			Justification = "Cannot call RegisterClassHandler in another way.")]
		static PasswordBoxBehaviors()
		{
			EventManager.RegisterClassHandler(
				typeof(PasswordBox),
				PasswordBox.PasswordChangedEvent,
				new RoutedEventHandler(PasswordBoxChanged),
				true);
		}

		/// <summary>
		/// Gets the Password property.
		/// </summary>
		/// <param name="dependencyObject">The object to get it from.</param>
		/// <returns>The password.</returns>
		public static string GetPassword(DependencyObject dependencyObject)
		{
			return (string)dependencyObject.GetValue(PasswordProperty);
		}

		/// <summary>
		/// Sets the Password property.
		/// </summary>
		/// <param name="dependencyObject">The object to set it on.</param>
		/// <param name="value">The value to set.</param>
		public static void SetPassword(DependencyObject dependencyObject, string value)
		{
			dependencyObject.SetValue(PasswordProperty, value);
		}

		/// <summary>
		/// Handles changes to the password property.
		/// Sets the password box value from the new attached property value.
		/// </summary>
		/// <param name="d">The sender object.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> 
		/// instance containing the event data.</param>
		private static void PasswordPropertyChanged(
			DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = d as PasswordBox;
			if (passwordBox == null)
			{
				// We only support attaching this property on PasswordBox objects.
				return;
			}

			string newValue = e.NewValue as string;
			if (newValue != passwordBox.Password)
			{
				passwordBox.Password = newValue;
			}
		}

		/// <summary>
		/// Handles the password change event.
		/// Sets the attached property value from password box.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> 
		/// instance containing the event data.</param>
		private static void PasswordBoxChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			SetPassword(passwordBox, passwordBox.Password);
		}
	}
}
