using System.Collections.Generic;
using NUnit.Framework;
using CyberClubManager.ComputerManagement;

namespace CyberClub.Tests
{
    [TestFixture]
    public class ComputerConfigurationManagerTests
    {
        [Test]
        public void BuildDefaultClubConfiguration_CreatesVipAndCommonHalls()
        {
            ComputerConfigurationManager manager = new ComputerConfigurationManager();

            manager.BuildDefaultClubConfiguration();
            List<string> lines = manager.GetClubConfigurationLines();

            Assert.That(lines, Does.Contain("  Hall: VIP hall"));
            Assert.That(lines, Does.Contain("  Hall: Common hall"));
        }

        [Test]
        public void BuildDefaultClubConfiguration_AddsDevicesAndSpecs()
        {
            ComputerConfigurationManager manager = new ComputerConfigurationManager();

            manager.BuildDefaultClubConfiguration();
            List<string> lines = manager.GetClubConfigurationLines();

            Assert.That(lines, Does.Contain("    Computer: VIP-PC-01"));
            Assert.That(lines, Does.Contain("    Specs: RTX 4070, Core i7, 32GB RAM, SSD 1TB"));
            Assert.That(lines, Does.Contain("      Device: Video card - RTX 4070"));
        }
    }
}
