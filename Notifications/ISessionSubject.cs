using CyberClubManager.Core;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Интерфейс наблюдаемого субъекта для управления наблюдателями сессий.
  /// </summary>
  public interface ISessionSubject {
    /// <summary>
    /// Регистрирует нового наблюдателя в системе.
    /// </summary>
    /// <param name="observer">Экземпляр наблюдателя.</param>
    void RegisterObserver(ISessionObserver observer);

    /// <summary>
    /// Удаляет существующего наблюдателя из системы.
    /// </summary>
    /// <param name="observer">Экземпляр наблюдателя.</param>
    void RemoveObserver(ISessionObserver observer);

    /// <summary>
    /// Уведомляет всех зарегистрированных наблюдателей о событии.
    /// </summary>
    /// <param name="session">Объект игровой сессии.</param>
    /// <param name="message">Текст уведомления.</param>
    void NotifyObservers(GameSession session, string message);
  }
}