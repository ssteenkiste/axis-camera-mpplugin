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

using System.Reflection;
using Autofac;
using AxisCameras.Core;
using AutofacModule = Autofac.Module;

namespace AxisCameras
{
    /// <summary>
    /// Module responsible for configuring Autofac.
    /// </summary>
    internal class PluginModule : AutofacModule
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        /// <remarks>Note that the ContainerBuilder parameter is unique to this module.</remarks>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterAssemblyTypes(
                builder,
                Assembly.GetExecutingAssembly(),
                Assembly.Load("AxisCameras.Configuration"),
                Assembly.Load("AxisCameras.Data"),
                Assembly.Load("AxisCameras.Mvvm"));
        }

        /// <summary>
        /// Registers all the classes found in specified assemblies as the interfaces they implement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="assemblies">The assemblies.</param>
        private static void RegisterAssemblyTypes(
            ContainerBuilder builder,
            params Assembly[] assemblies)
        {
            // Register all types 
            builder
                .RegisterAssemblyTypes(assemblies);

            // Register all classes as the interfaces they implement
            builder
                .RegisterAssemblyTypes(assemblies)
                .AsImplementedInterfaces();

            // Register all classes that implement ISingleInstance as SingleInstance.
            builder
                .RegisterAssemblyTypes(assemblies)
                .AssignableTo<ISingleInstance>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}