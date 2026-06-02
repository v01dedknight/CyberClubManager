using CyberClubManager.Core;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Интерфейс наблюдателя для системы уведомлений об игровых сессиях.
  /// </summary>
  public interface ISessionObserver {
    /// <summary>
    /// Метод, вызываемый при изменении состояния или отправке предупреждения по сессии.
    /// </summary>
    /// <param name="session">Объект игровой сессии.</param>
    /// <param name="message">Текст уведомления или предупреждения.</param>
    void OnSessionAlert(GameSession session, string message);
  }
}