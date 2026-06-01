using System;
using System.Collections.Generic;

namespace CyberClubManager.Composite
{
    /// <summary>
    /// Составной элемент Composite: компьютер с устройствами и характеристиками.
    /// </summary>
    public class Computer : IComputerComponent
    {
        private readonly string _name;
        private readonly string _systemSpecs;
        private readonly int _childIndentStep;
        private readonly List<IComputerComponent> _deviceList;

        /// <summary>
        /// Создает компьютер.
        /// </summary>
        /// <param name="name">Название компьютера.</param>
        /// <param name="systemSpecs">Системные характеристики.</param>
        /// <param name="childIndentStep">Шаг отступа для дочерних элементов.</param>
        public Computer(string name, string systemSpecs, int childIndentStep)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Computer name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(systemSpecs))
            {
                throw new ArgumentException("System specs cannot be empty.", nameof(systemSpecs));
            }

            if (childIndentStep < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(childIndentStep));
            }

            _name = name;
            _systemSpecs = systemSpecs;
            _childIndentStep = childIndentStep;
            _deviceList = new List<IComputerComponent>();
        }

        /// <summary>
        /// Возвращает название компьютера.
        /// </summary>
        /// <returns>Название компьютера.</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Возвращает системные характеристики компьютера.
        /// </summary>
        /// <returns>Строка характеристик.</returns>
        public string GetSystemSpecs()
        {
            return _systemSpecs;
        }

        /// <summary>
        /// Добавляет устройство к компьютеру.
        /// </summary>
        /// <param name="component">Устройство или другой компонент.</param>
        public void AddComponent(IComputerComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            _deviceList.Add(component);
        }

        /// <summary>
        /// Возвращает количество подключенных компонентов.
        /// </summary>
        /// <returns>Количество компонентов.</returns>
        public int GetComponentCount()
        {
            return _deviceList.Count;
        }

        /// <summary>
        /// Формирует строки с информацией о компьютере и его устройствах.
        /// </summary>
        /// <param name="indentLevel">Уровень отступа.</param>
        /// <returns>Список строк.</returns>
        public List<string> BuildInfoLines(int indentLevel)
        {
            List<string> infoLineList = new List<string>();
            string indent = new string(' ', indentLevel);

            infoLineList.Add(indent + "Computer: " + _name);
            infoLineList.Add(indent + "Specs: " + _systemSpecs);

            for (int deviceIndex = 0; deviceIndex < _deviceList.Count; ++deviceIndex)
            {
                List<string> deviceInfoLineList = _deviceList[deviceIndex].BuildInfoLines(indentLevel + _childIndentStep);

                for (int lineIndex = 0; lineIndex < deviceInfoLineList.Count; ++lineIndex)
                {
                    infoLineList.Add(deviceInfoLineList[lineIndex]);
                }
            }

            return infoLineList;
        }
    }
}
