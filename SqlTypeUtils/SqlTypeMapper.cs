using SqlTypeUtils.Value.Enums;
using System.Data;

namespace SqlTypeUtils;

/// <summary>
/// Класс, предоставляющий функциональность для сопоставления типов данных между .NET и SQL.
/// </summary>
public static class SqlTypeMapper
{
    /// <summary>
    /// Сопоставляет тип данных .NET с соответствующим типом данных SQL из перечисления <see cref="SqlDataType"/>.
    /// </summary>
    /// <param name="dotNetType">Тип данных .NET, который нужно сопоставить с типом <see cref="SqlDataType"/>.</param>
    /// <returns>
    /// Возвращает соответствующий тип <see cref="SqlDataType"/> для указанного типа .NET, или null, если тип не поддерживается.
    /// </returns>
    /// <remarks>
    /// Неподдерживаемые типы .NET:
    /// <br/>▪ <see cref="ushort"/> (не поддерживается в SQL Server)
    /// <br/>▪ <see cref="uint"/> (не поддерживается в SQL Server)
    /// <br/>▪ <see cref="ulong"/> (не поддерживается в SQL Server)
    /// <br/>▪ <see cref="sbyte"/> (не поддерживается в SQL Server)
    /// <br/>▪ <see cref="object"/> (не поддерживается в SQL Server)
    /// <br/>▪ <see cref="dynamic"/> (не поддерживается в SQL Server)
    /// </remarks>
    public static SqlDataType? GetSqlDataType(Type dotNetType)
    {
        if (dotNetType == typeof(bool)) return SqlDataType.Bit;                             // 1.  SqlDbType.Bit;

        if (dotNetType == typeof(byte)) return SqlDataType.TinyInt;                         // 2.  SqlDbType.TinyInt;
        if (dotNetType == typeof(short)) return SqlDataType.SmallInt;                       // 3.  SqlDbType.SmallInt;
        if (dotNetType == typeof(int)) return SqlDataType.Int;                              // 4.  SqlDbType.Int;
        if (dotNetType == typeof(long)) return SqlDataType.BigInt;                          // 5.  SqlDbType.BigInt;

        // decimal --> SqlDbType.SmallMoney;                                                // 6.  SqlDbType.SmallMoney;
        // decimal --> SqlDbType.Money;                                                     // 7.  SqlDbType.Money;

        if (dotNetType == typeof(decimal)) return SqlDataType.Decimal;                      // 8.  SqlDbType.Decimal;

        if (dotNetType == typeof(double)) return SqlDataType.Float;                         // 9.  SqlDbType.Float;
        if (dotNetType == typeof(float)) return SqlDataType.Real;                           // 10. SqlDbType.Real;

        // DateTime --> SqlDbType.SmallDateTime                                             // 11. SqlDbType.SmallDateTime;
        // DateTime --> SqlDbType.DateTime                                                  // 12. SqlDbType.DateTime;
        if (dotNetType == typeof(DateTime)) return SqlDataType.DateTime2;                   // 13. SqlDbType.DateTime2;
        if (dotNetType == typeof(DateTimeOffset)) return SqlDataType.DateTimeOffset;        // 14. SqlDbType.DateTimeOffset;
        if (dotNetType == typeof(TimeSpan)) return SqlDataType.Time;
        if (dotNetType == typeof(TimeOnly)) return SqlDataType.Time;                        // 15. SqlDbType.Time;
        if (dotNetType == typeof(DateOnly)) return SqlDataType.Date;                        // 16. SqlDbType.Date;

        // string --> SqlDbType.Char;                                                       // 17. SqlDbType.Char;
        // string --> SqlDbType.VarChar;                                                    // 18. SqlDbType.VarChar;
        // string --> SqlDbType.NChar;                                                      // 19. SqlDbType.NChar;
        if (dotNetType == typeof(string)) return SqlDataType.NVarChar;                      // 20. SqlDbType.NVarChar;

        if (dotNetType == typeof(Guid)) return SqlDataType.UniqueIdentifier;                // 21. SqlDbType.UniqueIdentifier;

        // byte[] --> SqlDbType.Binary                                                      // 22. SqlDbType.Binary;
        if (dotNetType == typeof(byte[])) return SqlDataType.VarBinary;                     // 23. SqlDbType.VarBinary;
        // byte[] --> SqlDbType.Timestamp                                                   // 24. SqlDbType.Timestamp;


        // Неподдерживаемые типы DotNet:
        // - ushort (не поддерживается в SQL Server)
        // - uint (не поддерживается в SQL Server)
        // - ulong (не поддерживается в SQL Server)
        // - sbyte (не поддерживается в SQL Server)
        // - object (не поддерживается в SQL Server)
        // - dynamic (не поддерживается в SQL Server)

        // Возврат null, если тип не поддерживается
        return null;
    }

