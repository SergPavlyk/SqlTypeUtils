using SqlTypeUtils.Value.Messages;
using SqlTypeUtils.Value.Utils;
using System.Data;
using System.Text;

namespace SqlTypeUtils.Value.Validators;

internal class SqlStringValidator
{
    public SqlDbType SqlDbType { get; }                         // SQL тип данных.
    public string Description { get; }                          // Описание типа.
    public int MinLength { get; }                               // Минимальная длина строки.
    public int MaxLength { get; }                               // Максимальная длина строки.
    public Encoding Encoding { get; }                           // Кодировка.
    public int MaxLengthUnlock { get; }                         // Максимальная длина строки без ограничений, для случая разблокировки ограничений.
    public ISqlValueValidatorMsgProvider MsgProvider { get; }   // Провайдер сообщений.   

    public SqlStringValidator(SqlDbType sqlDbType, string description, int minLength, int maxLength, int maxLengthUnlock, Encoding encoding, ISqlValueValidatorMsgProvider msgProvider)
    {
        SqlDbType = sqlDbType;
        Description = description;
        MinLength = minLength;
        MaxLength = maxLength;
        Encoding = encoding;
        MaxLengthUnlock = maxLengthUnlock;
        MsgProvider = msgProvider;
    }

    // Параметр size указывает максимальную длину строки для валидатора:
    //
    // Для типа `VARCHAR`:
    // - Максимальная длина 8000 байт.
    // - Если size отрицательное, используется `MaxLengthUnlock`, что позволяет хранить до 2 ГБ (2147483647 байт) данных (VARCHAR(MAX)).

    // Для типа `NVARCHAR`:
    // - Максимальная длина 4000 байт (каждый символ занимает 2 байта).
    // - Если size отрицательное, используется `MaxLengthUnlock`, что позволяет хранить до 1 ГБ (1073741823 байта) данных (NVARCHAR(MAX)).
    //
    // Для типа `CHAR`:
    // - Максимальная длина 8000 байт.
    // - Если size отрицательное, применяется `MaxLength`.
    //
    // Для типа `NCHAR`:
    // - Максимальная длина 4000 байт (каждый символ занимает 2 байта).
    // - Если size отрицательное, применяется `MaxLength`.
    //
    // Логика работы с параметром `size`:
    // - Если параметр `size` не задан или равен 0, используется значение `MaxLength`.
    // - Если значение `size` отрицательное, применяется `MaxLengthUnlock`.
    // - Если `size` больше, чем `MaxLength`, используется значение `MaxLength`.
    // - Если `size` больше, чем `MaxLengthUnlock`, применяется значение `MaxLengthUnlock`.

    public (string? outValue, string message) Validate(string valueString, int? size = null)
    {
        // Сообщения
        string successMsg = MsgProvider.SuccessMsg;
        string encodingMismatchMsg = string.Format(MsgProvider.EncodingMismatchMsg, Encoding.EncodingName);
        string lengthExceedMsg = "";
        string lengthShortMsg = "";


        // Проверка кодировки
        if (!StringUtils.IsValidEncoding(valueString, Encoding))
        {
            //string encodingMismatchMsg = "Строка не соответствует ожидаемой кодировке {Encoding.EncodingName}";
            return (null, encodingMismatchMsg);
        }

        if (size.HasValue)
        {
            if (size == 0) size = MaxLength;
            if (size < 0) size = MaxLengthUnlock;
            if (size > MaxLengthUnlock) size = MaxLengthUnlock;

            if (valueString.Length > size)
            {
                //string lengthExceedMsg = "Длина строки превышает допустимый максимум - {size}";
                lengthExceedMsg = string.Format(MsgProvider.LengthExceedMsg, size);
                return (null, lengthExceedMsg);
            }
        }
        else
        {
            int length = valueString.Length;

            if (length < MinLength)
            {
                //string lengthShortMsg = "Длина строки меньше допустимого минимума - {MinLength}";
                lengthShortMsg = string.Format(MsgProvider.LengthShortMsg, MinLength);
                return (null, lengthShortMsg);
            }
            else if (length > MaxLength)
            {
                //string lengthExceedMsg = "Длина строки превышает допустимый максимум - {MaxLength}";
                lengthExceedMsg = string.Format(MsgProvider.LengthExceedMsg, MaxLength);
                return (null, lengthExceedMsg);
            }
        }

        //string successMsg = "OK";
        return (valueString, successMsg);
    }
}
