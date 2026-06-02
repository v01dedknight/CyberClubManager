using System;
using System.Collections.Generic;
using CyberClubManager.Core;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Конкретная реализация субъекта для мониторинга времени сессий и отправки уведомлений.
  /// </summary>
  public class SessionNotificationService : ISessionSubject {
    private readonly List<ISessionObserver> _observers = new List<ISessionObserver>();

    /// <summary>
    /// Регистрирует наблюдателя.
    /// </summary>
    /// <param name="observer">Наблюдатель.</param>
    public void RegisterObserver(ISessionObserver observer) {
      if (observer == null) {
        throw new ArgumentNullException(nameof(observer));
      }

      if (!_observers.Contains(observer)) {
        _observers.Add(observer);
      }
    }

    /// <summary>
    /// Удаляет наблюдателя.
    /// </summary>
    /// <param name="observer">Наблюдатель.</param>
    public void RemoveObserver(ISessionObserver observer) {
      if (observer == null) {
        throw new ArgumentNullException(nameof(observer));
      }

      _observers.Remove(observer);
    }

    /// <summary>
    /// Рассылает уведомления всем наблюдателям.
    /// </summary>
    /// <param name="session">Сессия.</param>
    /// <param name="message">Сообщение.</param>
    public void NotifyObservers(GameSession session, string message) {
      for (int i = 0; i < _observers.Count; i++) {
        _observers[i].OnSessionAlert(session, message);
      }
    }

    /// <summary>
    /// Проверяет оставшееся время активных сессий и отправляет алерты при необходимости.
    /// </summary>
    /// <param name="activeSessions">Список активных сессий клуба.</param>
    /// <param name="currentTimeOverride">Фиктивное текущее время для юнит-тестирования.</param>
    public void CheckSessionsTime(IEnumerable<GameSession> activeSessions, DateTime? currentTimeOverride = null) {
      if (activeSessions == null) {
        return;
      }

      DateTime now = currentTimeOverride ?? DateTime.Now;

      foreach (GameSession session in activeSessions) {
        if (!session.IsActive) {
          continue;
        }

        TimeSpan elapsed = now - session.StartTime;
        double totalMinutes = session.HoursRequested * 60.0;
        double minutesRemaining = totalMinutes - elapsed.TotalMinutes;

        if (minutesRemaining <= 0) {
          NotifyObservers(session, $"ALERT: Session for player '{session.Username}' on PC #{session.PcId} has EXPIRED!");
        } else if (minutesRemaining <= 15) {
          NotifyObservers(session, $"WARNING: Only {Math.Ceiling(minutesRemaining)} minutes remaining for player '{session.Username}' on PC #{session.PcId}!");
        } else {
          NotifyObservers(session, $"STATUS: Session for player '{session.Username}' on PC #{session.PcId} is active. {Math.Ceiling(minutesRemaining)} minutes left.");
        }
      }
    }
  }
}