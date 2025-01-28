using SqlTypeUtils.Value.Messages;
using SqlTypeUtils.Value.Utils;
using System.Data;

namespace SqlTypeUtils.Value.Validators;

internal class SqlBinaryValidator
{
    public SqlDbType SqlDbType { get; }                         // SQL тип данных.
    public string Description { get; }                          // Описание типа.
    public int MinLength { get; }                               // Минимальная длина строки.
    public int MaxLength { get; }                               // Максимальная длина строки.
    public int MaxLengthUnlock { get; }                         // Максимальная длина строки без ограничений, для случая разблокировки ограничений.
    public ISqlValueValidatorMsgProvider MsgProvider { get; }   // Провайдер сообщений.

    public SqlBinaryValidator(SqlDbType sqlDbType, string description, int minLength, int maxLength, int maxLengthUnlock, ISqlValueValidatorMsgProvider msgProvider)
    {
        SqlDbType = sqlDbType;
        Description = description;
        MinLength = minLength;
        MaxLength = maxLength;
        MaxLengthUnlock = maxLengthUnlock;
        MsgProvider = msgProvider;
    }

    // Параметр size указывает максимальную длину массива байтов для валидатора:
    //
    // Для типа `VARBINARY`:
    // - Максимальная длина данных — 2 ГБ (2147483647 байт).
    // - Если size отрицательное, используется максимальный размер (`MaxLengthUnlock`), что позволяет хранить данные без ограничений в рамках `VARBINARY(MAX)`.
    //
    // Для типа `BINARY`:
    // - Максимальная длина данных — 8000 байт.
    // - Если size отрицательное, применяется значение `MaxLength`.
    //
    // Для типа `TIMESTAMP`:
    // - Длина фиксирована и составляет 8 байт.
    // - Максимальная длина не используется, так как длина всегда постоянная.
    //
    // Логика работы с параметром `size`:
    // - Если параметр `size` не задан или равен 0, используется значение `MaxLength`.
    // - Если значение `size` отрицательное, применяется `MaxLengthUnlock`.
    // - Если `size` больше, чем `MaxLength`, используется значение `MaxLength`.
    // - Если `size` больше, чем `MaxLengthUnlock`, применяется значение `MaxLengthUnlock`.

    public (string? outValue, string message) Validate(string hexString, int? size = null)
    {
        string dataTypeName = $"{MsgProvider.SqlTypeMsg} {SqlDbType}";                  // SQL SqlDbType (используется по умолчанию)

        // Сообщения
        string successMsg = MsgProvider.SuccessMsg;
        string missingValueMsg = MsgProvider.MissingValueMsg;
        string whitespaceValueMsg = MsgProvider.WhitespaceValueMsg;

        string oddLengthMsg = string.Format(MsgProvider.OddLengthMsg, hexString, dataTypeName);
        string byteLengthTooSmallMsg = "";
        string byteLengthTooLargeMsg = "";
        string invalidValueMsg = string.Format(MsgProvider.InvalidValueMsg, StringUtils.TruncateText(hexString), dataTypeName);
        string invalidHexCharsMsg = string.Format(MsgProvider.InvalidHexCharsMsg, StringUtils.TruncateText(hexString), dataTypeName);

        // Проверка на null или пустую строку
        if (string.IsNullOrEmpty(hexString))
        {
            //string missingValueMsg = "Значение отсутствует";
            return (null, missingValueMsg);
        }
        else if (string.IsNullOrWhiteSpace(hexString))
        {
            //string whitespaceValueMsg = "Значение содержит только пробелы";
            return (null, whitespaceValueMsg);
        }

        // Если строка содержит только "0", она заменяется на "0x" для представления пустого бинарного значения длиной 0
        hexString = hexString == "0" ? "0x" : hexString;

        // Удаляем пробелы и дефисы из строки
        hexString = hexString.Replace(" ", string.Empty).Replace("-", string.Empty);

        // Убираем префикс "0x", если он есть
        if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            hexString = hexString.Substring(2);
        }

        // Проверка на четность длины строки
        if (hexString.Length % 2 != 0)
        {
            //string oddLengthMsg = "Значение '{0}' должно иметь чётное количество символов";
            return (null, oddLengthMsg);
        }

        try
        {
            // Преобразуем строку в массив байтов
            byte[] byteArray = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                // Преобразуем два символа шестнадцатеричной строки в один байт
                byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            // Проверка длины массива байтов
            int byteArrayLength = byteArray.Length;

            if (size.HasValue)
            {
                if (size > MaxLength) size = MaxLengthUnlock;
                if (size == 0) size = MaxLength;
                if (size < 0) size = MaxLengthUnlock;

                if (byteArrayLength > size)
                {

                    //string byteLengthTooLargeMsg = "Длина массива байтов превышает допустимый максимум - {size}";
                    byteLengthTooLargeMsg = string.Format(MsgProvider.ByteLengthTooLargeMsg, size);
                    return (null, byteLengthTooLargeMsg);
                }
            }
            else
            {
                //if (size < 0) size = MaxLength;
                if (byteArrayLength < MinLength)
                {
                    //string byteLengthTooSmallMsg = "Длина массива байтов меньше допустимого минимума - {MinLength}";
                    byteLengthTooSmallMsg = string.Format(MsgProvider.ByteLengthTooSmallMsg, MinLength);
                    return (null, byteLengthTooSmallMsg);
                }
                else if (byteArrayLength > MaxLength)
                {
                    //string byteLengthTooLargeMsg = "Длина массива байтов превышает допустимый максимум - {MaxLength}";
                    byteLengthTooLargeMsg = string.Format(MsgProvider.ByteLengthTooLargeMsg, MaxLength);
                    return (null, byteLengthTooLargeMsg);
                }
            }

            //string successMsg = "OK";
            return ($"0x{BitConverter.ToString(byteArray).Replace("-", "")}", successMsg);
        }
        catch (FormatException)
        {
            //string invalidValueMsg = "Значение не соответствует типу {SqlDbType}";
            return (null, string.Format(invalidValueMsg, SqlDbType));
        }
        catch (OverflowException)
        {
            //string invalidHexCharsMsg = "Значение содержит недопустимые символы для шестнадцатичного формата";
            return (null, invalidHexCharsMsg);
        }
    }
}
