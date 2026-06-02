using System;
using System.Collections.Generic;
using CyberClubManager.App;
using CyberClubManager.ComputerManagement;
using CyberClubManager.Core;

namespace CyberClubManager {
  internal class Program {
    private const string DatabaseFilePath = "cyberclub_data.txt";

    private static void Main() {
      BookingController controller = new BookingController(DatabaseFilePath);
      Console.WriteLine("Welcome to CyberClub Manager!");

      bool isRunning = true;
      while (isRunning) {
        PrintMenu();
        string input = Console.ReadLine();
        isRunning = ProcessInput(input, controller);
      }
    }

    private static void PrintMenu() {
      Console.WriteLine("\n=== MANAGEMENT MENU ===");
      Console.WriteLine("1. Show hall status and rates");
      Console.WriteLine("2. Book a PC (Quick booking)");
      Console.WriteLine("3. Open a game session (With cost calculation)");
      Console.WriteLine("4. Close a game session");
      Console.WriteLine("5. Show active sessions");
      Console.WriteLine("6. Show financial report (Revenue)");
      Console.WriteLine("7. Show computer configuration structure");
      Console.WriteLine("0. Exit");
      Console.Write("Select an action: ");
    }

    private static bool ProcessInput(string input, BookingController controller) {
      try {
        switch (input) {
          case "1":
            controller.ListAllComputers();
            break;
          case "2":
            HandleQuickBooking(controller);
            break;
          case "3":
            HandleStartSession(controller);
            break;
          case "4":
            HandleCloseSession(controller);
            break;
          case "5":
            HandleShowActiveSessions(controller);
            break;
          case "6":
            controller.DisplayFinancialReport();
            break;
          case "7":
            HandleShowComputerConfiguration();
            break;
          case "0":
            Console.WriteLine("Exiting program. Have a good day!");
            return false;
          default:
            Console.WriteLine("Error: Unknown command. Please try again.");
            break;
        }
      } catch (Exception ex) {
        Console.WriteLine("[EXECUTION ERROR]: " + ex.Message);
      }

      return true;
    }

    private static void HandleQuickBooking(BookingController controller) {
      Console.Write("Enter computer ID for booking: ");

      if (int.TryParse(Console.ReadLine(), out int bookId)) {
        controller.BookComputer(bookId);
      } else {
        Console.WriteLine("Error: Invalid ID format.");
      }
    }

    private static void HandleStartSession(BookingController controller) {
      Console.Write("Enter computer ID: ");
      if (!int.TryParse(Console.ReadLine(), out int pcId)) {
        Console.WriteLine("Error: Invalid ID format.");
        return;
      }

      Console.Write("Enter player username: ");
      string username = Console.ReadLine();

      Console.Write("Number of hours: ");
      if (!int.TryParse(Console.ReadLine(), out int hours)) {
        Console.WriteLine("Error: Invalid hours format.");
        return;
      }

      GameSession session = controller.StartSession(pcId, username, hours);
      Console.WriteLine("\n[SUCCESS] Session successfully opened!");
      Console.WriteLine("Player: " + session.Username + " | PC #" + session.PcId);
      Console.WriteLine("Total to pay: " + session.TotalCost + " USD.");
    }

    private static void HandleCloseSession(BookingController controller) {
      Console.Write("Enter computer ID to close session: ");

      if (int.TryParse(Console.ReadLine(), out int closeId)) {
        controller.CloseSession(closeId);
      } else {
        Console.WriteLine("Error: Invalid ID format.");
      }
    }

    private static void HandleShowActiveSessions(BookingController controller) {
      IReadOnlyList<GameSession> activeSessions = controller.GetActiveSessions();
      Console.WriteLine("\n==== ACTIVE GAME SESSIONS ====");

      if (activeSessions.Count == 0) {
        Console.WriteLine("There are no active sessions at the moment.");
        return;
      }

      foreach (GameSession session in activeSessions) {
        Console.WriteLine("PC #" + session.PcId + " | Player: " + session.Username + " | Paid: " + session.TotalCost + " USD.");
      }
    }

    private static void HandleShowComputerConfiguration() {
      ComputerConfigurationManager manager = new ComputerConfigurationManager();
      manager.BuildDefaultClubConfiguration();

      List<string> configurationLines = manager.GetClubConfigurationLines();

      Console.WriteLine("\n==== COMPUTER CONFIGURATION STRUCTURE ====");

      foreach (string line in configurationLines) {
        Console.WriteLine(line);
      }
    }
  }
}