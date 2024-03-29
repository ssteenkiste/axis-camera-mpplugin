﻿#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
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
using System.Runtime.Serialization;

namespace AxisCameras.Data.Upgrades
{
    /// <summary>
    /// Exception thrown when there are gaps in the sequence of upgrades. A valid chain should be
    /// linked together by the FromVersion and ToVersion of IPartialUpgrade.
    /// </summary>
    [Serializable]
    public class UpgradeChainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeChainException"/> class.
        /// </summary>
        public UpgradeChainException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeChainException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UpgradeChainException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeChainException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public UpgradeChainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeChainException"/> class.
        /// </summary>
        /// <param name="info">
        /// The SerializationInfo that holds the serialized object data about the exception being
        /// thrown.
        /// </param>
        /// <param name="context">
        /// The StreamingContext that contains contextual information about the source or destination.
        /// </param>
        protected UpgradeChainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}