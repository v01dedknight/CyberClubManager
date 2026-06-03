using CyberClubManager.Core;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Интерфейс наблюдателя для получения уведомлений о состоянии игровых сессий.
  /// </summary>
  public interface ISessionObserver {
    /// <summary>
    /// Вызывается при плановом обновлении и проверке оставшегося времени сессии.
    /// </summary>
    /// <param name="session">Объект игровой сессии.</param>
    /// <param name="minutesRemaining">Оставшееся количество минут.</param>
    void OnSessionTimeChecked(GameSession session, double minutesRemaining);

    /// <summary>
    /// Вызывается при срабатывании автоматических предупреждений и критических алертов.
    /// </summary>
    /// <param name="alertMessage">Текст сообщения об алерте.</param>
    void OnSessionAlertTriggered(string alertMessage);
  }
}