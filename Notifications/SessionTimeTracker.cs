using System;
using System.Collections.Generic;
using CyberClubManager.Core;

namespace CyberClubManager.Notifications {
  /// <summary>
  /// Класс для автоматического отслеживания времени сессий и уведомления подписчиков.
  /// </summary>
  public class SessionTimeTracker : ISessionSubject {
    private readonly List<ISessionObserver> _observerList = new List<ISessionObserver>();
    private readonly HashSet<int> _warnedSessions = new HashSet<int>();
    private readonly double _sessionDuration = 15.0;

    /// <summary>
    /// Регистрирует наблюдателя.
    /// </summary>
    /// <param name="observer">Объект наблюдателя.</param>
    public void Attach(ISessionObserver observer) {
      if (observer == null) {
        throw new ArgumentNullException(nameof(observer));
      }

      if (!_observerList.Contains(observer)) {
        _observerList.Add(observer);
      }
    }

    /// <summary>
    /// Отменяет регистрацию наблюдателя.
    /// </summary>
    /// <param name="observer">Объект наблюдателя.</param>
    public void Detach(ISessionObserver observer) {
      if (observer != null) {
        _observerList.Remove(observer);
      }
    }

    /// <summary>
    /// Рассылает алерт всем текущим наблюдателям.
    /// </summary>
    /// <param name="alertMessage">Текст уведомления.</param>
    public void NotifyAlert(string alertMessage) {
      for (int index = 0; index < _observerList.Count; index++) {
        _observerList[index].OnSessionAlertTriggered(alertMessage);
      }
    }

    /// <summary>
    /// Выполняет итерационный обход активных сессий и уведомляет о лимитах времени.
    /// </summary>
    /// <param name="activeSessions">Коллекция активных игровых сессий.</param>
    public void CheckSessionsTime(IEnumerable<GameSession> activeSessions) {
      if (activeSessions == null) {
        return;
      }

      DateTime currentTime = DateTime.Now;

      foreach (GameSession session in activeSessions) {
        if (!session.IsActive) {
          continue;
        }

        DateTime endTime = session.StartTime.AddHours(session.HoursRequested);
        TimeSpan remainingTime = endTime - currentTime;
        double minutesRemaining = remainingTime.TotalMinutes;

        for (int index = 0; index < _observerList.Count; index++) {
          _observerList[index].OnSessionTimeChecked(session, minutesRemaining);
        }

        if (minutesRemaining <= 0) {
          NotifyAlert($"ALERT: Session for player '{session.Username}' on PC #{session.PcId} has EXPIRED!");
        } else if (minutesRemaining <= _sessionDuration && !_warnedSessions.Contains(session.PcId)) {
          _warnedSessions.Add(session.PcId);
          NotifyAlert($"WARNING: Only {Math.Round(minutesRemaining, 1)} minutes left for player '{session.Username}' on PC #{session.PcId}.");
        }
      }
    }

    /// <summary>
    /// Сбрасывает триггер предупреждения для ПК при открытии новой сессии.
    /// </summary>
    /// <param name="pcId">Идентификатор компьютера.</param>
    public void ResetPcWarningStatus(int pcId) {
      _warnedSessions.Remove(pcId);
    }
  }
}