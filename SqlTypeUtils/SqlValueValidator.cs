using SqlTypeUtils.Value.Enums;
using SqlTypeUtils.Value.Messages;
using SqlTypeUtils.Value.Validators;
using System.Data;
using System.Data.SqlTypes;
using System.Numerics;
using System.Text;

namespace SqlTypeUtils;

/// <summary>
/// Класс, предоставляющий функциональность для проверки значений различных типов данных SQL.
/// </summary>
public class SqlValueValidator
{
    private ISqlValueValidatorMsgProvider _msgProvider;

    private SqlBooleanValidator<bool> _boolVal = null!;                             // 1
    private SqlBooleanValidator<SqlBoolean> _boolSqlVal = null!;                    // 1.1

    private SqlNumericValidator<BigInteger> _tinyIntVal = null!;                    // 2
    private SqlNumericValidator<BigInteger> _smallIntVal = null!;                   // 3
    private SqlNumericValidator<BigInteger> _intVal = null!;                        // 4
    private SqlNumericValidator<BigInteger> _bigIntVal = null!;                     // 5

    private SqlNumericValidator<decimal> _smallMoneyVal = null!;                    // 6
    private SqlNumericValidator<decimal> _moneyVal = null!;                         // 7

    private SqlNumericValidator<SqlDecimal> _decimalVal = null!;                    // 8
    private SqlNumericValidator<SqlDecimal> _decimalSqlVal = null!;                 // 8.1

    private SqlNumericValidator<double> _floatVal = null!;                          // 9
    private SqlNumericValidator<float> _realVal = null!;                            // 10

    private SqlDateTimeValidator<DateTime> _smallDateTimeVal = null!;               // 11
    private SqlDateTimeValidator<DateOnly> _dateVal = null!;                        // 12
    private SqlDateTimeValidator<DateTime> _dateTimeVal = null!;                    // 13
    private SqlDateTimeValidator<DateTime> _dateTime2Val = null!;                   // 14
    private SqlDateTimeValidator<TimeSpan> _timeVal = null!;                        // 15
    private SqlDateTimeValidator<DateTimeOffset> _dateTimeOffsetVal = null!;        // 16

    private SqlStringValidator _charVal = null!;                                    // 17
    private SqlStringValidator _varCharVal = null!;                                 // 18
    private SqlStringValidator _nCharVal = null!;                                   // 19
    private SqlStringValidator _nVarCharVal = null!;                                // 20

    private SqlUniqueIdentifierValidator _uniqueIdentifierVal = null!;              // 21