    /// <summary>
    /// Сопоставляет тип данных SQL из перечисления <see cref="SqlDataType"/> с соответствующим типом из перечисления <see cref="SqlDbType"/>.
    /// </summary>
    /// <param name="sqlDataType">Тип данных SQL, который нужно сопоставить с типом <see cref="SqlDbType"/>.</param>
    /// <returns>Возвращает соответствующий тип <see cref="SqlDbType"/> для указанного типа <see cref="SqlDataType"/>.</returns>
    public static SqlDbType GetSqlDbType(SqlDataType sqlDataType)
    {
        if (sqlDataType == SqlDataType.Bit) return SqlDbType.Bit;                           // 1.  SqlDbType.Bit;

        if (sqlDataType == SqlDataType.TinyInt) return SqlDbType.TinyInt;                   // 2.  SqlDbType.TinyInt;
        if (sqlDataType == SqlDataType.SmallInt) return SqlDbType.SmallInt;                 // 3.  SqlDbType.SmallInt;
        if (sqlDataType == SqlDataType.Int) return SqlDbType.Int;                           // 4.  SqlDbType.Int;
        if (sqlDataType == SqlDataType.BigInt) return SqlDbType.BigInt;                     // 5.  SqlDbType.BigInt;

        if (sqlDataType == SqlDataType.SmallMoney) return SqlDbType.SmallMoney;             // 6.  SqlDbType.SmallMoney;
        if (sqlDataType == SqlDataType.Money) return SqlDbType.Money;                       // 7.  SqlDbType.Money;
        if (sqlDataType == SqlDataType.Decimal) return SqlDbType.Decimal;                   // 8.  SqlDbType.Decimal;

        if (sqlDataType == SqlDataType.Float) return SqlDbType.Float;                       // 9.  SqlDbType.Float;
        if (sqlDataType == SqlDataType.Real) return SqlDbType.Real;                         // 10. SqlDbType.Real;

        if (sqlDataType == SqlDataType.SmallDateTime) return SqlDbType.SmallDateTime;       // 11. SqlDbType.SmallDateTime;
        if (sqlDataType == SqlDataType.DateTime) return SqlDbType.DateTime;                 // 12. SqlDbType.DateTime;
        if (sqlDataType == SqlDataType.DateTime2) return SqlDbType.DateTime2;               // 13. SqlDbType.DateTime2;
        if (sqlDataType == SqlDataType.DateTimeOffset) return SqlDbType.DateTimeOffset;     // 14. SqlDbType.DateTimeOffset;
        if (sqlDataType == SqlDataType.Time) return SqlDbType.Time;                         // 15. SqlDbType.Time;
        if (sqlDataType == SqlDataType.Date) return SqlDbType.Date;                         // 16. SqlDbType.Date;

        if (sqlDataType == SqlDataType.Char) return SqlDbType.Char;                         // 17. SqlDbType.Char;
        if (sqlDataType == SqlDataType.VarChar) return SqlDbType.VarChar;                   // 18. SqlDbType.VarChar;
        if (sqlDataType == SqlDataType.NChar) return SqlDbType.NChar;                       // 19. SqlDbType.NChar;
        if (sqlDataType == SqlDataType.NVarChar) return SqlDbType.NVarChar;                 // 20. SqlDbType.NVarChar;

        if (sqlDataType == SqlDataType.UniqueIdentifier) return SqlDbType.UniqueIdentifier; // 21. SqlDbType.UniqueIdentifier;

        if (sqlDataType == SqlDataType.Binary) return SqlDbType.Binary;                     // 22. SqlDbType.Binary;
        if (sqlDataType == SqlDataType.VarBinary) return SqlDbType.VarBinary;               // 23. SqlDbType.VarBinary;
        if (sqlDataType == SqlDataType.Timestamp) return SqlDbType.Timestamp;               // 24. SqlDbType.Timestamp;

        // Этот код никогда не выполнится, так как все значения SqlDataType уже обработаны выше.
        return SqlDbType.NVarChar;
    }

