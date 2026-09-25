namespace SunamoSerializer;

/// <summary>
/// Provides additional serialization and deserialization utilities (partial class extension).
/// </summary>
public static partial class SF
{
    /// <summary>
    /// Gets the element at specified indices from a file. Returns null if not found.
    /// </summary>
    /// <param name="filePath">Path to the file to read.</param>
    /// <param name="elementIndex">The element index within a line.</param>
    /// <param name="lineIndex">The line index in the file.</param>
    /// <returns>The element at the specified position, or null if not found.</returns>
    public static string? GetElementAtIndexFile(string filePath, int elementIndex, int lineIndex)
    {
        var list = GetAllElementsFile(filePath);
        return GetElementAtIndex(list, elementIndex, lineIndex);
    }

    /// <summary>
    /// Gets the first line where the first element matches the specified value. Returns null if not found.
    /// </summary>
    /// <param name="filePath">Path to the file to read.</param>
    /// <param name="firstElement">The value to match against the first element of each line.</param>
    /// <returns>The matching line as a list of elements, or null if not found.</returns>
    public static List<string>? GetFirstWhereIsFirstElement(string filePath, string firstElement)
    {
        var list = GetAllElementsFile(filePath);
        for (var i = 0; i < list.Count; i++)
            if (list[i][0] == firstElement)
                return list[i];
        return null;
    }

    /// <summary>
    /// Gets the last line where the first element matches the specified value. Returns null if not found.
    /// </summary>
    /// <param name="filePath">Path to the file to read.</param>
    /// <param name="firstElement">The value to match against the first element of each line.</param>
    /// <returns>The matching line as a list of elements, or null if not found.</returns>
    public static List<string>? GetLastWhereIsFirstElement(string filePath, string firstElement)
    {
        var list = GetAllElementsFile(filePath);
        for (var i = list.Count - 1; i >= 0; i--)
            if (list[i][0] == firstElement)
                return list[i];
        return null;
    }

    /// <summary>
    /// Reads a settings file and extracts the separator from its first line.
    /// </summary>
    /// <param name="filePath">Path or name of the settings file.</param>
    /// <param name="readFileFunc">Function that resolves the file path and reads its content.</param>
    public static void ReadFileOfSettingsOther(string filePath, Func<string, string> readFileFunc)
    {
        var lines = SHGetLines.GetLines(readFileFunc(filePath));
        if (lines.Count > 1)
        {
            if (int.TryParse(lines[0], out int delimiterInt))
                SeparatorString = ((char)delimiterInt).ToString();
        }
    }

    /// <summary>
    /// Writes all element arrays to a file in serialized format.
    /// </summary>
    /// <param name="filePath">Path to the output file.</param>
    /// <param name="array">Array of element lists to serialize and write.</param>
    public static async Task WriteAllElementsToFile(string filePath, List<string>[] array)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in array)
            stringBuilder.AppendLine(PrepareToSerialization(item));
        await FileAsync.WriteAllTextAsync(filePath, stringBuilder.ToString());
    }

    /// <summary>
    /// Serializes string elements into a single delimited line using the default delimiter.
    /// </summary>
    /// <param name="elements">The elements to serialize.</param>
    /// <returns>A delimited string containing all elements.</returns>
    public static string PrepareToSerialization(params string[] elements)
    {
        return PrepareToSerialization(elements.ToList(), DefaultDelimiter);
    }

    /// <summary>
    /// Serializes a list of strings into a single delimited line.
    /// </summary>
    /// <param name="list">The list of strings to serialize.</param>
    /// <param name="separator">The separator string to use between elements.</param>
    /// <returns>A delimited string containing all elements.</returns>
    public static string PrepareToSerialization(List<string> list, string separator = "|")
    {
        if (separator == ReplaceForSeparatorString)
            throw new Exception("ReplaceForSeparatorString is the same as separator");
        CA.Replace(list, separator, ReplaceForSeparatorString);
        CA.Replace(list, Environment.NewLine, "");
        CA.Trim(list);
        var result = string.Join(separator, list);
        return result;
    }

    /// <summary>
    /// Gets all elements from a single line using the specified separator.
    /// </summary>
    /// <param name="text">The line to parse.</param>
    /// <param name="separator">The separator string used between elements. Defaults to "|".</param>
    /// <returns>List of parsed elements from the line.</returns>
    public static List<string> GetAllElementsLine(string text, string? separator = null)
    {
        if (separator == null)
            separator = "|";
        return SHSplit.Split(text, separator);
    }

    /// <summary>
    /// Gets all elements from a file, returning header and data rows separately.
    /// </summary>
    /// <param name="filePath">Path to the file to read.</param>
    /// <param name="separator">The separator string used between elements. Defaults to "|".</param>
    /// <returns>A tuple containing the header elements and a list of row element lists.</returns>
    public static (List<string> header, List<List<string>> rows) GetAllElementsFileAdvanced(string filePath, string separator = "|")
    {
        if (separator == null)
            separator = "|";
        var header = new List<string>();
        var result = new List<List<string>>();
        var lines = File.ReadAllLines(filePath).ToList();
        CA.Trim(lines);
        if (lines.Count > 0)
        {
            header = GetAllElementsLine(lines[0], separator);
            var singleLineBuilder = new StringBuilder();
            for (var i = 1; i < lines.Count; i++)
            {
                if (lines[i].Trim().Length == 0)
                    continue;
                singleLineBuilder.AppendLine(lines[i]);
                var elements = GetAllElementsLine(singleLineBuilder.ToString(), separator);
                CA.Trim(elements);
                singleLineBuilder.Clear();
                result.Add(elements);
            }
        }

        return (header, result);
    }
}
