using System;
using System.Collections.Generic;
using CyberClubManager.Core;
using CyberClubManager.Storage;

namespace CyberClubManager.App {
  /// <summary>
  /// Контроллер управления бронированием игровых мест.
  /// </summary>
  public class BookingController {
    private readonly FilePcRepository _repository;
    private readonly List<Computer> _computers;

    public BookingController(string filePath) {
      _repository = new FilePcRepository(filePath);
      _computers = _repository.GetAll();

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
        Console.WriteLine("Ошибка: Это игровое место уже занято сессией.");
        return;
      }

      pc.IsOccupied = true;
      _repository.SaveAll(_computers);
      Console.WriteLine($"Успех! Компьютер №{id} успешно забронирован под игровую сессию.");
    }
  }
}