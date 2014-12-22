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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Windows.Markup;

[assembly: AssemblyTitle("Axis Cameras MVVM Framework")]
[assembly: AssemblyDescription("Assembly containing the MVVM framework used by the plugin.")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("51a6872c-2b44-490e-a309-8ff28c3a0c1d")]
[assembly: CLSCompliant(true)]

[assembly: InternalsVisibleTo("AxisCameras.MvvmTest")]

[assembly: XmlnsDefinition("http://schemas.microsoft.com/expression/2010/interactivity", "System.Windows.Interactivity")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/expression/2010/interactivity", "i")]