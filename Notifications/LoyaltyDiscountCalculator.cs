using System;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Статический класс для расчета тарифов и скидок в системе лояльности компьютерного клуба.
  /// </summary>
  public static class LoyaltyDiscountCalculator {
    /// <summary>
    /// Вычисляет итоговую стоимость сессии с учетом скидок за время сессии и статуса клиента.
    /// </summary>
    /// <param name="username">Имя учетной записи игрока.</param>
    /// <param name="hours">Количество заказываемых часов.</param>
    /// <param name="baseHourlyRate">Базовая стоимость одного часа для выбранного ПК.</param>
    /// <returns>Итоговая стоимость с применением всех скидок.</returns>
    public static double CalculateTotalCost(string username, int hours, double baseHourlyRate) {
      if (hours <= 0) {
        throw new ArgumentException("Session hours must be greater than zero.", nameof(hours));
      }

      if (baseHourlyRate < 0) {
        throw new ArgumentException("Hourly rate cannot be negative.", nameof(baseHourlyRate));
      }

      double baseCost = hours * baseHourlyRate;
      double discountPercentage = 0.0;

      // 1. Скидка за длительность сессии (прогрессивный тариф)
      if (hours >= 5) {
        discountPercentage += 15.0;
      } else if (hours >= 3) {
        discountPercentage += 10.0;
      }

      // 2. Скидка по системе лояльности за статус учетной записи постоянного гостя
      if (!string.IsNullOrEmpty(username)) {
        string lowerName = username.ToLower();
        if (lowerName.Contains("pro") || lowerName.Contains("vip") || lowerName.Contains("regular")) {
          discountPercentage += 5.0;
        }
      }

      // Ограничение максимальной суммарной скидки лояльности в 30%
      if (discountPercentage > 30.0) {
        discountPercentage = 30.0;
      }

      double discountAmount = baseCost * (discountPercentage / 100.0);
      return baseCost - discountAmount;
    }
  }
}