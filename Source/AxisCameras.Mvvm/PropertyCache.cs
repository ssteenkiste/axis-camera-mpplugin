using System.Collections.Generic;

namespace AxisCameras.Mvvm
{
    /// <summary>
    /// Class responsible for holding a cache of properties and their respective values.
    /// </summary>
    public class PropertyCache
    {
        private readonly Dictionary<string, object> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCache" /> class.
        /// </summary>
        public PropertyCache()
        {
            properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns the value of specified property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <returns>
        /// The value of specified property if found in cache; otherwise the default value for the
        /// type of the value parameter.
        /// </returns>
        public T GetValue<T>(string propertyName)
        {
            object value;
            if (properties.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Sets the value of specified property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>true if property value was changed; otherwise false.</returns>
        public bool SetValue<T>(string propertyName, T value)
        {
            // Get old value
            var oldValue = GetValue<T>(propertyName);

            // Make sure new value differs from old
            if (EqualityComparer<T>.Default.Equals(oldValue, value))
            {
                return false;
            }

            // Update property
            properties[propertyName] = value;
            return true;
        }
    }
}