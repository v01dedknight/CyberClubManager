using System;
using CyberClubManager.Core;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Компонент отображения автоматических алертов в консоль администратора клуба.
  /// </summary>
  public class AdminConsoleLogger : ISessionObserver {
    /// <summary>
    /// Обрабатывает плановые проверки времени сессии.
    /// </summary>
    /// <param name="session">Текущая сессия.</param>
    /// <param name="minutesRemaining">Оставшееся время.</param>
    public void OnSessionTimeChecked(GameSession session, double minutesRemaining) {
      // Метод зарезервирован для детального фонового мониторинга при необходимости
    }

    /// <summary>
    /// Выводит стилизованное критическое сообщение в консоль администратора.
    /// </summary>
    /// <param name="alertMessage">Текст сообщения.</param>
    public void OnSessionAlertTriggered(string alertMessage) {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"[ADMIN ALERT] [{DateTime.Now:HH:mm:ss}] {alertMessage}");
      Console.ResetColor();
    }
  }
}