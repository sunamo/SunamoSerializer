namespace SunamoSerializer._sunamo.SunamoStringSplit;

/// <summary>
/// Provides string splitting utilities.
/// </summary>
internal class SHSplit
{
    /// <summary>
    /// Splits a string by the specified delimiters, removing empty entries.
    /// </summary>
    internal static List<string> Split(string text, params string[] delimiters)
    {
        return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
