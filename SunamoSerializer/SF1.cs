namespace SunamoSerializer;

public static partial class SF
{
    /// <summary>
    /// Get element at index file.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="elementIndex">The element index.</param>
    /// <param name="lineIndex">The line index.</param>
    /// <returns>The get element at index file result.</returns>
    public static string? GetElementAtIndexFile(string filePath, int elementIndex, int lineIndex)
    {
        var list = GetAllElementsFile(filePath);
        return GetElementAtIndex(list, elementIndex, lineIndex);
    }

    /// <summary>
    /// Get first where is first element.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="firstElement">The first element.</param>
    /// <returns>The get first where is first element result.</returns>
    public static List<string>? GetFirstWhereIsFirstElement(string filePath, string firstElement)
    {
        var list = GetAllElementsFile(filePath);
        for (var i = 0; i < list.Count; i++)
            if (list[i][0] == firstElement)
                return list[i];
        return null;
    }

    /// <summary>
    /// Get last where is first element.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="firstElement">The first element.</param>
    /// <returns>The get last where is first element result.</returns>
    public static List<string>? GetLastWhereIsFirstElement(string filePath, string firstElement)
    {
        var list = GetAllElementsFile(filePath);
        for (var i = list.Count - 1; i >= 0; i--)
            if (list[i][0] == firstElement)
                return list[i];
        return null;
    }

    /// <summary>
    /// Read file of settings other.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="readFileFunc">The read file func.</param>
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
    /// Write all elements to file.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="array">The array.</param>
    public static async Task WriteAllElementsToFile(string filePath, List<string>[] array)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in array)
            stringBuilder.AppendLine(PrepareToSerialization(item));
        await FileAsync.WriteAllTextAsync(filePath, stringBuilder.ToString());
    }

    /// <summary>
    /// Prepare to serialization.
    /// </summary>
    /// <param name="elements">The elements.</param>
    /// <returns>The prepare to serialization result.</returns>
    public static string PrepareToSerialization(params string[] elements)
    {
        return PrepareToSerialization(elements.ToList(), DefaultDelimiter);
    }

    /// <summary>
    /// Prepare to serialization.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="separator">The separator.</param>
    /// <returns>The prepare to serialization result.</returns>
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
    /// Get all elements line.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="separator">The separator.</param>
    /// <returns>The get all elements line result.</returns>
    public static List<string> GetAllElementsLine(string text, string? separator = null)
    {
        if (separator == null)
            separator = "|";
        return SHSplit.Split(text, separator);
    }

    /// <summary>
    /// Initializes a new instance of the get all elements file advanced class.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="rows">The rows.</param>
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
