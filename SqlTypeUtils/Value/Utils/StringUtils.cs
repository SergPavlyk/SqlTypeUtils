using System.Text;

namespace SqlTypeUtils.Value.Utils;

/// <summary>
/// Утилитарный класс для работы с текстовыми строками.
/// </summary>
internal static class StringUtils
{
    /// <summary>
    /// Обрезает строку до указанной длины и добавляет многоточие, если длина строки больше максимально допустимой.
    /// </summary>
    /// <param name="input">Строка, которую необходимо обрезать.</param>
    /// <param name="maxLength">Максимальная длина строки. Значение по умолчанию: 38.</param>
    /// <returns>
    /// Если длина строки больше максимальной длины, возвращает первые символы строки, с обрезкой до максимальной длины минус три символа (для добавления многоточия).
    /// <br/>В противном случае возвращает исходную строку.
    /// </returns>
    public static string TruncateText(string input, int maxLength = 40)
    {
        if (input.Length > maxLength)
        {
            return $"{input.Substring(0, maxLength - 3)}...";
        }

        return input;
    }


    /// <summary>
    /// Метод для проверки, соответствует ли строка указанной кодировке.
    /// </summary>
    /// <param name="value">Строка, которую нужно проверить.</param>
    /// <param name="expectedEncoding">Ожидаемая кодировка.</param>
    /// <returns>
    /// Возвращает <c>true</c>, если строка после кодирования и декодирования остается идентичной;
    /// <br/>в противном случае <c>false</c>.
    /// </returns>
    public static bool IsValidEncoding(string value, Encoding expectedEncoding)
    {
        byte[] encodedBytes = expectedEncoding.GetBytes(value);
        string decodedValue = expectedEncoding.GetString(encodedBytes);
        return value.Equals(decodedValue);
    }
}