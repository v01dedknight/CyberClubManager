using System;
using System.Collections.Generic;
using System.IO;
using CyberClubManager.Core;
using CyberClubManager.Storage;
using CyberClubManager.App;
using NUnit.Framework;

namespace CyberClub.Tests {
  [TestFixture]
  public class BookingTests {
    private const string TestDbFileName = "test_cyberclub_db.txt";
    private string _tempFilePath;
    private FilePcRepository _repository;

    [SetUp]
    public void SetUp() {
      _tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestDbFileName);
      _repository = new FilePcRepository(_tempFilePath);
    }

    [TearDown]
    public void TearDown() {
      if (_tempFilePath != null && File.Exists(_tempFilePath)) {
        File.Delete(_tempFilePath);
      }
    }

    [Test]
    public void StandardPcFactory_ShouldCreate_StandardComputerWithCorrectSpecs() {
      // Arrange
      StandardPcFactory factory = new();
      int targetId = 7;

      // Act
      Computer pc = factory.CreatePc(targetId);

      // Assert
      Assert.Multiple(() => {
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc.Id, Is.EqualTo(targetId));
        Assert.That(pc.Zone, Is.EqualTo(PcZone.Standard));
        Assert.That(pc.IsOccupied, Is.False);
        Assert.That(pc.HardwareSpecs, Does.Contain("4060"));
      });
    }

    [Test]
    public void VipPcFactory_ShouldCreate_VipComputerWithExtraAmenities() {
      // Arrange
      VipPcFactory factory = new();
      int targetId = 99;

      // Act
      VipComputer? pc = factory.CreatePc(targetId) as VipComputer;

      // Assert
      Assert.Multiple(() => {
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc!.Zone, Is.EqualTo(PcZone.Vip));
        Assert.That(pc.ExtraAmenities, Is.Not.Empty);
      });
    }

    [Test]
    public void Repository_ShouldSaveAndGetAllComputers_Correctly() {
      // Arrange
      List<Computer> computers = [
        new StandardPcFactory().CreatePc(1),
        new VipPcFactory().CreatePc(2)
      ];
      computers[0].IsOccupied = true;

      // Act
      _repository.SaveAll(computers);
      List<Computer> loadedComputers = _repository.GetAll();

      // Assert
      Assert.Multiple(() => {
        Assert.That(loadedComputers, Has.Count.EqualTo(2));
        Assert.That(loadedComputers[0].Id, Is.EqualTo(1));
        Assert.That(loadedComputers[0].IsOccupied, Is.True, "First computer should be saved as occupied.");
        Assert.That(loadedComputers[1].Zone, Is.EqualTo(PcZone.Vip));
      });
    }

    [Test]
    public void StartSession_ShouldCalculateCorrectCostAndChangePcStatus() {
      // Arrange
      List<Computer> computers = [new VipPcFactory().CreatePc(3)];
      _repository.SaveAll(computers);
      BookingController controller = new(_tempFilePath);

      // Act
      GameSession session = controller.StartSession(pcId: 3, username: "PlayerOne", hours: 3);

      // Assert
      Assert.Multiple(() => {
        Assert.That(session.IsActive, Is.True);
        Assert.That(session.TotalCost, Is.EqualTo(900.0));

        List<Computer> updatedComputers = _repository.GetAll();
        Computer? targetPc = updatedComputers.Find(c => c.Id == 3);
        Assert.That(targetPc, Is.Not.Null, "PC in database should be present after session start.");
        Assert.That(targetPc!.IsOccupied, Is.True, "PC in database should be occupied after session start.");
      });
    }

    [Test]
    public void CloseSession_ShouldAddMoneyToTotalRevenue_WhenSessionEnds() {
      // Arrange
      List<Computer> computers = [new StandardPcFactory().CreatePc(1)];
      _repository.SaveAll(computers);
      BookingController controller = new(_tempFilePath);

      // Act
      controller.StartSession(pcId: 1, username: "PlayerTwo", hours: 2);
      controller.CloseSession(pcId: 1);

      // Assert
      Assert.That(controller.GetTotalRevenue(), Is.EqualTo(300.0), "Revenue should increase by 300 after session closes.");
    }
  }
}