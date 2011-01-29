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
using System.Collections.Generic;
using System.IO;
using Autofac;
using AxisCameraMPPlugin.Configuration;
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Core;
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Properties;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using ActionType = MediaPortal.GUI.Library.Action.ActionType;
using Log = AxisCameraMPPlugin.Core.Log;
using PluginResources = AxisCameraMPPlugin.Properties.Resources;

namespace AxisCameraMPPlugin
{
	/// <summary>
	/// Class acting as setup form for the plugin.
	/// </summary>
	[PluginIcons(
		"AxisCameraMPPlugin.Resources.Setup_icon.png",
		"AxisCameraMPPlugin.Resources.Setup_icon_disabled.png")]
	public class SetupForm : GUIWindow, ISetupForm
	{
		private IContainer container;
		private Lazy<IEnumerable<Camera>> cameras;
		private Lazy<IPlayer> player;
		private Lazy<IIOService> ioService;


		/// <summary>
		/// The facade layout containing cameras.
		/// </summary>
		[SkinControl(50)]
		protected GUIFacadeControl facadeLayout;


		/// <summary>
		/// Returns the author of the plugin which is shown in the plugin menu.
		/// </summary>
		public string Author()
		{
			return PluginResources.Plugin_Author;
		}


		/// <summary>
		/// Indicates whether plugin can be enabled/disabled.
		/// </summary>
		public bool CanEnable()
		{
			return true;
		}


		/// <summary>
		/// Indicates if plugin is enabled by default.
		/// </summary>
		public bool DefaultEnabled()
		{
			return true;
		}


		/// <summary>
		/// Returns the description of the plugin is shown in the plugin menu.
		/// </summary>
		public string Description()
		{
			return PluginResources.Plugin_Description;
		}


		/// <summary>
		/// If the plugin should have its own button on the home menu of MediaPortal then it should
		/// return true to this method, otherwise if it should not be on home it should return false.
		/// </summary>
		/// <param name="strButtonText">Text the button should have.</param>
		/// <param name="strButtonImage">Image for the button, or empty for default.</param>
		/// <param name="strButtonImageFocus">Image for the button, or empty for default.</param>
		/// <param name="strPictureImage">Sub-picture for the button or empty for none.</param>
		/// <returns>true if plugin needs its own button on home; otherwise false.</returns>
		public bool GetHome(
			out string strButtonText,
			out string strButtonImage,
			out string strButtonImageFocus,
			out string strPictureImage)
		{
			strButtonText = PluginName();
			strButtonImage = string.Empty;
			strButtonImageFocus = string.Empty;
			strPictureImage = "hover_axiscamerampplugin.png";

			return true;
		}


		/// <summary>
		/// Gets ID of window plugin belonging to this setup.
		/// </summary>
		public int GetWindowId()
		{
			return GetID;
		}


		/// <summary>
		/// Indicates if a plugin has its own setup screen.
		/// </summary>
		public bool HasSetup()
		{
			return true;
		}


		/// <summary>
		/// Returns the name of the plugin which is shown in the plugin menu.
		/// </summary>
		public string PluginName()
		{
			return PluginResources.Plugin_Name;
		}


		/// <summary>
		/// Show the setup dialog.
		/// </summary>
		public void ShowPlugin()
		{
			Log.Info("Show setup");

			// Allow unsafe header parsing due to backward compatibility with VAPIX
			NetworkConfiguration.SetAllowUnsafeHeaderParsing20();

			// Create container
			CreateContainer();

			// Start configuring the plugin
			IConfigurationStarter configuration = container.Resolve<IConfigurationStarter>();
			configuration.Start();

			// Dispose the container
			DisposeContainer();
		}


		/// <summary>
		/// Gets or sets the get ID.
		/// </summary>
		public override int GetID
		{
			get { return Settings.Default.Plugin_ID; }
			set { throw new InvalidOperationException("Setting the plugin ID is prohibited."); }
		}


		/// <summary>
		/// Gets called by the runtime when a new window has been created.
		/// </summary>
		/// <returns>true if initialization was successful; otherwise false.</returns>
		public override bool Init()
		{
			Log.Info("Init");

			CreateContainer();

			cameras = new Lazy<IEnumerable<Camera>>(() =>
			{
				using (IPluginSettings pluginSettings = container.Resolve<IPluginSettings>())
				{
					return pluginSettings.Cameras;
				}
			});
			player = new Lazy<IPlayer>(() => container.Resolve<IPlayer>());
			ioService = new Lazy<IIOService>(() => container.Resolve<IIOService>());

			return Load(Path.Combine(GUIGraphicsContext.Skin, Settings.Default.Plugin_Skin));
		}


		/// <summary>
		/// Gets called by the runtime when a  window will be destroyed.
		/// </summary>
		public override void DeInit()
		{
			Log.Info("DeInit");

			base.DeInit();

			// Dispose the container
			DisposeContainer();
		}


		/// <summary>
		/// Gets called just before the plugin is shown.
		/// </summary>
		protected override void OnPageLoad()
		{
			Log.Info("Page loaded");

			foreach (Camera camera in cameras.Value)
			{
				facadeLayout.Add(CreateListItemFrom(camera));
			}

			base.OnPageLoad();
		}


		/// <summary>
		/// Called when control was clicked.
		/// </summary>
		/// <param name="controlId">The control id.</param>
		/// <param name="control">The control.</param>
		/// <param name="actionType">Type of the action.</param>
		protected override void OnClicked(int controlId, GUIControl control, ActionType actionType)
		{
			base.OnClicked(controlId, control, actionType);

			if (control == facadeLayout && actionType == ActionType.ACTION_SELECT_ITEM)
			{
				GUIListItem selectedItem = facadeLayout.SelectedListItem;
				if (selectedItem != null)
				{
					Camera camera = (Camera)selectedItem.MusicTag;
					player.Value.PlayLiveVideo(camera);
				}
			}
		}


		/// <summary>
		/// Creates a list item representing a camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		/// <returns>A list item representing a camera.</returns>
		private GUIListItem CreateListItemFrom(Camera camera)
		{
			return new GUIListItem
			{
				Label = camera.Name,
				IconImage = ioService.Value.CameraIconPath,
				ThumbnailImage = camera.SnapshotPath,

				// Store camera in music tag
				MusicTag = camera
			};
		}


		/// <summary>
		/// Creates a configured container.
		/// </summary>
		private void CreateContainer()
		{
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule(new PluginModule());

			container = builder.Build();
		}


		/// <summary>
		/// Disposes the container.
		/// </summary>
		private void DisposeContainer()
		{
			if (container != null)
			{
				container.Dispose();
				container = null;
			}
		}
	}
}
