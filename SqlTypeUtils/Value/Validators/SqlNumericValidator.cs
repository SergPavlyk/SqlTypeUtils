using SqlTypeUtils.Value.Messages;
using SqlTypeUtils.Value.Parsers;
using SqlTypeUtils.Value.Utils;
using System.Data;

namespace SqlTypeUtils.Value.Validators;

internal class SqlNumericValidator<T> where T : struct, IComparable
{
    public SqlDbType SqlDbType { get; }                         // SQL тип данных.
    public string Description { get; }                          // Описание типа.
    public T MinValue { get; }                                  // Минимально допустимое значение.
    public T MaxValue { get; }                                  // Максимально допустимое значение.
    public ISqlValueValidatorMsgProvider MsgProvider { get; }   // Провайдер сообщений.    

    public SqlNumericValidator(SqlDbType sqlDbType, string description, T minValue, T maxValue, ISqlValueValidatorMsgProvider msgProvider)
    {
        SqlDbType = sqlDbType;
        Description = description;
        MinValue = minValue;
        MaxValue = maxValue;
        MsgProvider = msgProvider;
    }

    public (string? outValue, string message) Validate(string valueString)
    {
        string dataTypeName = $"{MsgProvider.SqlTypeMsg} {SqlDbType}";                  // SQL SqlDbType (используется по умолчанию)
        if (MaxValue.ToString() == decimal.MaxValue.ToString())
        {
            dataTypeName = $"{MsgProvider.DotNetTypeMsg} {typeof(decimal).Name}";       // .NET Decimal (применяется только для .NET Decimal)
        }

        // Сообщения
        string successMsg = MsgProvider.SuccessMsg;
        string missingValueMsg = MsgProvider.MissingValueMsg;
        string whitespaceValueMsg = MsgProvider.WhitespaceValueMsg;

        string rangeLowMsg = string.Format(MsgProvider.RangeLowMsg, StringUtils.TruncateText(valueString), MinValue, MaxValue, dataTypeName);
        string rangeHighMsg = string.Format(MsgProvider.RangeHighMsg, StringUtils.TruncateText(valueString), MinValue, MaxValue, dataTypeName);
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

        // Убираем все пробелы и нормализуем строку, представляющую число.
        valueString = valueString.Replace(" ", "");

        // -----------------------------------------------------------------------
        // Проверка значения для типа SqlDecimal.
        // Этот блок кода выполняет проверку того, что значение соответствует формату числа, 
        // а также валидирует его на соответствие допустимому диапазону для типа SqlDecimal и возвращает детальное сообщение.
        // 
        // Важно: Если данный блок кода будет закомментирован или удалён:
        // 1. Когда значение не соответствует формату числа, будет возвращено сообщение:                    "Значение ({0}) не соответствует типу {1}".
        // 2. Когда значение выходит за пределы допустимого диапазона, будет возвращено то же сообщение:    "Значение ({0}) не соответствует типу {1}".
        //if (typeof(T) == typeof(SqlDecimal) && MinValue.ToString() == SqlDecimal.MinValue.ToString() && MaxValue.ToString() == SqlDecimal.MaxValue.ToString())
        //{
        //    try
        //    {
        //        bool isBelowMinValue = NumericStringUtils.IsComparisonValid(valueString, ComparisonType.LessThan, MinValue.ToString()!);
        //        //string rangeLowMsg = "Значение '{typedValue}' ниже допустимого диапазона ({MinValue} - {MaxValue}) для типа {SqlDbType}";
        //        if (isBelowMinValue) return (null, rangeLowMsg);

        //        bool isAboveMaxValue = NumericStringUtils.IsComparisonValid(valueString, ComparisonType.GreaterThan, MaxValue.ToString()!);
        //        //string rangeHighMsg = "Значение '{typedValue}' превышает допустимый диапазон ({MinValue} - {MaxValue}) для типа {SqlDbType}";
        //        if (isAboveMaxValue) return (null, rangeHighMsg);
        //    }
        //    catch
        //    {
        //        //string invalidValueMsg = "Значение ({0}) не соответствует типу {1}";
        //        return (null, invalidValueMsg);
        //    }

        //    // string successMsg = "OK";
        //    return (valueString.Replace("+", "").Replace(",","."), successMsg);
        //}
        // -----------------------------------------------------------------------


        // Используем NumericParser для попытки парсинга
        if (NumericParser.TryParse(typeof(T), valueString, out object? parsedValue) && parsedValue is T typedValue)
        {
            // Проверка на диапазон
            if (Comparer<T>.Default.Compare(typedValue, MinValue) < 0)
            {
                //string rangeLowMsg = "Значение '{typedValue}' ниже допустимого диапазона ({MinValue} - {MaxValue}) для типа {SqlDbType}";
                return (null, rangeLowMsg);
            }

            if (Comparer<T>.Default.Compare(typedValue, MaxValue) > 0)
            {
                //string rangeHighMsg = "Значение '{typedValue}' превышает допустимый диапазон ({MinValue} - {MaxValue}) для типа {SqlDbType}";
                return (null, rangeHighMsg);
            }

            // Успех: возвращаем значение числа, представленное строкой.
            // string successMsg = "OK";
            return (typedValue.ToString(), successMsg);
        }

        //string invalidValueMsg = "Значение ({0}) не соответствует типу {1}";
        return (null, invalidValueMsg);
    }
}
