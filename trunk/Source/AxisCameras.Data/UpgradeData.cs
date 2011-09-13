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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AxisCameras.Core.Contracts;
using AxisCameras.Data.IO;
using AxisCameras.Data.MediaPortal;
using AxisCameras.Data.Upgrades;

namespace AxisCameras.Data
{
	/// <summary>
	/// Class responsible for upgrading the data between version of the plugin.
	/// </summary>
	class UpgradeData : IUpgradeData
	{
		private const string BackupFileNameFormat = "{0}.bak";

		private readonly ISettings settings;
		private readonly IEnumerable<IPartialUpgrade> partialUpgrades;
		private readonly IIOService ioService;


		/// <summary>
		/// Initializes a new instance of the <see cref="UpgradeData"/> class.
		/// </summary>
		/// <param name="settings">The MediaPortal settings.</param>
		/// <param name="partialUpgrades">
		/// The partial upgrades, describing how to get from one version of the next.
		/// </param>
		/// <param name="ioService">The I/O service.</param>
		public UpgradeData(
			ISettings settings,
			IEnumerable<IPartialUpgrade> partialUpgrades,
			IIOService ioService)
		{
			Requires.NotNull(settings);
			Requires.NotNull(partialUpgrades);
			Requires.NotNull(ioService);

			this.settings = settings;
			this.partialUpgrades = partialUpgrades;
			this.ioService = ioService;
		}


		/// <summary>
		/// Gets a value indicating whether upgrading the data is required.
		/// </summary>
		public bool IsUpgradeRequired
		{
			get
			{
				if (ioService.FileExists(DataPersistenceInformation.FileName))
				{
					// Get the maximum data version stipulated by the partial upgrades
					int newestVersion = partialUpgrades.Max(pu => pu.ToVersion);

					// Upgrade is required if current version is lower than newest
					return CurrentVersion < newestVersion;
				}

				// The data file doesn't exist
				return false;
			}
		}


		/// <summary>
		/// Upgrades the data.
		/// </summary>
		/// <returns>true if upgrade was successful; otherwise false.</returns>
		public bool Upgrade()
		{
			// Start upgrade by backing up current data
			if (!Backup())
			{
				return false;
			}

			// Find the partial upgrades relevant when upgrading current version, and order them
			// according to version
			var relevantPartialUpgrades = partialUpgrades
				.Where(pu => pu.FromVersion >= CurrentVersion)
				.OrderBy(pu => pu.FromVersion);

			ValidateChainOfPartialUpgrades(relevantPartialUpgrades);

			foreach (var relevantPartialUpgrade in relevantPartialUpgrades)
			{
				if (!relevantPartialUpgrade.Upgrade())
				{
					// Upgrade failed
					return false;
				}

				// Save the current version after partial upgrade finished successfully, because if the
				// next upgrade fails, we would like to start upgrading from this version and not the
				// original version the next time the upgrade is executed.
				settings.SetValue(
					DataPersistenceInformation.DatabaseSection.Name,
					DataPersistenceInformation.DatabaseSection.VersionEntry,
					relevantPartialUpgrade.ToVersion);
			}

			// All partial upgrades finished successfully
			return true;
		}


		/// <summary>
		/// Gets the current data version.
		/// </summary>
		private int CurrentVersion
		{
			get
			{
				return settings.GetValueAsInt(
					DataPersistenceInformation.DatabaseSection.Name,
					DataPersistenceInformation.DatabaseSection.VersionEntry,
					1);
			}
		}


		/// <summary>
		/// Is backing up data.
		/// </summary>
		/// <returns>true if backup succeeded; otherwise false.</returns>
		private bool Backup()
		{
			string backupFileName = BackupFileNameFormat.InvariantFormat(
				DataPersistenceInformation.FileName);

			// Remove backup file if it exists
			if (ioService.FileExists(backupFileName))
			{
				if (!ioService.DeleteFile(backupFileName))
				{
					// Delete backup failed
					return false;
				}
			}

			return ioService.CopyFile(DataPersistenceInformation.FileName, backupFileName);
		}


		/// <summary>
		/// Validates that the chain of partial upgrades is connected. This method is only run in debug
		/// mode.
		/// </summary>
		[Conditional("DEBUG")]
		private void ValidateChainOfPartialUpgrades(IEnumerable<IPartialUpgrade> partialUpgrades)
		{
			if (partialUpgrades.Any())
			{
				var previous = partialUpgrades.First();

				// Make sure the first partial upgrade works on current version
				if (previous.FromVersion != CurrentVersion)
				{
					string message =
						"First partial upgrade with version '{0}' doesn't match current version '{1}'"
						.InvariantFormat(previous.FromVersion, CurrentVersion);

					throw new UpgradeChainException(message);
				}

				foreach (var current in partialUpgrades.Skip(1))
				{
					// The current version cannot continue from the previous version
					if (previous.ToVersion != current.FromVersion)
					{
						string message = "From version '{0}' doesn't connect to version '{1}'".InvariantFormat(
							previous.ToVersion,
							current.FromVersion);

						throw new UpgradeChainException(message);
					}

					previous = current;
				}
			}
		}
	}
}
