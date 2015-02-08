// ReSharper disable once CheckNamespace
namespace System.Windows.Interactivity
{
    /// <summary>
    /// Class that compliments <see cref="Interaction" /> by allowing stlyes to contain behaviors
    /// and triggers.
    /// </summary>
    public static class StyleInteraction
    {
        #region Behaviors

        /// <summary>
        /// Behaviors dependency property.
        /// </summary>
        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
            "Behaviors",
            typeof(StyleBehaviorCollection),
            typeof(StyleInteraction),
            new UIPropertyMetadata(null, OnPropertyBehaviorsChanged));

        /// <summary>
        /// Gets the behaviors.
        /// </summary>
        public static StyleBehaviorCollection GetBehaviors(DependencyObject target)
        {
            return (StyleBehaviorCollection)target.GetValue(BehaviorsProperty);
        }

        /// <summary>
        /// Sets the behaviors.
        /// </summary>
        public static void SetBehaviors(DependencyObject target, StyleBehaviorCollection value)
        {
            target.SetValue(BehaviorsProperty, value);
        }

        private static void OnPropertyBehaviorsChanged(
            DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var behaviors = Interaction.GetBehaviors(target);
            
            foreach (var behavior in (StyleBehaviorCollection)e.NewValue)
            {
                behaviors.Add(behavior);
            }
        }

        #endregion

        #region Triggers

        /// <summary>
        /// Triggers dependency property.
        /// </summary>
        public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached(
            "Triggers",
            typeof(StyleTriggerCollection),
            typeof(StyleInteraction),
            new UIPropertyMetadata(null, OnPropertyTriggersChanged));

        /// <summary>
        /// Gets the triggers.
        /// </summary>
        public static StyleTriggerCollection GetTriggers(DependencyObject target)
        {
            return (StyleTriggerCollection)target.GetValue(TriggersProperty);
        }

        /// <summary>
        /// Sets the triggers.
        /// </summary>
        public static void SetTriggers(DependencyObject target, StyleTriggerCollection value)
        {
            target.SetValue(TriggersProperty, value);
        }

        private static void OnPropertyTriggersChanged(
            DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var triggers = Interaction.GetTriggers(target);

            foreach (var trigger in (StyleTriggerCollection)e.NewValue)
            {
                triggers.Add(trigger);
            }
        }

        #endregion
    }
}