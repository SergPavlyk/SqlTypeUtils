using System.Globalization;

namespace SqlTypeUtils.Value.Parsers;

/// <summary>
/// Статический класс для парсинга строковых значений в различные временные типы.
/// </summary>
internal class DateTimeParser
{
    /// <summary>
    /// Выполняет попытку парсинга строки в указанный временной тип.
    /// </summary>
    /// <param name="targetType">Целевой временной тип для парсинга. Должен быть одним из поддерживаемых типов.</param>
    /// <param name="value">Строковое значение, которое необходимо распарсить.</param>
    /// <param name="result">Выходной параметр, содержащий результат парсинга, если он успешен. Иначе <c>null</c>.</param>
    /// <returns><c>true</c>, если парсинг был успешен; иначе <c>false</c>.</returns>
    /// <remarks>
    /// <br/>Поддерживаемые типы:
    /// <br/>▪ <see cref="TimeOnly"/>
    /// <br/>▪ <see cref="DateOnly"/>
    /// <br/>▪ <see cref="TimeSpan"/>
    /// <br/>▪ <see cref="DateTime"/>
    /// <br/>▪ <see cref="DateTimeOffset"/>
    /// <br/>
    /// <br/>При парсинге используются инвариантные настройки культуры (<see cref="CultureInfo.InvariantCulture"/>).
    /// </remarks>
    public static bool TryParse(Type targetType, string value, out object? result)
    {
        result = null;
        var cultureInfo = CultureInfo.InvariantCulture;

        if (targetType == typeof(TimeOnly))             // Пример: "03:45:30" (ЧЧ:ММ:СС)
        {
            if (TimeOnly.TryParse(value, cultureInfo, DateTimeStyles.None, out TimeOnly timeOnlyResult))
            {
                result = timeOnlyResult;
                return true;
            }
        }
        else if (targetType == typeof(DateOnly))        // Пример: "2024-10-17" (ГГГГ-ММ-ДД)
        {
            if (DateOnly.TryParse(value, cultureInfo, DateTimeStyles.None, out DateOnly dateOnlyResult))
            {
                result = dateOnlyResult;
                return true;
            }
        }
        else if (targetType == typeof(TimeSpan))        // Пример: "03:45:30.1234567" (ЧЧ:ММ:СС.миллисекунды)
        {
            if (TimeSpan.TryParse(value, cultureInfo, out TimeSpan timeSpanResult))
            {
                result = timeSpanResult;
                return true;
            }
        }
        else if (targetType == typeof(DateTime))        // Пример: "2024-10-17T13:45:00.1234567" (ГГГГ-ММ-ДДTЧЧ:ММ:СС.миллисекунды)
        {
            if (DateTime.TryParse(value, cultureInfo, DateTimeStyles.None, out DateTime dateTimeResult))
            {
                result = dateTimeResult;
                return true;
            }
        }
        else if (targetType == typeof(DateTimeOffset))  // Пример: "2024-10-17T13:45:00.1234567+03:00" (ГГГГ-ММ-ДДTЧЧ:ММ:СС.миллисекунды+/-ЧЧ:ММ)
        {
            if (DateTimeOffset.TryParse(value, cultureInfo, DateTimeStyles.None, out DateTimeOffset dateTimeOffsetResult))
            {
                result = dateTimeOffsetResult;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Форматирует типы, связанные с временем, в строку по стандарту ISO 8601.
    /// </summary>
    /// <param name="parsedValue">Значение, которое требуется форматировать. <br/>Поддерживаемые типы:
    ///    <br/> ▪ DateTime
    ///    <br/> ▪ DateTimeOffset
    ///    <br/> ▪ TimeSpan
    ///    <br/> ▪ DateOnly
    /// </param>
    /// <param name="iso8601Format">Строка формата, используемая для форматирования значения.</param>
    /// <returns>
    /// Возвращает строку с отформатированным значением, если тип поддерживается.
    /// <br/> В противном случае возвращает строку "Unsupported value type.".
    /// </returns>
    public static string ToIso8601TimeString(object parsedValue, string iso8601Format)
    {
        // Обрабатываем разные типы значений
        if (parsedValue is DateTime dateTimeValue)
        {
            return dateTimeValue.ToString(iso8601Format);
        }
        else if (parsedValue is DateTimeOffset dateTimeOffsetValue)
        {
            return dateTimeOffsetValue.ToString(iso8601Format);
        }
        else if (parsedValue is TimeSpan timeSpanValue)
        {
            return timeSpanValue.ToString(iso8601Format);
        }
        else if (parsedValue is DateOnly dateOnlyValue)
        {
            return dateOnlyValue.ToString(iso8601Format);
        }
        return "Unsupported value type.";
    }
}
