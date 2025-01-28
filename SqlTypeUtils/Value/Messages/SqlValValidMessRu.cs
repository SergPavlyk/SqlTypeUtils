namespace SqlTypeUtils.Value.Messages;

/// <summary>
/// Предоставляет сообщения валидации на русском языке для SQL типов. Реализует интерфейс <see cref="ISqlValueValidatorMsgProvider"/>.
/// </summary>
public class SqlValValidMessRu : ISqlValueValidatorMsgProvider
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SqlValValidMessRu"/>, который предоставляет сообщения валидации на русском языке для SQL типов.
    /// </summary>
    public SqlValValidMessRu() { }


    public string DotNetTypeMsg { get; set; } = ".NET";
    public string SqlTypeMsg { get; set; } = "SQL";

    public string SuccessMsg { get; set; } = "OK";                                                                                              // SqlBoolean, SqlNumeric, SqlUniqueIdentifier, SqlBinary
    public string MissingValueMsg { get; set; } = "Значение отсутствует";                                                                       // SqlBoolean, SqlNumeric, SqlUniqueIdentifier, SqlBinary
    public string WhitespaceValueMsg { get; set; } = "Значение содержит только пробелы";                                                        // SqlBoolean, SqlNumeric, SqlUniqueIdentifier, SqlBinary
    public string InvalidValueMsg { get; set; } = "Значение '{0}' не соответствует типу {1}";                                                   // SqlBoolean, SqlNumeric, SqlDateTime

    public string RangeLowMsg { get; set; } = "Значение '{0}' ниже допустимого диапазона ({1} - {2}) для типа {3}";                             // SqlNumeric, SqlDateTime
    public string RangeHighMsg { get; set; } = "Значение '{0}' превышает допустимый диапазон ({1} - {2}) для типа {3}";                         // SqlNumeric, SqlDateTime

    public string EncodingMismatchMsg { get; set; } = "Строка не соответствует ожидаемой кодировке {0}";                                        // SqlString
    public string LengthExceedMsg { get; set; } = "Длина строки превышает допустимый максимум - {0}";                                           // SqlString
    public string LengthShortMsg { get; set; } = "Длина строки меньше допустимого минимума - {0}";                                              // SqlString

    public string OddLengthMsg { get; set; } = "Значение '{0}' должно иметь чётное количество символов для типа {1}";                           // SqlBinary
    public string ByteLengthTooSmallMsg { get; set; } = "Длина массива байтов меньше допустимого минимума - {0}";                               // SqlBinary
    public string ByteLengthTooLargeMsg { get; set; } = "Длина массива байтов превышает допустимый максимум - {0}";                             // SqlBinary
    public string InvalidHexCharsMsg { get; set; } = "Значение '{0}' содержит недопустимые символы для шестнадцатеричного формата типа {1}";    // SqlBinary
}
