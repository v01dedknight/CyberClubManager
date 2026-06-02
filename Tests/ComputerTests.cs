using System.Collections.Generic;
using NUnit.Framework;
using CyberClubManager.Composite;

namespace CyberClub.Tests
{
    [TestFixture]
    public class ComputerTests
    {
        [Test]
        public void AddComponent_IncreasesDeviceCount()
        {
            Computer computer = new Computer("VIP-PC-01", "RTX 4070, Core i7, 32GB RAM", 2);
            Device device = new Device("Logitech G Pro", "Mouse");

            computer.AddComponent(device);

            Assert.That(computer.GetComponentCount(), Is.EqualTo(1));
        }

        [Test]
        public void BuildInfoLines_ReturnsComputerSpecsAndDeviceLines()
        {
            Computer computer = new Computer("VIP-PC-01", "RTX 4070, Core i7, 32GB RAM", 2);
            Device device = new Device("Logitech G Pro", "Mouse");

            computer.AddComponent(device);

            List<string> infoLineList = computer.BuildInfoLines(0);

            Assert.That(infoLineList[0], Is.EqualTo("Computer: VIP-PC-01"));
            Assert.That(infoLineList[1], Is.EqualTo("Specs: RTX 4070, Core i7, 32GB RAM"));
            Assert.That(infoLineList[2], Is.EqualTo("  Device: Mouse - Logitech G Pro"));
        }
    }
}
