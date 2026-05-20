using System;
using System.Collections.Generic;
using System.IO;
using CyberClubManager.Core;
using CyberClubManager.Storage;
using NUnit.Framework;

namespace CyberClub.Tests {
  [TestFixture]
  public class BookingTests {
    private string _tempFilePath;
    private FilePcRepository _repository;

    [SetUp]
    public void SetUp() {
      // Создаем временный файл для каждого теста, чтобы они были изолированы
      _tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_cyberclub_db.txt");
      _repository = new FilePcRepository(_tempFilePath);
    }

    [TearDown]
    public void TearDown() {
      // Удаляем временный файл после прохождения теста
      if (File.Exists(_tempFilePath)) {
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
      Assert.IsNotNull(pc);
      Assert.AreEqual(targetId, pc.Id);
      Assert.AreEqual(PcZone.Standard, pc.Zone);
      Assert.IsFalse(pc.IsOccupied);
      Assert.IsTrue(pc.HardwareSpecs.Contains("4060"));
    }

    [Test]
    public void VipPcFactory_ShouldCreate_VipComputerWithExtraAmenities() {
      // Arrange
      var factory = new VipPcFactory();
      int targetId = 99;

      // Act
      var pc = factory.CreatePc(targetId) as VipComputer;

      // Assert
      Assert.IsNotNull(pc);
      Assert.AreEqual(PcZone.Vip, pc.Zone);
      Assert.IsNotEmpty(pc.ExtraAmenities);
    }

    [Test]
    public void Repository_ShouldSaveAndGetAllComputers_Correctly() {
      // Arrange
      var computers = new List<Computer>
      {
                new StandardPcFactory().CreatePc(1),
                new VipPcFactory().CreatePc(2)
            };
      computers[0].IsOccupied = true; // Имитируем бронь

      // Act
      _repository.SaveAll(computers);
      var loadedComputers = _repository.GetAll();

      // Assert
      Assert.AreEqual(2, loadedComputers.Count);
      Assert.AreEqual(1, loadedComputers[0].Id);
      Assert.IsTrue(loadedComputers[0].IsOccupied, "Первый компьютер должен быть сохранен как занятый.");
      Assert.AreEqual(PcZone.Vip, loadedComputers[1].Zone);
    }
  }
}