namespace CyberClubManager.Notifications {
  /// <summary>
  /// Интерфейс издателя (субъекта) для управления и рассылки уведомлений наблюдателям.
  /// </summary>
  public interface ISessionSubject {
    /// <summary>
    /// Добавляет нового наблюдателя в список подписок.
    /// </summary>
    /// <param name="observer">Регистрируемый наблюдатель.</param>
    void Attach(ISessionObserver observer);

    /// <summary>
    /// Удаляет существующего наблюдателя из списка подписок.
    /// </summary>
    /// <param name="observer">Удаляемый наблюдатель.</param>
    void Detach(ISessionObserver observer);

    /// <summary>
    /// Оповещает всех зарегистрированных наблюдателей оставшемся алерте.
    /// </summary>
    /// <param name="alertMessage">Сообщение алерта.</param>
    void NotifyAlert(string alertMessage);
  }
}