namespace CyberClubManager.Loyalty {
  /// <summary>
  /// Интерфейс для расчета стоимости услуг с учетом программы скидок и лояльности.
  /// </summary>
  public interface ILoyaltyPricingStrategy {
    /// <summary>
    /// Вычисляет итоговую стоимость сессии на основе параметров бронирования.
    /// </summary>
    /// <param name="username">Имя пользователя для проверки статуса постоянного гостя.</param>
    /// <param name="hoursRequested">Количество часов аренды.</param>
    /// <param name="baseHourlyRate">Базовая часовая ставка для выбранной зоны ПК.</param>
    /// <returns>Итоговая стоимость игровой сессии со скидками.</returns>
    double CalculateFinalCost(string username, int hoursRequested, double baseHourlyRate);
  }
}