    /// <summary>
    /// Сопоставляет тип данных SQL из перечисления <see cref="SqlDataType"/> с соответствующим типом из перечисления <see cref="DbType"/>.
    /// </summary>
    /// <param name="sqlDataType">Тип данных SQL, который нужно сопоставить с типом <see cref="DbType"/>.</param>
    /// <returns>Возвращает соответствующий тип <see cref="DbType"/> для указанного типа <see cref="SqlDataType"/>.</returns>
    public static DbType GetDbType(SqlDataType sqlDataType)
    {
        if (sqlDataType == SqlDataType.Bit) return DbType.Boolean;                          // 1.  SqlDbType.Bit;

        if (sqlDataType == SqlDataType.TinyInt) return DbType.Byte;                         // 2.  SqlDbType.TinyInt;
        if (sqlDataType == SqlDataType.SmallInt) return DbType.Int16;                       // 3.  SqlDbType.SmallInt;
        if (sqlDataType == SqlDataType.Int) return DbType.Int32;                            // 4.  SqlDbType.Int;
        if (sqlDataType == SqlDataType.BigInt) return DbType.Int64;                         // 5.  SqlDbType.BigInt;

        if (sqlDataType == SqlDataType.SmallMoney) return DbType.Currency;                  // 6.  SqlDbType.SmallMoney;
        if (sqlDataType == SqlDataType.Money) return DbType.Currency;                       // 7.  SqlDbType.Money;
        if (sqlDataType == SqlDataType.Decimal) return DbType.Decimal;                      // 8.  SqlDbType.Decimal;

        if (sqlDataType == SqlDataType.Float) return DbType.Double;                         // 9.  SqlDbType.Float;
        if (sqlDataType == SqlDataType.Real) return DbType.Single;                          // 10. SqlDbType.Real;

        if (sqlDataType == SqlDataType.SmallDateTime) return DbType.DateTime;               // 11. SqlDbType.SmallDateTime;
        if (sqlDataType == SqlDataType.DateTime) return DbType.DateTime;                    // 12. SqlDbType.DateTime;
        if (sqlDataType == SqlDataType.DateTime2) return DbType.DateTime2;                  // 13. SqlDbType.DateTime2;
        if (sqlDataType == SqlDataType.DateTimeOffset) return DbType.DateTimeOffset;        // 14. SqlDbType.DateTimeOffset;
        if (sqlDataType == SqlDataType.Time) return DbType.Time;                            // 15. SqlDbType.Time;
        if (sqlDataType == SqlDataType.Date) return DbType.Date;                            // 16. SqlDbType.Date;

        if (sqlDataType == SqlDataType.Char) return DbType.AnsiStringFixedLength;           // 17. SqlDbType.Char;
        if (sqlDataType == SqlDataType.VarChar) return DbType.AnsiString;                   // 18. SqlDbType.VarChar;
        if (sqlDataType == SqlDataType.NChar) return DbType.StringFixedLength;              // 19. SqlDbType.NChar;
        if (sqlDataType == SqlDataType.NVarChar) return DbType.String;                      // 20. SqlDbType.NVarChar;

        if (sqlDataType == SqlDataType.UniqueIdentifier) return DbType.Guid;                // 21. SqlDbType.UniqueIdentifier;

        if (sqlDataType == SqlDataType.Binary) return DbType.Binary;                        // 22. SqlDbType.Binary;
        if (sqlDataType == SqlDataType.VarBinary) return DbType.Binary;                     // 23. SqlDbType.VarBinary;
        if (sqlDataType == SqlDataType.Timestamp) return DbType.Binary;                     // 24. SqlDbType.Timestamp;


        // Неподдерживаемые типы SqlDbType:
        // - Image (устаревший тип)
        // - Text (устаревший тип)
        // - NText (устаревший тип)
        // - Variant
        // - Xml
        // - Udt
        // - Structured

        // Этот код никогда не выполнится, так как все значения SqlDataType уже обработаны выше.
        return DbType.String;
    }

