namespace SunamoSerializer._sunamo;

/// <summary>
/// Provides collection and array manipulation utilities.
/// </summary>
internal class CA
{
    static string Replace(string text, string what, string replacement)
    {
        return text.Replace(what, replacement);
    }

    /// <summary>
    /// Replaces all occurrences of a substring in each element of a string list. Modifies the list in place.
    /// </summary>
    internal static void Replace(List<string> list, string what, string replacement)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = Replace(list[i], what, replacement);
        }
    }

    /// <summary>
    /// Trims whitespace from each element in a string list. Modifies the list in place.
    /// </summary>
    internal static List<string> Trim(List<string> list)
    {
        for (var i = 0; i < list.Count; i++) list[i] = list[i].Trim();

        return list;
    }
}
