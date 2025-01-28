using System.Data.SqlTypes;
using System.Globalization;
using System.Numerics;

namespace SqlTypeUtils.Value.Parsers;

/// <summary>
/// Статический класс для парсинга строковых значений в различные числовые типы.
/// </summary>
internal static class NumericParser
{
    /// <summary>
    /// Выполняет попытку парсинга строки в указанный числовой тип.
    /// </summary>
    /// <param name="targetType">Целевой числовой тип для парсинга.
    /// <br/>Должен быть одним из поддерживаемых типов: <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="BigInteger"/>, <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>, <see cref="SqlDecimal"/>.</param>
    /// <param name="value">Строковое значение, которое необходимо распарсить.</param>
    /// <param name="result">Выходной параметр, содержащий результат парсинга, если он успешен. Иначе <c>null</c>.</param>
    /// <returns><c>true</c>, если парсинг был успешен; иначе <c>false</c>.</returns>
    /// <remarks>
    /// <br/>Поддерживаемые типы:
    /// <br/>▪ <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>
    /// <br/>▪ <see cref="long"/>, <see cref="BigInteger"/>
    /// <br/>▪ <see cref="float"/>, <see cref="double"/>
    /// <br/>▪ <see cref="decimal"/>, <see cref="SqlDecimal"/>
    /// <br/>
    /// <br/>Если строка содержит запятую, она заменяется на точку для корректного парсинга чисел.
    /// <br/>При парсинге используется инвариантная культура (<see cref="CultureInfo.InvariantCulture"/>).
    /// </remarks>
    public static bool TryParse(Type targetType, string value, out object? result)
    {
        result = null;

        // Заменяем запятую на точку для корректного парсинга чисел
        value = value.Replace(',', '.');
        // Культура, в которой разделитель для дробных чисел — точка
        var cultureInfo = CultureInfo.InvariantCulture;

        // Проверка на byte
        if (targetType == typeof(byte) && byte.TryParse(value, NumberStyles.Integer, cultureInfo, out byte byteResult))
        {
            result = byteResult;
            return true;
        }

        // Проверка на short
        if (targetType == typeof(short) && short.TryParse(value, NumberStyles.Integer, cultureInfo, out short shortResult))
        {
            result = shortResult;
            return true;
        }

        // Проверка на int
        if (targetType == typeof(int) && int.TryParse(value, NumberStyles.Integer, cultureInfo, out int intResult))
        {
            result = intResult;
            return true;
        }

        // Проверка на long
        if (targetType == typeof(long) && long.TryParse(value, NumberStyles.Integer, cultureInfo, out long longResult))
        {
            result = longResult;
            return true;
        }

        // Проверка на BigInteger
        if (targetType == typeof(BigInteger) && BigInteger.TryParse(value, NumberStyles.Integer, cultureInfo, out BigInteger bigIntegerResult))
        {
            result = bigIntegerResult;
            return true;
        }

        // Проверка на float
        if (targetType == typeof(float) && float.TryParse(value, NumberStyles.Float, cultureInfo, out float floatResult))
        {
            result = floatResult;
            return true;
        }

        // Проверка на double
        if (targetType == typeof(double) && double.TryParse(value, NumberStyles.Float, cultureInfo, out double doubleResult))
        {
            result = doubleResult;
            return true;
        }

        // Проверка на decimal
        if (targetType == typeof(decimal) && decimal.TryParse(value, NumberStyles.Float, cultureInfo, out decimal decimalResult))
        {
            result = decimalResult;
            return true;
        }

        // Проверка на SqlDecimal
        if (targetType == typeof(SqlDecimal))
        {
            try
            {
                result = SqlDecimal.Parse(value);
                return true;
            }
            catch
            { }
        }

        return false; // Если не удалось преобразовать
    }
}