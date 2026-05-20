using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CyberClubManager.Core;

namespace CyberClubManager.Storage {
  /// <summary>
  /// Репозиторий для сохранения и чтения данных ПК из текстового файла.
  /// </summary>
  public class FilePcRepository {
    private readonly string _filePath;
    private readonly StandardPcFactory _standardFactory = new StandardPcFactory();
    private readonly VipPcFactory _vipFactory = new VipPcFactory();

    public FilePcRepository(string filePath) {
      _filePath = filePath;
      if (!File.Exists(_filePath)) {
        File.WriteAllText(_filePath, string.Empty);
      }
    }

    public List<Computer> GetAll() {
      var computers = new List<Computer>();
      if (!File.Exists(_filePath)) return computers;

      var lines = File.ReadAllLines(_filePath);
      foreach (var line in lines) {
        if (string.IsNullOrWhiteSpace(line)) continue;

        var parts = line.Split(';');
        if (parts.Length < 5) continue;

        int id = int.Parse(parts[0]);
        string specs = parts[1];
        double rate = double.Parse(parts[2], CultureInfo.InvariantCulture);
        bool isOccupied = bool.Parse(parts[3]);
        PcZone zone = (PcZone)Enum.Parse(typeof(PcZone), parts[4]);

        Computer pc = zone == PcZone.Vip
            ? _vipFactory.CreatePc(id)
            : _standardFactory.CreatePc(id);

        pc.HardwareSpecs = specs;
        pc.HourlyRate = rate;
        pc.IsOccupied = isOccupied;

        if (pc is VipComputer vip && parts.Length > 5) {
          vip.ExtraAmenities = parts[5];
        }

        computers.Add(pc);
      }

      return computers;
    }

    public void SaveAll(List<Computer> computers) {
      var lines = new List<string>();
      foreach (var pc in computers) {
        string line = $"{pc.Id};{pc.HardwareSpecs};{pc.HourlyRate.ToString(CultureInfo.InvariantCulture)};{pc.IsOccupied};{pc.Zone}";
        if (pc is VipComputer vip) {
          line += $";{vip.ExtraAmenities}";
        }
        lines.Add(line);
      }
      File.WriteAllLines(_filePath, lines);
    }
  }
}