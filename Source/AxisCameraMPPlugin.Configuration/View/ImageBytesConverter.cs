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
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AxisCameraMPPlugin.Configuration.View
{
	/// <summary>
	/// Converts from a byte array to a bitmap image.
	/// </summary>
	[ValueConversion(typeof(byte[]), typeof(BitmapImage))]
	class ImageBytesConverter : IValueConverter
	{
		/// <summary>
		/// Converts a byte array to a bitmap image.
		/// </summary>
		/// <param name="value">The byte array.</param>
		/// <returns>A bitmap image.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			byte[] imageBytes = value as byte[];

			if (imageBytes == null) return Binding.DoNothing;
			if (targetType != typeof(ImageSource)) return Binding.DoNothing;

			MemoryStream stream = new MemoryStream(imageBytes);

			BitmapImage image = new BitmapImage();
			image.BeginInit();
			image.StreamSource = stream;
			image.EndInit();

			return image;
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
