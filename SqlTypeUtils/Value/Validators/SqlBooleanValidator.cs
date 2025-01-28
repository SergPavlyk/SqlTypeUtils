using SqlTypeUtils.Value.Messages;
using SqlTypeUtils.Value.Parsers;
using SqlTypeUtils.Value.Utils;
using System.Data;
using System.Data.SqlTypes;

namespace SqlTypeUtils.Value.Validators;

internal class SqlBooleanValidator<T> where T : struct
{
    public SqlDbType SqlDbType { get; }                         // SQL тип данных.
    public string Description { get; }                          // Описание типа.
    public ISqlValueValidatorMsgProvider MsgProvider { get; }   // Провайдер сообщений.    

    public SqlBooleanValidator(SqlDbType dbType, string description, ISqlValueValidatorMsgProvider msgProvider)
    {
        SqlDbType = dbType;
        Description = description;
        MsgProvider = msgProvider;
    }

    public (string? outValue, string message) Validate(string valueString)
    {
        string dataTypeName = MsgProvider.SqlTypeMsg;
        if (typeof(T) == typeof(bool)) dataTypeName = $"{MsgProvider.DotNetTypeMsg} {typeof(bool).Name}";   // .NET Boolean 
        if (typeof(T) == typeof(SqlBoolean)) dataTypeName = $"{MsgProvider.SqlTypeMsg} {SqlDbType}";        // SQL Bit

        // Сообщения
        string successMsg = MsgProvider.SuccessMsg;
        string missingValueMsg = MsgProvider.MissingValueMsg;
        string whitespaceValueMsg = MsgProvider.WhitespaceValueMsg;
        string invalidValueMsg = string.Format(MsgProvider.InvalidValueMsg, StringUtils.TruncateText(valueString), dataTypeName);


        if (string.IsNullOrEmpty(valueString))
        {
            //string missingValueMsg = "Значение отсутствует";
            return (null, missingValueMsg);
        }

        if (string.IsNullOrWhiteSpace(valueString))
        {
            //string whitespaceValueMsg = "Значение содержит только пробелы";
            return (null, whitespaceValueMsg);
        }

        valueString = valueString.Trim();       // Убираем пробелы по краям.

        if (BooleanParser.TryParse(typeof(T), valueString, out object? parsedResult))
        {
            return (parsedResult?.ToString(), successMsg);
        }

        //string invalidValueMsg = "Значение ({0}) не соответствует типу {1}";
        return (null, invalidValueMsg);
    }
}
