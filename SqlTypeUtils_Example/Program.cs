using SqlTypeUtils;
using SqlTypeUtils.Value.Enums;
using SqlTypeUtils.Value.Messages;
using System.Data;
using System.Text;

namespace SqlTypeUtils_Example;

internal class Program
{
    static void Main(string[] args)
    {
        // Error message provider for SQL value validator.
        // In this case, an English language message provider is used.
        // If necessary, you can create a custom provider for other languages.
        ISqlValueValidatorMsgProvider msgProvider = new SqlValValidMessEn();

        // Create an instance of the SQL value validator, initialized with the message provider.
        // The validator will use the provided provider to generate error messages in English.
        SqlValueValidator validator = new SqlValueValidator(msgProvider);


        #region SqlBoolean

        //  -> Boolean type.
        var boolResult = validator.Validate(SqlDataType.Bit, "true");
        Console.WriteLine($"1.  Bit: {boolResult.outValue}, Message: {boolResult.message}");

        var boolSqlResult = validator.Validate(SqlDataType.Bit, "1", useExtended: true);
        Console.WriteLine($"1.1 Bit: {boolSqlResult.outValue}, Message: {boolSqlResult.message}");
        Console.WriteLine();
        #endregion

        #region SqlNumeric

        //  -> Integer types.
        var tinyIntResult = validator.Validate(SqlDataType.TinyInt, "255");
        Console.WriteLine($"2.  TinyInt: {tinyIntResult.outValue}, Message: {tinyIntResult.message}");

        var smallIntResult = validator.Validate(SqlDataType.SmallInt, "32767");
        Console.WriteLine($"3.  SmallInt: {smallIntResult.outValue}, Message: {smallIntResult.message}");

        var intResult = validator.Validate(SqlDataType.Int, "2147483647");
        Console.WriteLine($"4.  Int: {intResult.outValue}, Message: {intResult.message}");

        var bigIntResult = validator.Validate(SqlDataType.BigInt, "9223372036854775807");
        Console.WriteLine($"5.  BigInt: {bigIntResult.outValue}, Message: {bigIntResult.message}");
        Console.WriteLine();
        //---------------------------------------------------------------------------------------------------------

        //  -> Monetary types.
        var smallMoneyResult = validator.Validate(SqlDataType.SmallMoney, "214748.3647");
        Console.WriteLine($"6.  SmallMoney: {smallMoneyResult.outValue}, Message: {smallMoneyResult.message}");

        var moneyResult = validator.Validate(SqlDataType.Money, "922337203685477.5807");
        Console.WriteLine($"7.  Money: {moneyResult.outValue}, Message: {moneyResult.message}");
        Console.WriteLine();
        //---------------------------------------------------------------------------------------------------------

        //  -> Decimal type.
        var decimalResult = validator.Validate(SqlDataType.Decimal, "9999999999999999999999999999");
        Console.WriteLine($"8.  Decimal: {decimalResult.outValue}, Message: {decimalResult.message}");

        var decimalSqlResult = validator.Validate(SqlDataType.Decimal, "99999999999999999999999999999999999999", useExtended: true);
        Console.WriteLine($"8.1 Decimal: {decimalSqlResult.outValue}, Message: {decimalSqlResult.message}");
        //---------------------------------------------------------------------------------------------------------

        //  -> Floating-point types.
        var floatResult = validator.Validate(SqlDataType.Float, "1.7976931348623157E+308");
        Console.WriteLine($"9.  Float: {floatResult.outValue}, Message: {floatResult.message}");

        var realResult = validator.Validate(SqlDataType.Real, "3.4028235E+38");
        Console.WriteLine($"10. Real: {realResult.outValue}, Message: {realResult.message}");
        Console.WriteLine();
        #endregion

        #region SqlDateTime

        //  -> Date and time types.
        var smallDateTimeResult = validator.Validate(SqlDataType.SmallDateTime, "2079-01-06 15:45:00");
        Console.WriteLine($"11. SmallDateTime: {smallDateTimeResult.outValue}, Message: {smallDateTimeResult.message}");

        var dateResult = validator.Validate(SqlDataType.Date, "2025.01.17");
        Console.WriteLine($"12. Date: {dateResult.outValue}, Message: {dateResult.message}");

        var dateTimeResult = validator.Validate(SqlDataType.DateTime, "9999-01-06 15:45:00.999");
        Console.WriteLine($"13. DateTime: {dateTimeResult.outValue}, Message: {dateTimeResult.message}");

        var dateTime2Result = validator.Validate(SqlDataType.DateTime2, "9999-12-31 23:59:59.9999999");
        Console.WriteLine($"14. DateTime2: {dateTime2Result.outValue}, Message: {dateTime2Result.message}");

        var timeResult = validator.Validate(SqlDataType.Time, "23:59:59.9999999");
        Console.WriteLine($"15. Time: {timeResult.outValue}, Message: {timeResult.message}");

        var dateTimeOffsetResult = validator.Validate(SqlDataType.DateTimeOffset, "9999-12-31 23:59:59.9999999+03:00");
        Console.WriteLine($"16. DateTimeOffset: {dateTimeOffsetResult.outValue}, Message: {dateTimeOffsetResult.message}");
        Console.WriteLine();
        #endregion

        #region SqlString

        //  -> String types.
        var charResult = validator.Validate(SqlDataType.Char, "This is a Char type");
        Console.WriteLine($"17. Char: {charResult.outValue}, Message: {charResult.message}");

        var varCharResult = validator.Validate(SqlDataType.VarChar, "This is a VarChar type", size: 0);
        Console.WriteLine($"18. VarChar: {varCharResult.outValue}, Message: {varCharResult.message}");

        var nCharResult = validator.Validate(SqlDataType.NChar, "This is an NChar type string");
        Console.WriteLine($"19. NChar: {nCharResult.outValue}, Message: {nCharResult.message}");

        var nVarCharResult = validator.Validate(SqlDataType.NVarChar, "This is an NVarChar type string", size: 0);
        Console.WriteLine($"20. NVarChar: {nVarCharResult.outValue}, Message: {nVarCharResult.message}");
        Console.WriteLine();
        #endregion

        #region SqlUniqueIdentifier

        //  -> Unique identifier type (GUID).
        var uniqueIdentifierResult = validator.Validate(SqlDataType.UniqueIdentifier, "dfe518e8-ba00-40cc-84dc-8fd52e072703");
        Console.WriteLine($"21. UniqueIdentifier: {uniqueIdentifierResult.outValue}, Message: {uniqueIdentifierResult.message}");
        Console.WriteLine();
        #endregion

        #region SqlBinary

        // -> Binary types
        var binaryResult = validator.Validate(SqlDataType.Binary, string.Join(" ", Encoding.UTF8.GetBytes("123456789")));
        Console.WriteLine($"22. Binary: {binaryResult.outValue}, Message: {binaryResult.message}");

        var varbinaryResult = validator.Validate(SqlDataType.VarBinary, string.Join(" ", Encoding.UTF8.GetBytes("123456789")), size: 0);
        Console.WriteLine($"23. VarBinary: {varbinaryResult.outValue}, Message: {varbinaryResult.message}");

        var timestampResult = validator.Validate(SqlDataType.Timestamp, "00000000000007D8");
        Console.WriteLine($"24. Timestamp: {timestampResult.outValue}, Message: {timestampResult.message}");
        Console.WriteLine();
        Console.WriteLine();
        #endregion


        #region SqlTypeMapper

        // 1. .Net Type -> SqlDataType
        Type netType = typeof(byte);
        SqlDataType? sqlDataType_1 = SqlTypeMapper.GetSqlDataType(netType);
        Console.WriteLine($"1. For .Net type '{netType.Name}', the SqlDataType is: {sqlDataType_1?.ToString() ?? "Not found"}");

        // 2. SqlDataType -> SqlDbType
        SqlDataType sqlDataType_2 = SqlDataType.BigInt;
        SqlDbType sqlDbType_2 = SqlTypeMapper.GetSqlDbType(sqlDataType_2);
        Console.WriteLine($"2. For SqlDataType '{sqlDataType_2}', the SqlDbType is: {sqlDbType_2}");

        // 3. SqlDataType -> DbType
        SqlDataType sqlDataType_3 = SqlDataType.Money;
        DbType dbType = SqlTypeMapper.GetDbType(sqlDataType_3);
        Console.WriteLine($"3. For SqlDataType '{sqlDataType_3}', the DbType is: {dbType}");

        // 4. SqlDataType -> .Net Type
        SqlDataType sqlDataType_4 = SqlDataType.UniqueIdentifier;
        Type? type = SqlTypeMapper.GetType(sqlDataType_4);
        Console.WriteLine($"4. For SqlDataType {sqlDataType_4}, the .Net type is: {type?.Name ?? "Not found"}");
        #endregion

    }
}