using System.Text.RegularExpressions;

namespace SqlTypeUtils.Value.Utils;

/// <summary>
/// Перечисление, представляющее возможные операторы сравнения для чисел.
/// </summary>
internal enum ComparisonType
{
    /// <summary>
    /// Оператор "больше", используется для проверки, что первое число больше второго.
    /// </summary>
    GreaterThan = 1,    // Больше.

    /// <summary>
    /// Оператор "меньше", используется для проверки, что первое число меньше второго.
    /// </summary>
    LessThan = -1,      // Меньше.

    /// <summary>
    /// Оператор "равно", используется для проверки, что два числа равны.
    /// </summary>
    EqualTo = 0,        // Равно.

    /// <summary>
    /// Оператор "не равно", используется для проверки, что два числа не равны.
    /// </summary>
    NotEqualTo = -2     // Не равно.
}

/// <summary>
/// Перечисление, представляющее возможные типы чисел: целое, дробное и некорректное.
/// </summary>
internal enum NumberType
{
    /// <summary>
    /// Тип, представляющий целое число.
    /// </summary>
    Integer,

    /// <summary>
    /// Тип, представляющий дробное число.
    /// </summary>
    Decimal,

    /// <summary>
    /// Тип, представляющий некорректное значение.
    /// </summary>
    Invalid
}

/// <summary>
/// Утилитарный класс для работы с числами, представленными строками. 
/// <br/>Содержит методы для сравнения чисел, определения их типов, а также преобразования десятичных разделителей.
/// </summary>
internal static class NumericStringUtils
{
    /// <summary>
    /// Пытается выполнить сравнение двух чисел, представленных строками.
    /// <br/>Проверяет, являются ли строки допустимыми числами, и затем сравнивает их с использованием указанного оператора сравнения.
    /// </summary>
    /// <param name="firstNumStr">Первая строка, представляющая число для сравнения.</param>
    /// <param name="comparisonOperator">Оператор сравнения, который определяет, что нужно проверить: больше, меньше, равно или не равно.</param>
    /// <param name="secondNumStr">Вторая строка, представляющая число для сравнения.</param>
    /// <param name="result">Результат сравнения, если метод выполнен успешно. <c>true</c>, если условие сравнения истинно, иначе <c>false</c>.</param>
    /// <returns>
    /// Возвращает <c>true</c>, если сравнение выполнено успешно, иначе <c>false</c>.
    /// </returns>
    public static bool TryIsComparisonValid(string firstNumStr, ComparisonType comparisonOperator, string secondNumStr, out bool result)
    {
        result = false; // Устанавливаем значение по умолчанию.

        try
        {
            // Выполняем проверку и сравнение с использованием основного метода.
            result = IsComparisonValid(firstNumStr, comparisonOperator, secondNumStr);
            return true; // Успешное выполнение.
        }
        catch
        {
            return false; // Возвращаем false при любом исключении.
        }
    }


    /// <summary>
    /// Выполняет сравнение двух чисел, представленных строками.
    /// <br/>Проверяет, являются ли строки допустимыми числами, и затем сравнивает их с использованием указанного оператора сравнения.
    /// </summary>
    /// <param name="firstNumStr">Первая строка, представляющая число для сравнения.</param>
    /// <param name="comparisonOperator">Оператор сравнения, который определяет, что нужно проверить: больше, меньше, равно или не равно.</param>
    /// <param name="secondNumStr">Вторая строка, представляющая число для сравнения.</param>
    /// <returns>
    /// Возвращает <c>true</c>, если условие сравнения истинно, иначе <c>false></c>.
    /// <br/>Если строки не являются допустимыми числами, выбрасывает исключение <see cref="ArgumentException"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если хотя бы одна из строк не представляет допустимое число.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если передан некорректный оператор сравнения.
    /// </exception>
    public static bool IsComparisonValid(string firstNumStr, ComparisonType comparisonOperator, string secondNumStr)
    {
        // Проверяем, что обе строки представляют допустимые числа.
        if (DetermineNumberType(firstNumStr) == NumberType.Invalid) throw new ArgumentException($"'{firstNumStr}' is not a valid number.");
        if (DetermineNumberType(secondNumStr) == NumberType.Invalid) throw new ArgumentException($"'{secondNumStr}' is not a valid number.");

        // Используем существующий метод для сравнения строковых чисел.
        string comparisonResult = CompareDecimalStrings(firstNumStr, secondNumStr);

        // Сравниваем результат с переданным знаком и возвращаем true, если условие сравнения истинно.
        switch (comparisonOperator)
        {
            case ComparisonType.GreaterThan:
                return comparisonResult == ">" ? true : false;
            case ComparisonType.LessThan:
                return comparisonResult == "<" ? true : false;
            case ComparisonType.EqualTo:
                return comparisonResult == "=" ? true : false;
            case ComparisonType.NotEqualTo:
                return comparisonResult != "=" ? true : false; // Если числа не равны, возвращаем true.
            default:
                throw new ArgumentException("Invalid comparison sign.");
        }
    }

