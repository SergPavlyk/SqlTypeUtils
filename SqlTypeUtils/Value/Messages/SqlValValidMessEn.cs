namespace SqlTypeUtils.Value.Messages;

/// <summary>
/// Provides validation messages in English for SQL types. Implements the <see cref="ISqlValueValidatorMsgProvider"/> interface.
/// </summary>
public class SqlValValidMessEn : ISqlValueValidatorMsgProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlValValidMessEn"/> class, which provides validation messages in English for SQL types.
    /// </summary>
    public SqlValValidMessEn() { }


    public string DotNetTypeMsg { get; set; } = ".NET";
    public string SqlTypeMsg { get; set; } = "SQL";

    public string SuccessMsg { get; set; } = "OK";                                                                                          // SqlBoolean, SqlNumeric, SqlUniqueIdentifier, SqlBinary
    public string MissingValueMsg { get; set; } = "Value is missing";                                                                       // SqlBoolean, SqlNumeric, SqlUniqueIdentifier, SqlBinary
    public string WhitespaceValueMsg { get; set; } = "Value contains only whitespace";                                                      // SqlBoolean, SqlNumeric, SqlUniqueIdentifier, SqlBinary
    public string InvalidValueMsg { get; set; } = "Value '{0}' does not match the {1} type";                                                // SqlBoolean, SqlNumeric, SqlDateTime

    public string RangeLowMsg { get; set; } = "Value '{0}' is below the allowed range ({1} - {2}) for type {3}";                            // SqlNumeric, SqlDateTime
    public string RangeHighMsg { get; set; } = "Value '{0}' exceeds the allowed range ({1} - {2}) for type {3}";                            // SqlNumeric, SqlDateTime

    public string EncodingMismatchMsg { get; set; } = "String does not match the expected encoding {0}";                                    // SqlString
    public string LengthExceedMsg { get; set; } = "String length exceeds the allowed maximum - {0}";                                        // SqlString
    public string LengthShortMsg { get; set; } = "String length is less than the allowed minimum - {0}";                                    // SqlString

    public string OddLengthMsg { get; set; } = "Value '{0}' must have an even number of characters for type {1}";                           // SqlBinary
    public string ByteLengthTooSmallMsg { get; set; } = "Byte array length is less than the allowed minimum - {0}";                         // SqlBinary
    public string ByteLengthTooLargeMsg { get; set; } = "Byte array length exceeds the allowed maximum - {0}";                              // SqlBinary
    public string InvalidHexCharsMsg { get; set; } = "Value '{0}' contains invalid characters for the hexadecimal format of type {1}";      // SqlBinary
}