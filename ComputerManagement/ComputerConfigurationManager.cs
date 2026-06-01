using System.Collections.Generic;
using CyberClubManager.Composite;

namespace CyberClubManager.ComputerManagement
{
    /// <summary>
    /// Менеджер конфигураций ПК.
    /// Отвечает за создание залов VIP/Общий, добавление ПК, устройств и характеристик.
    /// </summary>
    public class ComputerConfigurationManager
    {
        private const int RootIndentLevel = 0;
        private const int ChildIndentStep = 2;

        private readonly ComputerHall _clubRoot;

        /// <summary>
        /// Создает менеджер конфигураций.
        /// </summary>
        public ComputerConfigurationManager()
        {
            _clubRoot = new ComputerHall("Cyber Club", ChildIndentStep);
        }

        /// <summary>
        /// Создает новый игровой зал.
        /// </summary>
        /// <param name="hallName">Название зала.</param>
        /// <returns>Игровой зал.</returns>
        public ComputerHall CreateHall(string hallName)
        {
            return new ComputerHall(hallName, ChildIndentStep);
        }

        /// <summary>
        /// Создает новый компьютер с характеристиками.
        /// </summary>
        /// <param name="computerName">Название компьютера.</param>
        /// <param name="systemSpecs">Системные характеристики.</param>
        /// <returns>Компьютер.</returns>
        public Computer CreateComputer(string computerName, string systemSpecs)
        {
            return new Computer(computerName, systemSpecs, ChildIndentStep);
        }

        /// <summary>
        /// Создает новое устройство.
        /// </summary>
        /// <param name="deviceName">Название устройства.</param>
        /// <param name="deviceType">Тип устройства.</param>
        /// <returns>Устройство.</returns>
        public Device CreateDevice(string deviceName, string deviceType)
        {
            return new Device(deviceName, deviceType);
        }

        /// <summary>
        /// Добавляет компонент в корень клуба.
        /// </summary>
        /// <param name="component">Компонент клуба.</param>
        public void AddToClub(IComputerComponent component)
        {
            _clubRoot.AddComponent(component);
        }

        /// <summary>
        /// Создает демонстрационную структуру клуба: VIP и Общий зал.
        /// </summary>
        public void BuildDefaultClubConfiguration()
        {
            ComputerHall vipHall = CreateHall("VIP hall");
            ComputerHall commonHall = CreateHall("Common hall");

            Computer vipComputer = CreateComputer("VIP-PC-01", "RTX 4070, Core i7, 32GB RAM, SSD 1TB");
            vipComputer.AddComponent(CreateDevice("RTX 4070", "Video card"));
            vipComputer.AddComponent(CreateDevice("HyperX Cloud II", "Headset"));
            vipComputer.AddComponent(CreateDevice("Logitech G Pro", "Mouse"));
            vipComputer.AddComponent(CreateDevice("Mechanical Keyboard", "Keyboard"));

            Computer commonComputer = CreateComputer("COMMON-PC-01", "GTX 1660, Core i5, 16GB RAM, SSD 512GB");
            commonComputer.AddComponent(CreateDevice("GTX 1660", "Video card"));
            commonComputer.AddComponent(CreateDevice("A4Tech Bloody", "Keyboard"));
            commonComputer.AddComponent(CreateDevice("Samsung 24 inch", "Monitor"));
            commonComputer.AddComponent(CreateDevice("Logitech G102", "Mouse"));

            vipHall.AddComponent(vipComputer);
            commonHall.AddComponent(commonComputer);

            AddToClub(vipHall);
            AddToClub(commonHall);
        }

        /// <summary>
        /// Возвращает строки с полной структурой компьютерного клуба.
        /// </summary>
        /// <returns>Список строк для вывода.</returns>
        public List<string> GetClubConfigurationLines()
        {
            return _clubRoot.BuildInfoLines(RootIndentLevel);
        }
    }
}