    /// <summary>
    /// Сопоставляет тип данных SQL из перечисления <see cref="SqlDataType"/> с соответствующим типом .NET.
    /// </summary>
    /// <param name="sqlDataType">Тип данных SQL, который нужно сопоставить с типом .NET.</param>
    /// <returns>
    /// Возвращает соответствующий тип .NET для указанного типа <see cref="Type"/>.
    /// Если тип не найден, возвращает <see cref="void"/>.
    /// </returns>
    public static Type GetType(SqlDataType sqlDataType)
    {
        if (sqlDataType == SqlDataType.Bit) return typeof(bool);                            // 1.  SqlDbType.Bit;

        if (sqlDataType == SqlDataType.TinyInt) return typeof(byte);                        // 2.  SqlDbType.TinyInt;
        if (sqlDataType == SqlDataType.SmallInt) return typeof(short);                      // 3.  SqlDbType.SmallInt;
        if (sqlDataType == SqlDataType.Int) return typeof(int);                             // 4.  SqlDbType.Int;
        if (sqlDataType == SqlDataType.BigInt) return typeof(long);                         // 5.  SqlDbType.BigInt;

        if (sqlDataType == SqlDataType.SmallMoney) return typeof(decimal);                  // 6.  SqlDbType.SmallMoney;
        if (sqlDataType == SqlDataType.Money) return typeof(decimal);                       // 7.  SqlDbType.Money;

        if (sqlDataType == SqlDataType.Decimal) return typeof(decimal);                     // 8.  SqlDbType.Decimal;
        if (sqlDataType == SqlDataType.Float) return typeof(double);                        // 9.  SqlDbType.Float;
        if (sqlDataType == SqlDataType.Real) return typeof(float);                          // 10. SqlDbType.Real;

        if (sqlDataType == SqlDataType.SmallDateTime) return typeof(DateTime);              // 11. SqlDbType.SmallDateTime;
        if (sqlDataType == SqlDataType.DateTime) return typeof(DateTime);                   // 12. SqlDbType.DateTime;
        if (sqlDataType == SqlDataType.DateTime2) return typeof(DateTime);                  // 13. SqlDbType.DateTime2;
        if (sqlDataType == SqlDataType.DateTimeOffset) return typeof(DateTimeOffset);       // 14. SqlDbType.DateTimeOffset;
        if (sqlDataType == SqlDataType.Time) return typeof(TimeSpan);                       // 15. SqlDbType.Time;
        if (sqlDataType == SqlDataType.Date) return typeof(DateTime);                       // 16. SqlDbType.Date;

        if (sqlDataType == SqlDataType.Char) return typeof(string);                         // 17. SqlDbType.Char;
        if (sqlDataType == SqlDataType.VarChar) return typeof(string);                      // 18. SqlDbType.VarChar;
        if (sqlDataType == SqlDataType.NChar) return typeof(string);                        // 19. SqlDbType.NChar;
        if (sqlDataType == SqlDataType.NVarChar) return typeof(string);                     // 20. SqlDbType.NVarChar;

        if (sqlDataType == SqlDataType.UniqueIdentifier) return typeof(Guid);               // 21. SqlDbType.UniqueIdentifier;

        if (sqlDataType == SqlDataType.Binary) return typeof(byte[]);                       // 22. SqlDbType.Binary;
        if (sqlDataType == SqlDataType.VarBinary) return typeof(byte[]);                    // 23. SqlDbType.VarBinary;
        if (sqlDataType == SqlDataType.Timestamp) return typeof(byte[]);                    // 24. SqlDbType.Timestamp;

        // Этот код никогда не выполнится, так как все значения SqlDataType уже обработаны выше.
        return typeof(void);
    }
}
