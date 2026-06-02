using System;
using System.Collections.Generic;
using CyberClubManager.Core;
using CyberClubManager.Storage;
using CyberClubManager.Notifications;

namespace CyberClubManager.App {
  /// <summary>
  /// Контроллер для управления бронированием и игровыми сессиями.
  /// </summary>
  public class BookingController {
    private readonly FilePcRepository _repository;
    private readonly List<Computer> _computers;
    private readonly List<GameSession> _activeSessions;
    private readonly SessionNotificationService _notificationService;
    private double _totalRevenue;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BookingController"/>.
    /// </summary>
    /// <param name="filePath">Путь к файлу базы данных.</param>
    public BookingController(string filePath) {
      _repository = new FilePcRepository(filePath);
      _computers = _repository.GetAll();
      _activeSessions = new List<GameSession>();
      _totalRevenue = 0.0;

      InitializeDefaultComputersIfEmpty();
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BookingController"/> с поддержкой уведомлений.
    /// </summary>
    /// <param name="filePath">Путь к файлу базы данных.</param>
    /// <param name="notificationService">Сервис уведомлений о сессиях.</param>
    public BookingController(string filePath, SessionNotificationService notificationService) : this(filePath) {
      _notificationService = notificationService;
    }

    /// <summary>
    /// Отображает текущий статус и тарифы всех компьютеров в зале.
    /// </summary>
    public void ListAllComputers() {
      Console.WriteLine("\n==== CURRENT HALL STATUS ====");
      foreach (Computer pc in _computers) {
        string status = pc.IsOccupied ? "[OCCUPIED]" : "[AVAILABLE]";
        Console.WriteLine($"ID: {pc.Id} | Zone: {pc.Zone,-8} | Status: {status,-11} | Rate: {pc.HourlyRate} USD/h");
      }
    }

    /// <summary>
    /// Выполняет быстрое бронирование компьютера по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор компьютера.</param>
    public void BookComputer(int id) {
      Computer pc = _computers.Find(c => c.Id == id);
      if (pc == null) {
        Console.WriteLine("Error: Computer with this ID not found.");
        return;
      }

      if (pc.IsOccupied) {
        Console.WriteLine("Error: This gaming place is already occupied.");
        return;
      }

      pc.IsOccupied = true;
      _repository.SaveAll(_computers);
      Console.WriteLine($"Success! Computer #{id} is booked.");
    }

    /// <summary>
    /// Открывает новую игровую сессию с расчетом стоимости и учетом скидок лояльности.
    /// </summary>
    /// <param name="pcId">Идентификатор компьютера.</param>
    /// <param name="username">Имя игрока.</param>
    /// <param name="hours">Количество запрашиваемых часов.</param>
    /// <returns>Созданная игровая сессия.</returns>
    /// <exception cref="InvalidOperationException">Бросается, если компьютер занят или не найден.</exception>
    public GameSession StartSession(int pcId, string username, int hours) {
      Computer pc = _computers.Find(c => c.Id == pcId);
      if (pc != null) {
        if (pc.IsOccupied) {
          throw new InvalidOperationException($"Computer #{pcId} is already occupied by a session.");
        }

        // Рассчитываем полную стоимость всей сессии через калькулятор Екатерины
        double totalCost = LoyaltyDiscountCalculator.CalculateTotalCost(username, hours, pc.HourlyRate);

        // Вычисляем эффективную часовую ставку со скидкой
        double finalHourlyRate = totalCost / hours;

        GameSession session = new GameSession(pcId, username, hours, finalHourlyRate);

        pc.IsOccupied = true;
        _repository.SaveAll(_computers);
        _activeSessions.Add(session);

        return session;
      }

      throw new InvalidOperationException($"Computer with ID {pcId} not found.");
    }

    /// <summary>
    /// Закрывает активную игровую сессию и обновляет выручку.
    /// </summary>
    /// <param name="pcId">Идентификатор компьютера.</param>
    public void CloseSession(int pcId) {
      GameSession session = _activeSessions.Find(s => s.PcId == pcId && s.IsActive);
      if (session == null) {
        Console.WriteLine($"Error: Active session for PC #{pcId} not found.");
        return;
      }

      Computer pc = _computers.Find(c => c.Id == pcId);
      if (pc != null) {
        pc.IsOccupied = false;
        _repository.SaveAll(_computers);
      }

      session.EndSession();
      _totalRevenue += session.TotalCost;
      _activeSessions.Remove(session);

      // Запускаем триггер проверки времени остальных сессий при изменении состояния зала (если сервис передан)
      _notificationService?.CheckSessionsTime(_activeSessions);

      Console.WriteLine($"Session for player {session.Username} on PC #{pcId} successfully closed. Added {session.TotalCost} USD to revenue.");
    }

    /// <summary>
    /// Отображает финансовый отчет игрового клуба.
    /// </summary>
    public void DisplayFinancialReport() {
      Console.WriteLine("\n==== CLUB FINANCIAL REPORT ====");
      Console.WriteLine($"Total revenue in register: {_totalRevenue} USD");
      Console.WriteLine($"Current active sessions: {_activeSessions.Count}");
    }

    /// <summary>
    /// Возвращает сумму общей выручки клуба.
    /// </summary>
    /// <returns>Общая выручка.</returns>
    public double GetTotalRevenue() => _totalRevenue;

    /// <summary>
    /// Возвращает список активных сессий только для чтения.
    /// </summary>
    /// <returns>Список сессий.</returns>
    public IReadOnlyList<GameSession> GetActiveSessions() => _activeSessions.AsReadOnly();

    /// <summary>
    /// Заполняет репозиторий компьютерами по умолчанию, если база данных пуста.
    /// </summary>
    private void InitializeDefaultComputersIfEmpty() {
      if (_computers.Count == 0) {
        _computers.Add(new StandardPcFactory().CreatePc(1));
        _computers.Add(new StandardPcFactory().CreatePc(2));
        _computers.Add(new VipPcFactory().CreatePc(3));
        _repository.SaveAll(_computers);
      }
    }
  }
}