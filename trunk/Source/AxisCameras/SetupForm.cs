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
using System.Globalization;
using System.IO;
using System.Linq;
using Autofac;
using AxisCameras.Configuration;
using AxisCameras.Configuration.Service;
using AxisCameras.Data;
using AxisCameras.Player;
using AxisCameras.Properties;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using ActionType = MediaPortal.GUI.Library.Action.ActionType;
using Log = AxisCameras.Core.Log;
using PluginResources = AxisCameras.Properties.Resources;
using Utils = MediaPortal.Util.Utils;

namespace AxisCameras
{
	/// <summary>
	/// Class acting as setup form for the plugin.
	/// </summary>
	[PluginIcons(
		"AxisCameras.Resources.Setup_icon.png",
		"AxisCameras.Resources.Setup_icon_disabled.png")]
	public class SetupForm : GUIWindow, ISetupForm
	{
		private IContainer container;
		private Lazy<IEnumerable<Camera>> cameras;
		private Lazy<ICameraPlayer> player;
		private Lazy<IIOService> ioService;
		private Guid selectedCameraGuid;


		/// <summary>
		/// The facade layout containing cameras.
		/// </summary>
		[SkinControl(50)]
		private GUIFacadeControl facadeLayout = null;


		/// <summary>
		/// Initializes a new instance of the <see cref="SetupForm"/> class.
		/// </summary>
		public SetupForm()
		{
			Log.Info("Version: {0}", new ProductInformation().Version);
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
			strButtonImage = string.Empty;
			strButtonImageFocus = string.Empty;
			strPictureImage = "hover_AxisCameras.png";

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
		/// Returns the name of the module which is shown at the bottom of the plugin.
		/// </summary>
		public override string GetModuleName()
		{
			return PluginResources.Module_Name;
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
			player = new Lazy<ICameraPlayer>(() => container.Resolve<ICameraPlayer>());
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

			// Make sure any cameras have been added
			if (cameras.Value.Any())
			{
				// Add cameras
				foreach (Camera camera in cameras.Value)
				{
					facadeLayout.Add(CreateListItemFrom(camera));
				}

				// Sort list of cameras
				facadeLayout.Sort(new CameraComparer());

				// Set selected camera
				SetSelectedCamera();
			}
			else
			{
				// Display message to user about how to add cameras to the plugin
				int messageBoxId = (int)GUIWindow.Window.WINDOW_DIALOG_OK;

				using (GUIDialogOK messageBox = (GUIDialogOK)GUIWindowManager.GetWindow(messageBoxId))
				{
					messageBox.SetHeading(PluginResources.SetupForm_AddCameras_Title);
					messageBox.SetLines(PluginResources.SetupForm_AddCameras);
					messageBox.DoModal(GUIWindowManager.ActiveWindow);
				}
			}

			// Set item count property
			GUIPropertyManager.SetProperty("#itemcount", Utils.GetObjectCountLabel(cameras.Value.Count()));

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
				int selectedListItemIndex = facadeLayout.SelectedListItemIndex;
				if (selectedListItemIndex != -1)
				{
					PlayLiveViewFrom(selectedListItemIndex);
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
			GUIListItem listItem = new GUIListItem
			{
				Label = camera.Name,
				IconImage = ioService.Value.CameraIconFileName,
				ThumbnailImage = ioService.Value.GetThumbFileName(camera.Id),

				// Store camera in music tag
				MusicTag = camera
			};

			// Register for selected item changes
			listItem.OnItemSelected += (item, parent) =>
			{
				Camera selectedCamera = (Camera)item.MusicTag;
				selectedCameraGuid = selectedCamera.Id;
			};

			return listItem;
		}


		/// <summary>
		/// Sets the selected camera.
		/// </summary>
		private void SetSelectedCamera()
		{
			// Rather ugly way of setting the selected camera
			if (selectedCameraGuid == Guid.Empty && facadeLayout.Count > 0)
			{
				// Select first camera
				GUIControl.SelectItemControl(GetID, facadeLayout.GetID, 0);
			}
			else
			{
				for (int index = 0; index < facadeLayout.Count; index++)
				{
					Camera camera = (Camera)facadeLayout[index].MusicTag;

					if (camera.Id == selectedCameraGuid)
					{
						// Set the last selected camera
						GUIControl.SelectItemControl(GetID, facadeLayout.GetID, index);
						break;
					}
				}
			}
		}


		/// <summary>
		/// Plays live view from specified list item index representing a camera.
		/// </summary>
		/// <param name="selectedListItemIndex">
		/// The index of the list item to start playing live view from.
		/// </param>
		private void PlayLiveViewFrom(int selectedListItemIndex)
		{
			// The first played camera should be the one represented by the selected item, calculate
			// using modula the playlist by wrapping the index, e.g.
			//
			//   cameraCount = 4
			//   startIndex = 2
			//   stopIndex = 2 + 4 = 6
			//
			//   playlist = 2,3,0,1
			int cameraCount = cameras.Value.Count();
			int startIndex = selectedListItemIndex;
			int stopIndex = selectedListItemIndex + cameraCount;

			IEnumerable<Camera> cameraPlaylist =
				from listItemIndex in Enumerable.Range(startIndex, stopIndex)
				let listItem = facadeLayout[listItemIndex % cameraCount]
				select (Camera)listItem.MusicTag;

			player.Value.PlayLiveVideo(cameraPlaylist);
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


		/// <summary>
		/// Class comparing cameras, used when sorting the list of cameras.
		/// </summary>
		class CameraComparer : IComparer<GUIListItem>
		{
			/// <summary>
			/// Compares two GUIListItems and returns a value indicating whether one is less than, equal
			/// to, or greater than the other.
			/// </summary>
			/// <returns>
			/// Less than zero: x is less than y.
			/// Zero: x equals y.
			/// Greater than zero: x is greater than y.
			/// </returns>
			public int Compare(GUIListItem x, GUIListItem y)
			{
				if (x == y)
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return -1;
				}

				return string.Compare(x.Label, y.Label, false, CultureInfo.CurrentCulture);
			}
		}
	}
}
