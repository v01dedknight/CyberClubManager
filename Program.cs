using System;
using CyberClubManager.App;
using CyberClubManager.Core;

namespace CyberClubManager {
  internal class Program {
    private static void Main(string[] args) {
      // База данных будет лежать рядом с исполняемым файлом приложения
      string dbPath = "cyberclub_data.txt";
      BookingController controller = new BookingController(dbPath);

      Console.WriteLine("Добро пожаловать в CyberClub Manager!");

      while (true) {
        Console.WriteLine("\n=== МЕНЮ УПРАВЛЕНИЯ ===");
        Console.WriteLine("1. Показать статус зала и тарифы");
        Console.WriteLine("2. Забронировать ПК (Быстрое бронирование)");
        Console.WriteLine("3. Открыть игровую сессию (С расчётом стоимости)");
        Console.WriteLine("4. Завершить игровую сессию");
        Console.WriteLine("5. Показать активные сессии");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");

        string input = Console.ReadLine();

        try {
          switch (input) {
            case "1":
              controller.ListAllComputers();
              break;

            case "2":
              Console.Write("Введите ID компьютера для бронирования: ");
              if (int.TryParse(Console.ReadLine(), out int bookId)) {
                controller.BookComputer(bookId);
              } else {
                Console.WriteLine("Ошибка: Некорректный ID.");
              }
              break;

            case "3":
              Console.Write("Введите ID компьютера: ");
              if (!int.TryParse(Console.ReadLine(), out int pcId)) {
                Console.WriteLine("Ошибка: Некорректный ID.");
                break;
              }

              Console.Write("Введите никнейм игрока: ");
              string username = Console.ReadLine();

              Console.Write("Количество часов: ");
              if (!int.TryParse(Console.ReadLine(), out int hours)) {
                Console.WriteLine("Ошибка: Некорректное количество часов.");
                break;
              }

              GameSession session = controller.StartSession(pcId, username, hours);
              Console.WriteLine($"\n[УСПЕХ] Сессия успешно открыта!");
              Console.WriteLine($"Игрок: {session.Username} | ПК №{session.PcId}");
              Console.WriteLine($"Время старта: {session.StartTime:HH:mm:ss}");
              Console.WriteLine($"Заявлено часов: {session.HoursRequested} ч.");
              Console.WriteLine($"Итого к оплате: {session.TotalCost} руб.");
              break;

            case "4":
              Console.Write("Введите ID компьютера для завершения сессии: ");
              if (int.TryParse(Console.ReadLine(), out int closeId)) {
                controller.CloseSession(closeId);
              } else {
                Console.WriteLine("Ошибка: Некорректный ID.");
              }
              break;

            case "5":
              var activeSessions = controller.GetActiveSessions();
              Console.WriteLine("\n==== АКТИВНЫЕ ИГРОВЫЕ СЕССИИ ====");
              if (activeSessions.Count == 0) {
                Console.WriteLine("В данный момент нет активных сессий.");
              } else {
                foreach (var s in activeSessions) {
                  Console.WriteLine($"ПК №{s.PcId} | Игрок: {s.Username} | Оплачено: {s.TotalCost} руб. | До: {s.StartTime.AddHours(s.HoursRequested):HH:mm}");
                }
              }
              break;

            case "0":
              Console.WriteLine("Выход из программы. Хорошего дня!");
              return;

            default:
              Console.WriteLine("Ошибка: Неизвестная команда. Попробуйте снова.");
              break;
          }
        } catch (Exception ex) {
          Console.WriteLine($"[ОШИБКА ИСПОЛНЕНИЯ]: {ex.Message}");
        }
      }
    }
  }
}