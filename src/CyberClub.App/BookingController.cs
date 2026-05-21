using System;
using System.Collections.Generic;
using CyberClubManager.Core;
using CyberClubManager.Storage;

namespace CyberClubManager.App {
  /// <summary>
  /// Контроллер управления бронированием и игровыми сессиями.
  /// </summary>
  public class BookingController {
    private readonly FilePcRepository _repository;
    private readonly List<Computer> _computers;
    private readonly List<GameSession> _activeSessions;
    private double _totalRevenue; // Наша касса

    public BookingController(string filePath) {
      _repository = new FilePcRepository(filePath);
      _computers = _repository.GetAll();
      _activeSessions = new List<GameSession>();
      _totalRevenue = 0.0;

      // Если файл базы пуст, генерируем начальный зал для теста
      if (_computers.Count == 0) {
        _computers.Add(new StandardPcFactory().CreatePc(1));
        _computers.Add(new StandardPcFactory().CreatePc(2));
        _computers.Add(new VipPcFactory().CreatePc(3));
        _repository.SaveAll(_computers);
      }
    }

    public void ListAllComputers() {
      Console.WriteLine("\n==== ТЕКУЩИЙ СТАТУС ЗАЛА ====");
      foreach (var pc in _computers) {
        string status = pc.IsOccupied ? "[ЗАНЯТ]" : "[СВОБОДЕН]";
        Console.WriteLine($"ID: {pc.Id} | Зона: {pc.Zone,-8} | Статус: {status,-10} | Тариф: {pc.HourlyRate} руб/ч");
      }
    }

    public void BookComputer(int id) {
      var pc = _computers.Find(c => c.Id == id);
      if (pc == null) {
        Console.WriteLine("Ошибка: Компьютер с таким ID не найден.");
        return;
      }

      if (pc.IsOccupied) {
        Console.WriteLine("Ошибка: Это игровое место уже занято.");
        return;
      }

      pc.IsOccupied = true;
      _repository.SaveAll(_computers);
      Console.WriteLine($"Успех! Компьютер №{id} забронирован.");
    }

    public GameSession StartSession(int pcId, string username, int hours) {
      var pc = _computers.Find(c => c.Id == pcId);
      if (pc == null)
        throw new InvalidOperationException($"Компьютер с ID {pcId} не найден.");

      if (pc.IsOccupied)
        throw new InvalidOperationException($"Компьютер №{pcId} уже занят сессией.");

      var session = new GameSession(pcId, username, hours, pc.HourlyRate);

      pc.IsOccupied = true;
      _repository.SaveAll(_computers);
      _activeSessions.Add(session);

      return session;
    }

    public void CloseSession(int pcId) {
      var session = _activeSessions.Find(s => s.PcId == pcId && s.IsActive);
      if (session == null) {
        Console.WriteLine($"Ошибка: Активная сессия для ПК №{pcId} не найдена.");
        return;
      }

      var pc = _computers.Find(c => c.Id == pcId);
      if (pc != null) {
        pc.IsOccupied = false;
        _repository.SaveAll(_computers);
      }

      session.EndSession();
      _totalRevenue += session.TotalCost; // Деньги падают в кассу при закрытии
      _activeSessions.Remove(session);
      Console.WriteLine($"Сессия игрока {session.Username} на ПК №{pcId} успешно завершена. В кассу добавлено {session.TotalCost} руб.");
    }

    /// <summary>
    /// Вывод финансового отчета по клубу.
    /// </summary>
    public void DisplayFinancialReport() {
      Console.WriteLine("\n==== ФИНАНСОВЫЙ ОТЧЕТ КЛУБА ====");
      Console.WriteLine($"Общая выручка в кассе: {_totalRevenue} руб.");
      Console.WriteLine($"Количество активных сессий сейчас: {_activeSessions.Count}");
    }

    public double GetTotalRevenue() => _totalRevenue;

    public IReadOnlyList<GameSession> GetActiveSessions() => _activeSessions.AsReadOnly();
  }
}