    private SqlBinaryValidator _binaryVal = null!;                                  // 22
    private SqlBinaryValidator _varBinaryVal = null!;                               // 23
    private SqlBinaryValidator _timestampVal = null!;                               // 24

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SqlValueValidator"/> с использованием стандартных сообщений валидации на английском языке.
    /// </summary>
    public SqlValueValidator()
    {
        _msgProvider = new SqlValValidMessEn();
        Init();
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SqlValueValidator"/> с использованием пользовательского провайдера сообщений валидации.
    /// </summary>
    /// <param name="msgProvider">Экземпляр <see cref="ISqlValueValidatorMsgProvider"/>, который предоставляет пользовательские сообщения валидации.</param>
    public SqlValueValidator(ISqlValueValidatorMsgProvider msgProvider)
    {
        _msgProvider = msgProvider;
        Init();
    }


    /// <summary>
    /// Инициализирует валидаторы для различных типов данных SQL.
    /// </summary>
    private void Init()
    {
        // SqlBoolean
        _boolVal = _boolVal = new SqlBooleanValidator<bool>(SqlDbType.Bit, "TRUE или FALSE", _msgProvider);
        _boolSqlVal = new SqlBooleanValidator<SqlBoolean>(SqlDbType.Bit, "TRUE или FALSE, '1' или '0'", _msgProvider);
        // ----------------------------------------------------------------------------------------

        // SqlNumeric
        //  -> Целочисленные типы.
        _tinyIntVal = new SqlNumericValidator<BigInteger>(SqlDbType.TinyInt, "8-битное целое число без знака (0-255)", 0, 255, _msgProvider);
        _smallIntVal = new SqlNumericValidator<BigInteger>(SqlDbType.SmallInt, "16-битное знаковое целое число (-32,768 до 32,767)", -32768, 32767, _msgProvider);
        _intVal = new SqlNumericValidator<BigInteger>(SqlDbType.Int, "32-битное знаковое целое число (-2,147,483,648 до 2,147,483,647)", -2147483648, 2147483647, _msgProvider);
        _bigIntVal = new SqlNumericValidator<BigInteger>(SqlDbType.BigInt, "64-битное знаковое целое число (-9,223,372,036,854,775,808 до 9,223,372,036,854,775,807)", -9223372036854775808, 9223372036854775807, _msgProvider);

        //  -> Денежные типы.
        _smallMoneyVal = new SqlNumericValidator<decimal>(SqlDbType.SmallMoney, "Деньги (-214,748.3648 до +214,748.3647, точность 0.0001)", -214748.3648m, 214748.3647m, _msgProvider);
        _moneyVal = new SqlNumericValidator<decimal>(SqlDbType.Money, "Деньги (-9,223,372,036,854,77.5808 до +9,223,372,036,854,77.5807, точность 0.0001)", -922337203685477.5808m, 922337203685477.5807m, _msgProvider);

        //  -> Десятичные типы.
        _decimalVal = new SqlNumericValidator<SqlDecimal>(SqlDbType.Decimal, "Число с фиксированной точностью и масштабом (-79,228,162,514,264,337,593,543,950,335 до 79,228,162,514,264,337,593,543,950,335, точность до 28-29 значащих цифр)", decimal.MinValue, decimal.MaxValue, _msgProvider);
        _decimalSqlVal = new SqlNumericValidator<SqlDecimal>(SqlDbType.Decimal, "Число с фиксированной точностью и масштабом (от -10^38 + 1 до 10^38 - 1, с точностью до 38 значащих цифр и масштабом от 0 до 38)", SqlDecimal.MinValue, SqlDecimal.MaxValue, _msgProvider);

        //  -> Типы с плавающей запятой.
        _floatVal = new SqlNumericValidator<double>(SqlDbType.Float, "Число с плавающей запятой (-1.7976931348623157E+308 до 1.7976931348623157E+308)", -1.7976931348623157E+308, 1.7976931348623157E+308, _msgProvider);
        _realVal = new SqlNumericValidator<float>(SqlDbType.Real, "Число с плавающей запятой меньшей точности (-3.4028235E+38f до 3.4028235E+38f)", -3.4028235E+38f, 3.4028235E+38f, _msgProvider);
        // ----------------------------------------------------------------------------------------

        // SqlDateTime
        _smallDateTimeVal = new SqlDateTimeValidator<DateTime>(SqlDbType.SmallDateTime, "Дата и время с точностью до минуты (1900-2079)", "yyyy-MM-dd HH:mm:ss", new DateTime(1900, 1, 1, 0, 0, 0), new DateTime(2079, 6, 6, 23, 59, 59), _msgProvider);
        _dateVal = new SqlDateTimeValidator<DateOnly>(SqlDbType.Date, "Только дата (0001-9999)", "yyyy-MM-dd", new DateOnly(1, 1, 1), new DateOnly(9999, 12, 31), _msgProvider);
        _dateTimeVal = new SqlDateTimeValidator<DateTime>(SqlDbType.DateTime, "Дата и время (1753-9999) с точностью до 3 миллисекунд", "yyyy-MM-dd HH:mm:ss.fff", new DateTime(1753, 1, 1, 0, 0, 0, 0), new DateTime(9999, 12, 31, 23, 59, 59, 997), _msgProvider);
        _dateTime2Val = new SqlDateTimeValidator<DateTime>(SqlDbType.DateTime2, "Дата и время (0001-9999) с точностью до 100 наносекунд", "yyyy-MM-dd HH:mm:ss.fffffff", new DateTime(1, 1, 1, 0, 0, 0, 0), new DateTime(9999, 12, 31, 23, 59, 59, 999).AddTicks(9999), _msgProvider);
        _timeVal = new SqlDateTimeValidator<TimeSpan>(SqlDbType.Time, "Время без даты (00:00:00 до 23:59:59) с точностью до 100 наносекунд)", @"hh\:mm\:ss\.fffffff", new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(23, 59, 59, 59, 999), _msgProvider);
        _dateTimeOffsetVal = new SqlDateTimeValidator<DateTimeOffset>(SqlDbType.DateTimeOffset, "Дата и время с временной зоной (0001-9999) с точностью до 100 наносекунд", "yyyy-MM-dd HH:mm:ss.fffffffK", DateTimeOffset.MinValue, DateTimeOffset.MaxValue, _msgProvider);
        // ----------------------------------------------------------------------------------------

        // SqlString
        _charVal = new SqlStringValidator(SqlDbType.Char, "Строка фиксированной длины (1-8000 символов, ASCII)", 0, 8000, 8000, Encoding.ASCII, _msgProvider);
        _varCharVal = new SqlStringValidator(SqlDbType.VarChar, "Строка переменной длины (1-8000 байт, ASCII)", 0, 8000, int.MaxValue, Encoding.ASCII, _msgProvider);
        _nCharVal = new SqlStringValidator(SqlDbType.NChar, "Строка фиксированной длины, поддерживающая Юникод (1-4000 символов)", 0, 4000, 4000, Encoding.Unicode, _msgProvider);
        _nVarCharVal = new SqlStringValidator(SqlDbType.NVarChar, "Строка переменной длины, поддерживающая Юникод (1-4000 символов)", 0, 4000, int.MaxValue / 2, Encoding.Unicode, _msgProvider);
        // ----------------------------------------------------------------------------------------

        // SqlUniqueIdentifier
        _uniqueIdentifierVal = new SqlUniqueIdentifierValidator(SqlDbType.UniqueIdentifier, "Guid. Глобальный уникальный идентификатор (GUID)", _msgProvider);
        // ----------------------------------------------------------------------------------------
        
        // SqlBinary
        _binaryVal = new SqlBinaryValidator(SqlDbType.Binary, "Фиксированный размер бинарных данных (1-8000 байт).", 0, 8000, 8000, _msgProvider);
        _varBinaryVal = new SqlBinaryValidator(SqlDbType.VarBinary, "Двоичные данные переменной длины (1-8000 байт).", 0, 8000, int.MaxValue, _msgProvider);
        _timestampVal = new SqlBinaryValidator(SqlDbType.Timestamp, "Уникальная версия строки в таблице (8 байт).", 8, 8, 8, _msgProvider);
        // ----------------------------------------------------------------------------------------
    }

    /// <summary>
    /// Проверяет, можно ли преобразовать строку в заданный тип данных SQL.
    /// </summary>
    /// <param name="sqlDataType">Тип данных SQL, для которого проверяется значение.</param>
    /// <param name="valueString">Строка, которую нужно проверить.</param>
    /// <param name="size">Необязательный параметр, указывающий длину значения для типов данных VarChar, NVarChar или VarBinary (по умолчанию равен 0).</param>
    /// <param name="useExtended">Необязательный параметр, указывающий, какой валидатор использовать для некоторых типов данных, таких как Boolean и Decimal (по умолчанию равен false).
    /// <br/>
    /// <br/>Когда <c>false</c>, используется основной валидатор для <c>Boolean</c> или <c>Decimal</c>.
    /// <br/>Когда <c>true</c>, используется альтернативный валидатор для более широкого диапазона значений для <c>SqlBoolean</c> или <c>SqlDecimal</c>.</param>
    /// <returns>
    /// Кортеж с двумя значениями:
    /// <br/>▪ <c>outValue</c> - Отформатированое значение, если строку можно привести к указанному типу, или null, если преобразование невозможно.
    /// <br/>▪ <c>message</c> - Сообщение о результате проверки.
    /// </returns>
    public (string? outValue, string message) Validate(SqlDataType sqlDataType, string valueString, int size = 0, bool useExtended = false)
    {
        if (sqlDataType == SqlDataType.Bit && !useExtended) return _boolVal.Validate(valueString);              // 1
        if (sqlDataType == SqlDataType.Bit && useExtended) return _boolSqlVal.Validate(valueString);            // 1.1

        if (sqlDataType == SqlDataType.TinyInt) return _tinyIntVal.Validate(valueString);                       // 2
        if (sqlDataType == SqlDataType.SmallInt) return _smallIntVal.Validate(valueString);                     // 3
        if (sqlDataType == SqlDataType.Int) return _intVal.Validate(valueString);                               // 4
        if (sqlDataType == SqlDataType.BigInt) return _bigIntVal.Validate(valueString);                         // 5

        if (sqlDataType == SqlDataType.SmallMoney) return _smallMoneyVal.Validate(valueString);                 // 6
        if (sqlDataType == SqlDataType.Money) return _moneyVal.Validate(valueString);                           // 7

        if (sqlDataType == SqlDataType.Decimal && !useExtended) return _decimalVal.Validate(valueString);       // 8
        if (sqlDataType == SqlDataType.Decimal && useExtended) return _decimalSqlVal.Validate(valueString);     // 8.1

        if (sqlDataType == SqlDataType.Float) return _floatVal.Validate(valueString);                           // 9
        if (sqlDataType == SqlDataType.Real) return _realVal.Validate(valueString);                             // 10

        if (sqlDataType == SqlDataType.SmallDateTime) return _smallDateTimeVal.Validate(valueString);           // 11
        if (sqlDataType == SqlDataType.Date) return _dateVal.Validate(valueString);                             // 12
        if (sqlDataType == SqlDataType.DateTime) return _dateTimeVal.Validate(valueString);                     // 13
        if (sqlDataType == SqlDataType.DateTime2) return _dateTime2Val.Validate(valueString);                   // 14
        if (sqlDataType == SqlDataType.Time) return _timeVal.Validate(valueString);                             // 15
        if (sqlDataType == SqlDataType.DateTimeOffset) return _dateTimeOffsetVal.Validate(valueString);         // 16

        if (sqlDataType == SqlDataType.Char) return _charVal.Validate(valueString);                             // 17
        if (sqlDataType == SqlDataType.VarChar) return _varCharVal.Validate(valueString, size);                 // 18
        if (sqlDataType == SqlDataType.NChar) return _nCharVal.Validate(valueString);                           // 19
        if (sqlDataType == SqlDataType.NVarChar) return _nVarCharVal.Validate(valueString, size);               // 20

        if (sqlDataType == SqlDataType.UniqueIdentifier) return _uniqueIdentifierVal.Validate(valueString);     // 21

        if (sqlDataType == SqlDataType.Binary) return _binaryVal.Validate(valueString);                         // 22
        if (sqlDataType == SqlDataType.VarBinary) return _varBinaryVal.Validate(valueString, size);             // 23
        if (sqlDataType == SqlDataType.Timestamp) return _timestampVal.Validate(valueString);                   // 24


        // Этот код никогда не выполнится, так как все значения SqlDataType уже обработаны выше.
        return (null, $"SQL type {sqlDataType} is not supported.");

        // Поддерживаемые типы SqlDbType:
        // 1.Bit
        // 2.TinyInt
        // 3.SmallInt
        // 4.Int
        // 5.BigInt
        // 6.SmallMoney
        // 7.Money
        // 8.Decimal
        // 9.Float
        // 10.Real
        // 11.SmallDateTime
        // 12.Date
        // 13.DateTime
        // 14.DateTime2
        // 15.Time
        // 16.DateTimeOffset
        // 17.Char
        // 18.VarChar
        // 19.NChar
        // 20.NVarChar
        // 21.UniqueIdentifier
        // 22.Binary
        // 23.VarBinary
        // 24.Timestamp

        // Неподдерживаемые типы SqlDbType:
        // - Image (устаревший тип)
        // - Text (устаревший тип)
        // - NText (устаревший тип)
        // - Variant
        // - Xml
        // - Udt (пользовательский тип)
        // - Structured (структурированный тип)
    }
}
