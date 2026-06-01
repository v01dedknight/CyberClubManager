using System;
using System.Collections.Generic;

namespace CyberClubManager.Composite
{
    /// <summary>
    /// Листовой элемент Composite: устройство компьютера.
    /// Например: видеокарта, клавиатура, монитор, мышь.
    /// </summary>
    public class Device : IComputerComponent
    {
        private readonly string _name;
        private readonly string _type;

        /// <summary>
        /// Создает устройство.
        /// </summary>
        /// <param name="name">Название устройства.</param>
        /// <param name="type">Тип устройства.</param>
        public Device(string name, string type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Device name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Device type cannot be empty.", nameof(type));
            }

            _name = name;
            _type = type;
        }

        /// <summary>
        /// Возвращает название устройства.
        /// </summary>
        /// <returns>Название устройства.</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Формирует строку с информацией об устройстве.
        /// </summary>
        /// <param name="indentLevel">Уровень отступа.</param>
        /// <returns>Список из одной строки.</returns>
        public List<string> BuildInfoLines(int indentLevel)
        {
            List<string> infoLineList = new List<string>();
            string indent = new string(' ', indentLevel);
            infoLineList.Add(indent + "Device: " + _type + " - " + _name);
            return infoLineList;
        }
    }
}
