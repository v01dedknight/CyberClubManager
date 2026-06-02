using System;
using System.Collections.Generic;
using CyberClubManager.Core;
using CyberClubManager.Loyalty;
using CyberClubManager.Notifications;
using Moq;
using NUnit.Framework;

namespace CyberClub.Tests {
  /// <summary>
  /// Набор модульных тестов для верификации работы системы лояльности и уведомлений.
  /// </summary>
  [TestFixture]
  public class LoyaltyNotificationsTests {
    private Mock<ISessionObserver> _observerMock;
    private SessionTimeTracker _tracker;
    private LoyaltyPricingStrategy _pricingStrategy;

    /// <summary>
    /// Инициализация тестовой среды перед каждым запуском.
    /// </summary>
    [SetUp]
    public void SetUp() {
      _observerMock = new Mock<ISessionObserver>();
      _tracker = new SessionTimeTracker();
      _pricingStrategy = new LoyaltyPricingStrategy();
      _tracker.Attach(_observerMock.Object);
    }

    /// <summary>
    /// Проверяет применение комбинированной скидки (опт + VIP статус).
    /// </summary>
    [Test]
    public void CalculateFinalCost_VipUserAndLongHours_AppliesCombinedDiscount() {
      // Базовая цена 100, 5 часов -> 500. Скидка 10% (опт) + 15% (VIP) = 25%.
      // 500 * (1 - 0.25) = 375
      double finalCost = _pricingStrategy.CalculateFinalCost("VipGamer", 5, 100.0);

      Assert.That(finalCost, Is.EqualTo(375.0));
    }

    /// <summary>
    /// Проверяет расчет без скидок для новых пользователей с малым временем.
    /// </summary>
    [Test]
    public void CalculateFinalCost_RegularUserShortHours_NoDiscount() {
      double finalCost = _pricingStrategy.CalculateFinalCost("NewPlayer", 2, 100.0);

      Assert.That(finalCost, Is.EqualTo(200.0));
    }

    /// <summary>
    /// Проверяет генерацию критического алерта, если время сессии полностью истекло.
    /// </summary>
    [Test]
    public void CheckSessionsTime_SessionExpired_TriggersAlert() {
      GameSession expiredSession = new(1, "User1", 1, 150.0) {
        StartTime = DateTime.Now.AddHours(-2)
      };

      List<GameSession> sessions = [expiredSession];
      _tracker.CheckSessionsTime(sessions);

      _observerMock.Verify(obs => obs.OnSessionAlertTriggered(It.Is<string>(msg => msg.Contains("EXPIRED"))), Times.AtLeastOnce);
    }

    /// <summary>
    /// Проверяет, что предупреждение о скором завершении (осталось меньше 15 минут) срабатывает строго один раз.
    /// </summary>
    [Test]
    public void CheckSessionsTime_SessionWarning_TriggersWarningAlertOnce() {
      GameSession expiringSession = new(2, "User2", 1, 150.0) {
        StartTime = DateTime.Now.AddMinutes(-50)
      };

      List<GameSession> sessions = [expiringSession];
      _tracker.CheckSessionsTime(sessions);
      _tracker.CheckSessionsTime(sessions);

      _observerMock.Verify(obs => obs.OnSessionAlertTriggered(It.Is<string>(msg => msg.Contains("WARNING"))), Times.Once);
    }
  }
}