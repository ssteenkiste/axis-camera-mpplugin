#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class describing information about the product.
	/// </summary>
	public class ProductInformation : IProductInformation
	{
		private readonly Lazy<FileVersionInfo> fileVersionInfo;


		/// <summary>
		/// Initializes a new instance of the <see cref="ProductInformation"/> class.
		/// </summary>
		public ProductInformation()
		{
			fileVersionInfo = new Lazy<FileVersionInfo>(() =>
				FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location));
		}


		/// <summary>
		/// Gets the product version.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
			Justification = "I am not sure on how to fix this actually.")]
		public string Version
		{
			get { return fileVersionInfo.Value.ProductVersion; }
		}
	}
}
