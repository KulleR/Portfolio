using System;

namespace Deville.Core.Mapper
{
    public interface IMapper
    {
        /// <summary>
        /// Выполняет отображение исходного объекта в новый объект назначения.</summary>
        /// <param name="source">Объект-источник данных для отображения</param>
        /// <param name="sourceType">Тип объекта-источника(использование)</param>
        /// <param name="destinationType">Тип объекта-назначения(создание)</param>
        /// <returns>Отображенный объект назначения</returns>
        object Map(object source, Type sourceType, Type destinationType);
    }
}
