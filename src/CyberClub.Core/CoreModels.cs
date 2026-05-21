using System;

namespace CyberClubManager.Core {
  // PC zones in the club.
  public enum PcZone {
    Standard,
    Vip
  }

  // Abstract class representing a gaming computer.
  public abstract class Computer {
    public int Id { get; set; }
    public string HardwareSpecs { get; set; }
    public double HourlyRate { get; set; }
    public bool IsOccupied { get; set; }
    public abstract PcZone Zone { get; }

    protected Computer(int id, string hardwareSpecs, double hourlyRate) {
      Id = id;
      HardwareSpecs = hardwareSpecs;
      HourlyRate = hourlyRate;
      IsOccupied = false;
    }
  }

  // Standard gaming place.
  public class StandardComputer : Computer {
    private const double StandardHourlyRate = 150.0;
    private const string StandardSpecs = "RTX 4060, Core i5, 16GB RAM";

    public override PcZone Zone {
      get {
        return PcZone.Standard;
      }
    }

    public StandardComputer(int id)
        : base(id, StandardSpecs, StandardHourlyRate) {
    }
  }

  // VIP gaming place with increased comfort.
  public class VipComputer : Computer {
    private const double VipHourlyRate = 300.0;
    private const string VipSpecs = "RTX 4090, Core i9, 32GB RAM";
    private const string DefaultAmenities = "Mechanical Keyboard, Gaming Chair, Energy Drink Free";

    public override PcZone Zone {
      get {
        return PcZone.Vip;
      }
    }

    public string ExtraAmenities { get; set; }

    public VipComputer(int id)
        : base(id, VipSpecs, VipHourlyRate) {
      ExtraAmenities = DefaultAmenities;
    }
  }

  // User gaming session.
  public class GameSession {
    public int PcId { get; set; }
    public string Username { get; set; }
    public DateTime StartTime { get; set; }
    public int HoursRequested { get; set; }
    public double TotalCost { get; set; }
    public bool IsActive { get; set; }

    public GameSession(int pcId, string username, int hoursRequested, double hourlyRate) {
      PcId = pcId;
      Username = username ?? throw new ArgumentNullException(nameof(username));
      if (hoursRequested > 0) {
        HoursRequested = hoursRequested;
      } else {
        throw new ArgumentException("Session time must be greater than zero.");
      }
      StartTime = DateTime.Now;
      TotalCost = hoursRequested * hourlyRate;
      IsActive = true;
    }

    public void EndSession() {
      IsActive = false;
    }
  }

  // Abstract factory pattern base.
  public abstract class PcFactory {
    public abstract Computer CreatePc(int id);
  }

  public class StandardPcFactory : PcFactory {
    public override Computer CreatePc(int id) {
      return new StandardComputer(id);
    }
  }

  public class VipPcFactory : PcFactory {
    public override Computer CreatePc(int id) {
      return new VipComputer(id);
    }
  }
}