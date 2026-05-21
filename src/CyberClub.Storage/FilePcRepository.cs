using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CyberClubManager.Core;

namespace CyberClubManager.Storage {
  // Repository for saving and loading PC data from a text file.
  public class FilePcRepository {
    private const int MinDataPartsLength = 5;
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
      List<Computer> computers = new List<Computer>();
      if (!File.Exists(_filePath)) return computers;

      string[] lines = File.ReadAllLines(_filePath);
      foreach (string line in lines) {
        if (string.IsNullOrWhiteSpace(line)) {
          continue;
        }

        string[] parts = line.Split(';');
        if (parts.Length < MinDataPartsLength) {
          continue;
        }

        int id = int.Parse(parts[0]);
        string specs = parts[1];
        double rate = double.Parse(parts[2], CultureInfo.InvariantCulture);
        bool isOccupied = bool.Parse(parts[3]);
        PcZone zone = (PcZone)Enum.Parse(typeof(PcZone), parts[4]);

        Computer pc;
        if (zone == PcZone.Vip) {
          pc = _vipFactory.CreatePc(id);
        } else {
          pc = _standardFactory.CreatePc(id);
        }

        pc.HardwareSpecs = specs;
        pc.HourlyRate = rate;
        pc.IsOccupied = isOccupied;

        if (pc is VipComputer vip && parts.Length > MinDataPartsLength) {
          vip.ExtraAmenities = parts[5];
        }

        computers.Add(pc);
      }

      return computers;
    }

    public void SaveAll(List<Computer> computers) {
      List<string> lines = new List<string>();
      foreach (Computer pc in computers) {
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