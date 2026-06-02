using System;

namespace CyberClubManager.Loyalty {
  /// <summary>
  /// Стратегия расчета тарифов и скидок для системы лояльности игрового клуба.
  /// </summary>
  public class LoyaltyPricingStrategy : ILoyaltyPricingStrategy {
    private const double HourlyBulkThresholdFive = 5.0;
    private const double HourlyBulkThresholdThree = 3.0;
    private const double MaxDiscountPercentage = 0.50;

    /// <summary>
    /// Рассчитывает финальную стоимость бронирования с учетом скидок по времени и имени.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <param name="hoursRequested">Количество часов.</param>
    /// <param name="baseHourlyRate">Почасовой тариф компьютера.</param>
    /// <returns>Полная стоимость сессии.</returns>
    public double CalculateFinalCost(string username, int hoursRequested, double baseHourlyRate) {
      if (hoursRequested <= 0) {
        return 0.0;
      }

      double activeDiscount = 0.0;

      // 1. Оптовая скидка за объем купленного времени
      if (hoursRequested >= HourlyBulkThresholdFive) {
        activeDiscount += 0.10;
      } else if (hoursRequested >= HourlyBulkThresholdThree) {
        activeDiscount += 0.05;
      }

      // 2. Персональная скидка постоянного или привилегированного клиента
      if (!string.IsNullOrEmpty(username)) {
        string standardName = username.Trim().ToLowerInvariant();

        if (standardName.StartsWith("vip", StringComparison.Ordinal) || standardName.Equals("artem", StringComparison.Ordinal)) {
          activeDiscount += 0.15;
        }
      }

      // Ограничение максимальной скидки в клубе
      if (activeDiscount > MaxDiscountPercentage) {
        activeDiscount = MaxDiscountPercentage;
      }

      double ratePerHour = baseHourlyRate * (1.0 - activeDiscount);
      return ratePerHour * hoursRequested;
    }
  }
}