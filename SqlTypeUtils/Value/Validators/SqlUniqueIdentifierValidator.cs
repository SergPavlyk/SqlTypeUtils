using SqlTypeUtils.Value.Messages;
using SqlTypeUtils.Value.Utils;
using System.Data;

namespace SqlTypeUtils.Value.Validators;

internal class SqlUniqueIdentifierValidator
{
    public SqlDbType SqlDbType { get; }                         // SQL тип данных.
    public string Description { get; }                          // Описание типа.
    public ISqlValueValidatorMsgProvider MsgProvider { get; }   // Провайдер сообщений.    

    public SqlUniqueIdentifierValidator(SqlDbType sqlDbType, string description, ISqlValueValidatorMsgProvider msgProvider)
    {
        SqlDbType = sqlDbType;
        Description = description;
        MsgProvider = msgProvider;
    }

    public (string? outValue, string message) Validate(string valueString)
    {
        string dataTypeName = $"{MsgProvider.SqlTypeMsg} {SqlDbType}";                  // SQL SqlDbType (используется по умолчанию)

        // Сообщения
        string successMsg = MsgProvider.SuccessMsg;
        string missingValueMsg = MsgProvider.MissingValueMsg;
        string whitespaceValueMsg = MsgProvider.WhitespaceValueMsg;
        string invalidValueMsg = string.Format(MsgProvider.InvalidValueMsg, StringUtils.TruncateText(valueString), dataTypeName);

        // Проверка на null или пустую строку
        if (string.IsNullOrEmpty(valueString))
        {
            //string missingValueMsg = "Значение отсутствует";
            return (null, missingValueMsg);
        }
        else if (string.IsNullOrWhiteSpace(valueString))
        {
            //string whitespaceValueMsg = "Значение содержит только пробелы";
            return (null, whitespaceValueMsg);
        }

        // Попытка парсинга строки в Guid
        if (Guid.TryParse(valueString, out Guid parsedGuid))
        {
            //string successMsg = "OK";
            return (parsedGuid.ToString(), successMsg);
        }
        else
        {
            //string invalidValueMsg = "Значение '{valueString}' не соответствует типу {SqlDbType}";
            return (null, invalidValueMsg);
        }
    }
}
