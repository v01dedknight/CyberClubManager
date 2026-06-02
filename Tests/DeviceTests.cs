using NUnit.Framework;
using CyberClubManager.Composite;

namespace CyberClub.Tests
{
    [TestFixture]
    public class DeviceTests
    {
        [Test]
        public void GetName_ReturnsDeviceName()
        {
            Device device = new Device("RTX 4070", "Video card");

            Assert.That(device.GetName(), Is.EqualTo("RTX 4070"));
        }

        [Test]
        public void BuildInfoLines_ReturnsDeviceInfoLine()
        {
            Device device = new Device("RTX 4070", "Video card");

            string actualInfoLine = device.BuildInfoLines(0)[0];

            Assert.That(actualInfoLine, Is.EqualTo("Device: Video card - RTX 4070"));
        }
    }
}
