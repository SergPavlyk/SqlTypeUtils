namespace SqlTypeUtils.Value.Messages;

/// <summary>
/// Интерфейс для предоставления сообщений валидации.
/// <br/>Определяет стандартный набор сообщений для различных типов валидации.
/// </summary>
public interface ISqlValueValidatorMsgProvider
{
    string DotNetTypeMsg { get; set; }
    string SqlTypeMsg { get; set; }

    string SuccessMsg { get; set; }
    string MissingValueMsg { get; set; }
    string WhitespaceValueMsg { get; set; }
    string InvalidValueMsg { get; set; }

    string RangeLowMsg { get; set; }
    string RangeHighMsg { get; set; }

    string EncodingMismatchMsg { get; set; }
    string LengthExceedMsg { get; set; }
    string LengthShortMsg { get; set; }

    string OddLengthMsg { get; set; }
    string ByteLengthTooSmallMsg { get; set; }
    string ByteLengthTooLargeMsg { get; set; }
    string InvalidHexCharsMsg { get; set; }
}
