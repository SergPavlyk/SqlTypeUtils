using SqlTypeUtils.Value.Messages;
using SqlTypeUtils.Value.Parsers;
using SqlTypeUtils.Value.Utils;
using System.Data;

namespace SqlTypeUtils.Value.Validators;

internal class SqlDateTimeValidator<T> where T : struct, IComparable<T>
{
    public SqlDbType SqlDbType { get; }                         // SQL тип данных.
    public string Description { get; }                          // Описание типа.
    public string Iso8601Format { get; }                        // Формат даты и времени в ISO 8601.
    public T MinValue { get; }                                  // Минимально допустимое значение.
    public T MaxValue { get; }                                  // Максимально допустимое значение.
    public ISqlValueValidatorMsgProvider MsgProvider { get; }   // Провайдер сообщений.    

    public SqlDateTimeValidator(SqlDbType sqlDbType, string description, string iso8601Format, T minValue, T maxValue, ISqlValueValidatorMsgProvider msgProvider)
    {
        SqlDbType = sqlDbType;
        Description = description;
        Iso8601Format = iso8601Format;
        MinValue = minValue;
        MaxValue = maxValue;
        MsgProvider = msgProvider;
    }

    public (string? outValue, string message) Validate(string valueString)
    {
        string dataTypeName = $"{MsgProvider.SqlTypeMsg} {SqlDbType}";                  // SQL SqlDbType (используется по умолчанию)

        // Сообщения
        string successMsg = MsgProvider.SuccessMsg;
        string missingValueMsg = MsgProvider.MissingValueMsg;
        string whitespaceValueMsg = MsgProvider.WhitespaceValueMsg;

        string rangeLowMsg = string.Format(MsgProvider.RangeLowMsg, StringUtils.TruncateText(valueString), MinValue, MaxValue, SqlDbType);
        string rangeHighMsg = string.Format(MsgProvider.RangeHighMsg, StringUtils.TruncateText(valueString), MinValue, MaxValue, SqlDbType);
        string invalidValueMsg = string.Format(MsgProvider.InvalidValueMsg, StringUtils.TruncateText(valueString), dataTypeName);

        // Проверяем, что значение не пустое или null.
        if (string.IsNullOrEmpty(valueString))
        {
            //string missingValueMsg = "Значение отсутствует";
            return (null, missingValueMsg);
        }

        // Проверяем, что значение не состоит только из пробелов.
        if (string.IsNullOrWhiteSpace(valueString))
        {
            //string whitespaceValueMsg = "Значение содержит только пробелы";
            return (null, whitespaceValueMsg);
        }

        if (DateTimeParser.TryParse(typeof(T), valueString, out object? parsedValue) && parsedValue is T typedValue)
        {
            // Проверка на диапазон
            if (typedValue.CompareTo(MinValue) < 0)
            {
                //string rangeLowMsg = "Значение '{typedValue}' ниже допустимого диапазона ({MinValue} - {MaxValue}) для типа {SqlDbType}";
                return (null, rangeLowMsg);
            }
            else if (typedValue.CompareTo(MaxValue) > 0)
            {
                //string rangeHighMsg = "Значение '{typedValue}' превышает допустимый диапазон ({MinValue} - {MaxValue}) для типа {SqlDbType}";
                return (null, rangeHighMsg);
            }
            else
            {
                // Успех: возвращаем отформатированное значение времени в строке по стандарту ISO 8601.
                //string successMsg = "OK";
                return (DateTimeParser.ToIso8601TimeString(typedValue, Iso8601Format), successMsg);
            }
        }
        else
        {
            // Неуспех: возвращаем null, так как не удалось преобразовать строку в ожидаемый тип.
            //string invalidValueMsg = "Значение '{valueString}' не соответствует типу {SqlDbType}";
            return (null, invalidValueMsg);
        }
    }
}
