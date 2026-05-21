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
    private string? _tempFilePath;
    private FilePcRepository? _repository;

    [SetUp]
    public void SetUp() {
      _tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_cyberclub_db.txt");
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
      var factory = new StandardPcFactory();
      int targetId = 7;

      // Act
      var pc = factory.CreatePc(targetId);

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
      var factory = new VipPcFactory();
      int targetId = 99;

      // Act
      var pc = factory.CreatePc(targetId) as VipComputer;

      // Assert
      Assert.Multiple(() => {
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc!.Zone, Is.EqualTo(PcZone.Vip));
        Assert.That(pc!.ExtraAmenities, Is.Not.Empty);
      });
    }

    [Test]
    public void Repository_ShouldSaveAndGetAllComputers_Correctly() {
      // Arrange
      var computers = new List<Computer>
      {
                new StandardPcFactory().CreatePc(1),
                new VipPcFactory().CreatePc(2)
            };
      computers[0].IsOccupied = true;

      // Act
      _repository!.SaveAll(computers);
      var loadedComputers = _repository.GetAll();

      // Assert
      Assert.Multiple(() => {
        Assert.That(loadedComputers, Has.Count.EqualTo(2));
        Assert.That(loadedComputers[0].Id, Is.EqualTo(1));
        Assert.That(loadedComputers[0].IsOccupied, Is.True, "Первый компьютер должен быть сохранен как занятый.");
        Assert.That(loadedComputers[1].Zone, Is.EqualTo(PcZone.Vip));
      });
    }

    [Test]
    public void StartSession_ShouldCalculateCorrectCostAndChangePcStatus() {
      // Arrange
      var computers = new List<Computer> { new VipPcFactory().CreatePc(3) };
      _repository!.SaveAll(computers);

      var controller = new BookingController(_tempFilePath!);

      // Act
      var session = controller.StartSession(pcId: 3, username: "Stepanyan", hours: 3);

      // Assert
      Assert.Multiple(() => {
        Assert.That(session.IsActive, Is.True);
        // Тариф VIP = 300. 300 * 3 часа = 900.0
        Assert.That(session.TotalCost, Is.EqualTo(900.0));

        // Проверяем, что изменения улетели в репозиторий
        var updatedComputers = _repository.GetAll();
        var targetPc = updatedComputers.Find(c => c.Id == 3);
        Assert.That(targetPc!.IsOccupied, Is.True, "ПК в базе должен стать занятым после старта сессии.");
      });
    }

    [Test]
    public void CloseSession_ShouldAddMoneyToTotalRevenue_WhenSessionEnds() {
      // Arrange
      var computers = new List<Computer> { new StandardPcFactory().CreatePc(1) };
      _repository!.SaveAll(computers);
      var controller = new BookingController(_tempFilePath!);

      // Act
      controller.StartSession(pcId: 1, username: "Guts", hours: 2); // 150 * 2 = 300
      controller.CloseSession(pcId: 1);

      // Assert
      Assert.That(controller.GetTotalRevenue(), Is.EqualTo(300.0), "Касса должна пополниться на 300 рублей после закрытия сессии.");
    }
  }
}