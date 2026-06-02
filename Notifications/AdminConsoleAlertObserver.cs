using System;
using CyberClubManager.Core;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Конкретный наблюдатель для вывода критических алертов в консоль администратора клуба.
  /// </summary>
  public class AdminConsoleAlertObserver : ISessionObserver {
    /// <summary>
    /// Обрабатывает полученное событие и выводит предупреждения в консоль.
    /// </summary>
    /// <param name="session">Игровая сессия.</param>
    /// <param name="message">Сообщение.</param>
    public void OnSessionAlert(GameSession session, string message) {
      if (string.IsNullOrWhiteSpace(message)) {
        return;
      }

      if (message.Contains("ALERT")) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ADMIN CRITICAL] {message}");
        Console.ResetColor();
      } else if (message.Contains("WARNING")) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[ADMIN WARNING] {message}");
        Console.ResetColor();
      } else {
        Console.WriteLine($"[ADMIN LOG] {message}");
      }
    }
  }
}