    /// <summary>
    /// Проверяет, может ли строка быть преобразована в тип <see cref="SqlDecimal"/>.
    /// </summary>
    /// <param name="numStr">Строка, представляющая число.</param>
    /// <returns>
    /// <c>true</c>, если строка является допустимым числом и его длина (без учёта знаков и разделителей) не превышает 38 символов.
    /// <br/><c>false</c> в противном случае.
    /// </returns>
    public static bool IsValidSqlDecimal(string numStr)
    {
        // Проверяем, является ли строка числом
        if (DetermineNumberType(numStr) == NumberType.Invalid)
            return false;

        // Очищаем строку от лишних символов
        string cleanedNumber = numStr
            .Replace("+", string.Empty)
            .Replace("-", string.Empty)
            .Replace(".", string.Empty)
            .Replace(",", string.Empty);

        // Проверяем длину строки (ограничение для SqlDecimal)
        return true;
    }

    /// <summary>
    /// Определяет тип числа, представленное строкой.
    /// Проверяет, является ли строка целым числом, дробным числом или некорректным значением.
    /// </summary>
    /// <param name="numStr">Строка, представляющая число для проверки.</param>
    /// <returns>Возвращает <see cref="NumberType"/> для указанного числа.</returns>
    /// <remarks>
    /// Используются регулярные выражения для проверки форматов целых и дробных чисел. 
    /// <br/>Если строка не соответствует ни одному из форматов, возвращается <see cref="NumberType.Invalid"/>.
    /// </remarks>
    public static NumberType DetermineNumberType(string numStr)
    {
        // Регулярное выражение для целого числа (включает опциональный знак).
        string integerPattern = @"^[+-]?\d+$";

        // Регулярное выражение для дробного числа (включает опциональный знак и точку или запятую).
        string decimalPattern = @"^[+-]?\d+(\.\d+|\d*,\d+)$";       // Обработка и точки, и запятой.

        // Если строка соответствует целому числу, возвращаем NumberType.Integer.
        if (Regex.IsMatch(numStr, integerPattern))
            return NumberType.Integer;
        // Если строка соответствует дробному числу, возвращаем NumberType.Decimal.
        else if (Regex.IsMatch(numStr, decimalPattern))
            return NumberType.Decimal;
        // Если строка не соответствует ни одному из форматов, возвращаем NumberType.Invalid.
        else
            return NumberType.Invalid;
    }


