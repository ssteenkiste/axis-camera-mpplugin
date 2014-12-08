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

using System.Collections.ObjectModel;
using System.Linq;
using AxisCameras.Configuration.Properties;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// View model where video source is entered if specified device is a video server.
    /// </summary>
    internal class WizardPageTwoViewModel : WizardPageViewModel, IWizardPageTwoViewModel
    {
        /// <summary>
        /// Gets the video sources.
        /// </summary>
        public ReadOnlyObservableCollection<int> VideoSources
        {
            get { return GetValue<ReadOnlyObservableCollection<int>>(); }
            private set { SetValue(value); }
        }

        /// <summary>
        /// Gets the selected video source.
        /// </summary>
        public int SelectedVideoSource
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets the header of the wizard page.
        /// </summary>
        public override string Header
        {
            get { return Resources.WizardPageTwo_Header; }
        }

        /// <summary>
        /// Gets the description of the wizard page.
        /// </summary>
        public override string Description
        {
            get { return Resources.WizardPageTwo_Description; }
        }

        /// <summary>
        /// Loads page properties from specified camera.
        /// </summary>
        /// <param name="camera">The camera to load page properties from.</param>
        public override void Load(ConfigurableCamera camera)
        {
            Requires.NotNull(camera);

            VideoSources = new ReadOnlyObservableCollection<int>(
                new ObservableCollection<int>(
                    Enumerable.Range(1, camera.VideoSourceCount)));

            SelectedVideoSource = camera.VideoSource;
        }

        /// <summary>
        /// Saves page properties to specified camera.
        /// </summary>
        /// <param name="camera">The camera to save page properties to.</param>
        public override void Save(ConfigurableCamera camera)
        {
            Requires.NotNull(camera);

            if (camera.VideoSource != SelectedVideoSource)
            {
                camera.VideoSource = SelectedVideoSource;

                // Remove snapshot if another video source is selected
                camera.Snapshot = null;
            }
        }

        /// <summary>
        /// Instruct the wizard whether wizard page should be skipped.
        /// </summary>
        /// <param name="camera">The camera displayed in the page.</param>
        /// <returns>true to skip page; otherwise false.</returns>
        public override bool ShouldSkipPage(ConfigurableCamera camera)
        {
            // Skip page if camera (device actually) only has one video source
            return camera.VideoSourceCount <= 1;
        }
    }
}