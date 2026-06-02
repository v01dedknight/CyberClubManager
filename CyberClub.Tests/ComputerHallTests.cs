using System.Collections.Generic;
using NUnit.Framework;
using CyberClubManager.Composite;

namespace CyberClub.Tests
{
    [TestFixture]
    public class ComputerHallTests
    {
        [Test]
        public void AddComponent_IncreasesComponentCount()
        {
            ComputerHall hall = new ComputerHall("VIP hall", 2);
            Computer computer = new Computer("VIP-PC-01", "RTX 4070, Core i7, 32GB RAM", 2);

            hall.AddComponent(computer);

            Assert.That(hall.GetComponentCount(), Is.EqualTo(1));
        }

        [Test]
        public void BuildInfoLines_ReturnsFullClubStructure()
        {
            ComputerHall gamingClub = new ComputerHall("Gaming club", 2);
            ComputerHall vipHall = new ComputerHall("VIP hall", 2);
            Computer vipComputer = new Computer("VIP-PC-01", "RTX 4070, Core i7, 32GB RAM", 2);
            Device videoCard = new Device("RTX 4070", "Video card");

            vipComputer.AddComponent(videoCard);
            vipHall.AddComponent(vipComputer);
            gamingClub.AddComponent(vipHall);

            List<string> infoLineList = gamingClub.BuildInfoLines(0);

            Assert.That(infoLineList[0], Is.EqualTo("Hall: Gaming club"));
            Assert.That(infoLineList[1], Is.EqualTo("  Hall: VIP hall"));
            Assert.That(infoLineList[2], Is.EqualTo("    Computer: VIP-PC-01"));
            Assert.That(infoLineList[3], Is.EqualTo("    Specs: RTX 4070, Core i7, 32GB RAM"));
            Assert.That(infoLineList[4], Is.EqualTo("      Device: Video card - RTX 4070"));
        }
    }
}
