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
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MediaPortal.Common.Utils;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("Axis Camera MP Plugin")]
[assembly: AssemblyDescription("A Axis camera plugin for the MediaPortal media center application.")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("16cbf278-1df1-44ae-94f1-97b282718148")]
[assembly: CLSCompliant(false)]
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]

// This attribute is used to specify against which version of MediaPortal the plugin was built. It
// may also optionally specify the minimum MediaPortal version required so that it can work
// properly. If minRequiredVersion is omitted it is assumed equal to designedForVersion. Any
// version of MediaPortal earlier than minRequiredVersion will consider the plugin incompatible and
// will not load it. If any subsystems used by the plugin have breaking changes in any version
// later than designedForVersion, the plugin will be considered incompatible and will not be
// loaded.
//
// The attribute can be applied to either the assemblies or classes. When applied to an assembly it
// will affect all contained classes that do not have the same attribute applied upon them.

[assembly: CompatibleVersion("1.1.6.27733", "1.1.6.27644")]

// This attribute is used to specify which subsystems the plugin is using. This information
// combined with the CompatibleVersion attribute information allows the Compatibility Manager to
// determine if a plugin is still compatible with a version of MediaPortal released after the
// release of the plugin. Specify only one subsystem (as a string) in the subsystem parameter.
//
// The attribute can be applied to either the assemblies or classes. When applied to an assembly it
// will effectively be applied to all contained classes. If some of the contained classes are also
// tagged with the same subsystem(s) the attribute applied to the classes takes precedence.

[assembly: UsesSubsystem("MP.SkinEngine.Controls")]
[assembly: UsesSubsystem("MP.SkinEngine.Dialogs")]
[assembly: UsesSubsystem("MP.Players.Video")]
[assembly: UsesSubsystem("MP.Config")]

// Provide access to internal types and methods to test functions

[assembly: InternalsVisibleTo("AxisCamerasTest")]