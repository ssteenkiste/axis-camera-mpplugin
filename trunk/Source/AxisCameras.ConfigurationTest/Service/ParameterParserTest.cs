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

using AxisCameras.Configuration.Service;
using NUnit.Framework;

namespace AxisCameras.ConfigurationTest.Service
{
    [TestFixture]
    public class ParameterParserTest
    {
        [Test]
        public void SingleParameter()
        {
            var parser = new ParameterParser();
            var result = parser.Parse("Network.UPnP.FriendlyName=AXIS 210 - 00408C6D796F\n");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.ContainsKey("Network.UPnP.FriendlyName"), Is.True);
            Assert.That(result["Network.UPnP.FriendlyName"], Is.EqualTo("AXIS 210 - 00408C6D796F"));
        }

        [Test]
        public void ParameterGroup()
        {
            var parser = new ParameterParser();
            var result = parser.Parse(
                "Network.UPnP.Enabled=yes\n" +
                "Network.UPnP.FriendlyName=AXIS 210 - 00408C6D796F\n" +
                "Network.UPnP.NATTraversal.Enabled=no\n" +
                "Network.UPnP.NATTraversal.Router=\n" +
                "Network.UPnP.NATTraversal.ExternalIPAddress=\n" +
                "Network.UPnP.NATTraversal.Active=no\n" +
                "Network.UPnP.NATTraversal.MinPort=32768\n" +
                "Network.UPnP.NATTraversal.MaxPort=65535\n");

            Assert.That(result.Count, Is.EqualTo(8));
            Assert.That(result.ContainsKey("Network.UPnP.Enabled"), Is.True);
            Assert.That(result.ContainsKey("Network.UPnP.FriendlyName"), Is.True);
            Assert.That(result.ContainsKey("Network.UPnP.NATTraversal.Enabled"), Is.True);
            Assert.That(result.ContainsKey("Network.UPnP.NATTraversal.Router"), Is.True);
            Assert.That(result.ContainsKey("Network.UPnP.NATTraversal.ExternalIPAddress"), Is.True);
            Assert.That(result.ContainsKey("Network.UPnP.NATTraversal.Active"), Is.True);
            Assert.That(result.ContainsKey("Network.UPnP.NATTraversal.MinPort"), Is.True);
            Assert.That(result.ContainsKey("Network.UPnP.NATTraversal.MaxPort"), Is.True);

            Assert.That(result["Network.UPnP.Enabled"], Is.EqualTo("yes"));
            Assert.That(result["Network.UPnP.FriendlyName"], Is.EqualTo("AXIS 210 - 00408C6D796F"));
            Assert.That(result["Network.UPnP.NATTraversal.Enabled"], Is.EqualTo("no"));
            Assert.That(result["Network.UPnP.NATTraversal.Router"], Is.EqualTo(""));
            Assert.That(result["Network.UPnP.NATTraversal.ExternalIPAddress"], Is.EqualTo(""));
            Assert.That(result["Network.UPnP.NATTraversal.Active"], Is.EqualTo("no"));
            Assert.That(result["Network.UPnP.NATTraversal.MinPort"], Is.EqualTo("32768"));
            Assert.That(result["Network.UPnP.NATTraversal.MaxPort"], Is.EqualTo("65535"));
        }

        [Test]
        public void EqualSign()
        {
            var parser = new ParameterParser();
            var result = parser.Parse("name=val=ue\n");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.ContainsKey("name"), Is.True);
            Assert.That(result["name"], Is.EqualTo("val=ue"));
        }

        [Test]
        public void WhiteSpaces()
        {
            var parser = new ParameterParser();
            var result = parser.Parse(" name = value \n ");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.ContainsKey("name"), Is.True);
            Assert.That(result["name"], Is.EqualTo("value"));
        }
    }
}