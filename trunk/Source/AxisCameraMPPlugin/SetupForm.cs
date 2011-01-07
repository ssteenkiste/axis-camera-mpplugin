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
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Properties;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using ActionType = MediaPortal.GUI.Library.Action.ActionType;
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
		private readonly IEnumerable<Camera> cameras;
		private readonly IPlayer player;
		private readonly IIOService ioService;
		private IContainer container;


		/// <summary>
		/// The facade layout containing cameras.
		/// </summary>
		[SkinControl(50)]
		protected GUIFacadeControl facadeLayout;


		/// <summary>
		/// Initializes a new instance of the <see cref="SetupForm"/> class.
		/// </summary>
		public SetupForm()
		{
			container = CreateContainer();
			cameras = container.Resolve<IPluginSettings>().GetCameras();
			player = container.Resolve<IPlayer>();
			ioService = container.Resolve<IIOService>();
		}


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
			strButtonImage = String.Empty;
			strButtonImageFocus = String.Empty;
			strPictureImage = String.Empty;

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
		/// Show the setup dialog
		/// </summary>
		public void ShowPlugin()
		{
			Log.Info("Show setup for {0}", PluginName());

			IConfigurationStarter configuration = container.Resolve<IConfigurationStarter>();
			configuration.Start();
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
			return Load(Path.Combine(GUIGraphicsContext.Skin, Settings.Default.Plugin_Skin));
		}


		/// <summary>
		/// Gets called just before the plugin is shown.
		/// </summary>
		protected override void OnPageLoad()
		{
			foreach (Camera camera in cameras)
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
					player.PlayLiveVideo(camera);
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
				IconImage = ioService.GetThumbPath("CameraPortrait.png"),
				ThumbnailImage = camera.SnapshotPath,

				// Store camera in music tag
				MusicTag = camera
			};
		}


		/// <summary>
		/// Creates a configured container.
		/// </summary>
		private IContainer CreateContainer()
		{
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule(new PluginModule());

			return builder.Build();
		}


		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting
		/// unmanaged resources.
		/// </summary>
		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the SetupForm is
		/// reclaimed by garbage collection.
		/// </summary>
		~SetupForm()
		{
			Dispose(false);
		}


		/// <summary>
		/// Releases unmanaged and optionally managed resources.
		/// </summary>
		/// <param name="disposing">
		/// true to release both managed and unmanaged resources; false to release only unmanaged
		/// resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			base.Dispose();

			if (disposing)
			{
				// Free managed resources
				if (container != null)
				{
					container.Dispose();
					container = null;
				}
			}
		}

		#endregion
	}
}
