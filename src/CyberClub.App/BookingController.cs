using System;
using System.Collections.Generic;
using CyberClubManager.Core;
using CyberClubManager.Storage;

namespace CyberClubManager.App {
  // Controller for managing bookings and game sessions.
  public class BookingController {
    private readonly FilePcRepository _repository;
    private readonly List<Computer> _computers;
    private readonly List<GameSession> _activeSessions;
    private double _totalRevenue;

    public BookingController(string filePath) {
      _repository = new FilePcRepository(filePath);
      _computers = _repository.GetAll();
      _activeSessions = new List<GameSession>();
      _totalRevenue = 0.0;

      InitializeDefaultComputersIfEmpty();
    }

    private void InitializeDefaultComputersIfEmpty() {
      if (_computers.Count == 0) {
        _computers.Add(new StandardPcFactory().CreatePc(1));
        _computers.Add(new StandardPcFactory().CreatePc(2));
        _computers.Add(new VipPcFactory().CreatePc(3));
        _repository.SaveAll(_computers);
      }
    }

    public void ListAllComputers() {
      Console.WriteLine("\n==== CURRENT HALL STATUS ====");
      foreach (Computer pc in _computers) {
        string status = pc.IsOccupied ? "[OCCUPIED]" : "[AVAILABLE]";
        Console.WriteLine($"ID: {pc.Id} | Zone: {pc.Zone,-8} | Status: {status,-11} | Rate: {pc.HourlyRate} USD/h");
      }
    }

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

    public GameSession StartSession(int pcId, string username, int hours) {
      Computer pc = _computers.Find(c => c.Id == pcId);
      if (pc != null) {
        if (pc.IsOccupied) {
          throw new InvalidOperationException($"Computer #{pcId} is already occupied by a session.");
        }

        GameSession session = new GameSession(pcId, username, hours, pc.HourlyRate);

        pc.IsOccupied = true;
        _repository.SaveAll(_computers);
        _activeSessions.Add(session);

        return session;
      }

      throw new InvalidOperationException($"Computer with ID {pcId} not found.");
    }

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
      Console.WriteLine($"Session for player {session.Username} on PC #{pcId} successfully closed. Added {session.TotalCost} USD to revenue.");
    }

    public void DisplayFinancialReport() {
      Console.WriteLine("\n==== CLUB FINANCIAL REPORT ====");
      Console.WriteLine($"Total revenue in register: {_totalRevenue} USD");
      Console.WriteLine($"Current active sessions: {_activeSessions.Count}");
    }

    public double GetTotalRevenue() => _totalRevenue;

    public IReadOnlyList<GameSession> GetActiveSessions() => _activeSessions.AsReadOnly();
  }
}