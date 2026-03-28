namespace SunamoSerializer._sunamo.SunamoExceptions;

/// <summary>
/// Provides utility methods for throwing exceptions with additional debugging support.
/// </summary>
internal partial class ThrowEx
{
    /// <summary>
    /// Breaks into debugger and throws a custom exception with the specified message.
    /// </summary>
    internal static void Custom(string message)
    {
        Debugger.Break();
        throw new Exception(message);
    }
}
