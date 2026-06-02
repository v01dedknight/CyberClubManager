using System.Collections.Generic;

namespace CyberClubManager.Composite
{
    /// <summary>
    /// Общий интерфейс для элементов структуры компьютерного клуба.
    /// Используется в паттерне Composite.
    /// </summary>
    public interface IComputerComponent
    {
        /// <summary>
        /// Возвращает имя компонента.
        /// </summary>
        /// <returns>Имя компонента.</returns>
        string GetName();

        /// <summary>
        /// Формирует строки с информацией о компоненте.
        /// </summary>
        /// <param name="indentLevel">Уровень отступа.</param>
        /// <returns>Список строк для вывода.</returns>
        List<string> BuildInfoLines(int indentLevel);
    }
}
