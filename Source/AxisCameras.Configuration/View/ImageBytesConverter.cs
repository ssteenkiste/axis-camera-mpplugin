﻿#region Copyright (C) 2005-2015 Team MediaPortal

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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AxisCameras.Configuration.View
{
    /// <summary>
    /// Converts from a byte sequence to a bitmap image.
    /// </summary>
    [ValueConversion(typeof(IEnumerable<byte>), typeof(BitmapImage))]
    internal class ImageBytesConverter : IValueConverter
    {
        /// <summary>
        /// Converts a sequence of bytes to a bitmap image.
        /// </summary>
        /// <param name="value">The sequence of bytes.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A bitmap image.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageBytes = value as IEnumerable<byte>;

            if (imageBytes == null)
                return Binding.DoNothing;
            if (targetType != typeof(ImageSource))
                return Binding.DoNothing;

            using (var stream = new MemoryStream(imageBytes.ToArray()))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();

                return image;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}