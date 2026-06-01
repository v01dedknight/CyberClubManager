using System;
using System.Collections.Generic;

namespace CyberClubManager.Composite
{
    /// <summary>
    /// Составной элемент Composite: игровой зал, который содержит компьютеры или другие залы.
    /// </summary>
    public class ComputerHall : IComputerComponent
    {
        private readonly string _name;
        private readonly int _childIndentStep;
        private readonly List<IComputerComponent> _componentList;

        /// <summary>
        /// Создает игровой зал.
        /// </summary>
        /// <param name="name">Название зала.</param>
        /// <param name="childIndentStep">Шаг отступа для дочерних элементов.</param>
        public ComputerHall(string name, int childIndentStep)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Hall name cannot be empty.", nameof(name));
            }

            if (childIndentStep < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(childIndentStep));
            }

            _name = name;
            _childIndentStep = childIndentStep;
            _componentList = new List<IComputerComponent>();
        }

        /// <summary>
        /// Возвращает название зала.
        /// </summary>
        /// <returns>Название зала.</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Добавляет компонент в зал.
        /// </summary>
        /// <param name="component">Компьютер, устройство или другой зал.</param>
        public void AddComponent(IComputerComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            _componentList.Add(component);
        }

        /// <summary>
        /// Возвращает количество компонентов в зале.
        /// </summary>
        /// <returns>Количество компонентов.</returns>
        public int GetComponentCount()
        {
            return _componentList.Count;
        }

        /// <summary>
        /// Формирует строки с информацией о зале и его содержимом.
        /// </summary>
        /// <param name="indentLevel">Уровень отступа.</param>
        /// <returns>Список строк.</returns>
        public List<string> BuildInfoLines(int indentLevel)
        {
            List<string> infoLineList = new List<string>();
            string indent = new string(' ', indentLevel);

            infoLineList.Add(indent + "Hall: " + _name);

            for (int componentIndex = 0; componentIndex < _componentList.Count; ++componentIndex)
            {
                List<string> componentInfoLineList = _componentList[componentIndex].BuildInfoLines(indentLevel + _childIndentStep);

                for (int lineIndex = 0; lineIndex < componentInfoLineList.Count; ++lineIndex)
                {
                    infoLineList.Add(componentInfoLineList[lineIndex]);
                }
            }

            return infoLineList;
        }
    }
}
