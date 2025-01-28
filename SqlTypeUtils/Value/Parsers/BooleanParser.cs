using System.Data.SqlTypes;

namespace SqlTypeUtils.Value.Parsers;

/// <summary>
/// Статический класс для парсинга строк в логический тип.
/// </summary>
internal static class BooleanParser
{
    /// <summary>
    /// Выполняет попытку парсинга строки в указанный логический тип.
    /// </summary>
    /// <param name="targetType">Целевой логический тип.</param>
    /// <param name="value">Строковое значение, которое необходимо распарсить.</param>
    /// <param name="result">Выходной параметр, содержащий результат парсинга, если он успешен. Иначе <c>null</c>.</param>
    /// <returns><c>true</c>, если парсинг был успешен; иначе <c>false</c>.</returns>
    /// <remarks>
    /// <br/>Поддерживаемые типы:
    /// <br/>▪ <see cref="bool"/>
    /// <br/>▪ <see cref="SqlBoolean"/>
    /// </remarks>
    public static bool TryParse(Type targetType, string value, out object? result)
    {
        result = null;

        if (targetType == typeof(bool))
        {
            if (bool.TryParse(value, out bool boolResult))
            {
                result = boolResult;
                return true;
            }
        }

        if (targetType == typeof(SqlBoolean))
        {
            //try
            //{
            //    result = SqlBoolean.Parse(value);
            //    return true;
            //}
            //catch
            //{ }
            if (value == "1" || value.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                result = SqlBoolean.True;
                return true;
            }
            if (value == "0" || value.Equals("False", StringComparison.OrdinalIgnoreCase))
            {
                result = SqlBoolean.False;
                return true;
            }
        }

        return false;
    }
}
