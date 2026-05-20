using System;
using CyberClubManager.App;

namespace CyberClubManager {
  class Program {
    static void Main() {
      // Инициализируем контроллер и файл БД
      BookingController controller = new BookingController("cyberclub_db.txt");

      while (true) {
        Console.WriteLine("\n=================================");
        Console.WriteLine("    СИСТЕМА УПРАВЛЕНИЯ КЛУБОМ     ");
        Console.WriteLine("=================================");
        Console.WriteLine("1. Показать список компьютеров");
        Console.WriteLine("2. Забронировать игровое место");
        Console.WriteLine("3. Выйти из системы");
        Console.Write("\nВыберите действие (1-3): ");

        string choice = Console.ReadLine();

        if (choice == "1") {
          controller.ListAllComputers();
        } else if (choice == "2") {
          Console.Write("Введите ID компьютера для брони: ");
          if (int.TryParse(Console.ReadLine(), out int id)) {
            controller.BookComputer(id);
          } else {
            Console.WriteLine("Ошибка: Введите корректный числовой ID.");
          }
        } else if (choice == "3") {
          Console.WriteLine("Завершение работы программы...");
          break;
        } else {
          Console.WriteLine("Неверный ввод. Пожалуйста, выберите пункт от 1 до 3.");
        }
      }
    }
  }
}