    // ---------------------------------------------------------------------------------------
    // Сравнивает два десятичных числа, представленных строками, с учетом их целых и дробных частей.
    // Метод поддерживает числа, записанные с разделителем в виде точки или запятой, а также знаки "+" и "-".
    // ---------------------------------------------------------------------------------------
    private static string CompareDecimalStrings(string str1, string str2)
    {
        // Заменяем запятую на точку, если таковая имеется, для корректного сравнения,
        // а также убираем символ "+" (если он присутствует) для нормализации чисел.
        str1 = str1.Replace(',', '.').Replace("+", "");
        str2 = str2.Replace(',', '.').Replace("+", "");

        // Убираем знак из чисел, если они равны 0 или -0.
        // Если оба числа равны 0 и -0, они считаются равными.
        if (str1 == "0" && str2 == "-0" || str1 == "-0" && str2 == "0") return "=";

        // Разделяем на целую и дробную части.
        string[] parts1 = str1.Split('.');
        string[] parts2 = str2.Split('.');

        // Сравниваем целые части
        string integerComparison = CompareIntegerStrings(parts1[0], parts2[0]);
        if (integerComparison != "=") return integerComparison;

        // Если целые части равны, сравниваем дробные части.
        string fractional1 = parts1.Length > 1 ? parts1[1] : "0";
        string fractional2 = parts2.Length > 1 ? parts2[1] : "0";

        // Дополняем нулями, если дробные части имеют разную длину.
        int maxLength = Math.Max(fractional1.Length, fractional2.Length);
        fractional1 = fractional1.PadRight(maxLength, '0');
        fractional2 = fractional2.PadRight(maxLength, '0');

        // Сравниваем дробные части как положительные целые числа.
        return ComparePositiveIntegerStrings(fractional1, fractional2);
    }


    // ---------------------------------------------------------------------------------------
    // Сравнивает два целых числа, представленных строками, с учетом их знаков.
    // Поддерживаются положительные и отрицательные числа, а также "0" и "-0".
    // Метод не работает с дробными числами.
    // ---------------------------------------------------------------------------------------
    private static string CompareIntegerStrings(string str1, string str2)
    {
        // Удаляем все нули, идущие в начале числа, из строк.
        str1 = str1.TrimStart('0').Replace("+", "");
        str2 = str2.TrimStart('0').Replace("+", "");

        // Убираем знак из чисел, если они равны 0 или -0.
        // Если оба числа равны "0" или "-0", они считаются равными.
        if (str1 == "0" && str2 == "-0" || str1 == "-0" && str2 == "0") return "=";

        // Проверяем, является ли число отрицательным.
        bool isNegative1 = str1.StartsWith("-");
        bool isNegative2 = str2.StartsWith("-");

        // Если одно число отрицательное, а другое положительное, положительное всегда больше.
        if (!isNegative1 && isNegative2) return ">";        // Первое число больше.
        if (isNegative1 && !isNegative2) return "<";        // Первое число меньше.

        // Если оба числа отрицательные, сравниваем их модули.
        // Результат инвертируется, так как числа отрицательные.
        if (isNegative1 && isNegative2)
        {
            str1 = str1.TrimStart('-');
            str2 = str2.TrimStart('-');

            var cmpResult = ComparePositiveIntegerStrings(str1, str2);
            if (cmpResult == ">") return "<";
            if (cmpResult == "<") return ">";
        }
        else if (!isNegative1 && !isNegative2)
        {
            // Если оба числа положительные, сравниваем их как положительные.
            return ComparePositiveIntegerStrings(str1, str2);
        }
        return "=";
    }


    // ---------------------------------------------------------------------------------------
    // Сравнивает два положительных целых числа, представленных строками.
    // Метод предполагает, что строки содержат только цифры и не имеют знаков "+" или "-".
    // Дробные числа не поддерживаются.
    // ---------------------------------------------------------------------------------------
    private static string ComparePositiveIntegerStrings(string str1, string str2)
    {
        // Удаляем все нули, идущие в начале числа, из строк.
        str1 = str1.TrimStart('0');
        str2 = str2.TrimStart('0');

        // Сравниваем длину строк: более длинная строка всегда больше.                                      
        if (str1.Length > str2.Length) return ">";      // Первое число больше.         
        if (str1.Length < str2.Length) return "<";      // Второе число больше.

        // Если строки одинаковой длины, сравниваем их символы по порядку.
        for (int i = 0; i < str1.Length; i++)
        {
            if (str1[i] > str2[i]) return ">";          // Первое число больше.
            else if (str1[i] < str2[i]) return "<";     // Второе число больше.
        }

        return "=";                                     // Если все символы одинаковы, возвращаем "=", числа равны.
    }
}