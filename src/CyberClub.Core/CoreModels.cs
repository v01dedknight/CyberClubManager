using System;

namespace CyberClubManager.Core {
  /// <summary>
  /// Зоны компьютерного клуба.
  /// </summary>
  public enum PcZone {
    Standard,
    Vip
  }

  /// <summary>
  /// Абстрактный класс игрового компьютера.
  /// </summary>
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

  /// <summary>
  /// Обычное игровое место.
  /// </summary>
  public class StandardComputer : Computer {
    public override PcZone Zone => PcZone.Standard;

    public StandardComputer(int id)
        : base(id, "RTX 4060, Core i5, 16GB RAM", 150.0) {
    }
  }

  /// <summary>
  /// VIP игровое место повышенного комфорта.
  /// </summary>
  public class VipComputer : Computer {
    public override PcZone Zone => PcZone.Vip;
    public string ExtraAmenities { get; set; }

    public VipComputer(int id)
        : base(id, "RTX 4090, Core i9, 32GB RAM", 300.0) {
      ExtraAmenities = "Mechanical Keyboard, Gaming Chair, Energy Drink Free";
    }
  }

  /// <summary>
  /// Абстрактная фабрика (Factory Method паттерн).
  /// </summary>
  public abstract class PcFactory {
    public abstract Computer CreatePc(int id);
  }

  public class StandardPcFactory : PcFactory {
    public override Computer CreatePc(int id) => new StandardComputer(id);
  }

  public class VipPcFactory : PcFactory {
    public override Computer CreatePc(int id) => new VipComputer(id);
  }
}