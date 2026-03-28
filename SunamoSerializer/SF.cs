namespace SunamoSerializer;

/// <summary>
/// Provides serialization and deserialization utilities for text-based data files with delimited content.
/// </summary>
public static partial class SF
{
    /// <summary>
    /// Replacement string used when separator character is found in content.
    /// </summary>
    public const string ReplaceForSeparatorString = "_";

    private static readonly SerializeContentArgs serializeContentArgs = new();

    /// <summary>
    /// Replacement character used when separator character is found in content.
    /// </summary>
    public static readonly char ReplaceForSeparatorChar = '_';

    /// <summary>
    /// Gets or sets the default delimiter used for serialization.
    /// </summary>
    public static string DefaultDelimiter { get; set; } = "|";

    /// <summary>
    /// Gets or sets the separator string used for serialization.
    /// </summary>
    public static string SeparatorString { get => serializeContentArgs.SeparatorString; set => serializeContentArgs.SeparatorString = value; }

    /// <summary>
    /// Gets the key code of the separator character.
    /// </summary>
    public static int KeyCodeSeparator => serializeContentArgs.SeparatorChar;

    /// <summary>
    /// Gets the separator character. Must be property to avoid inconsistency when changing value.
    /// </summary>
    public static char SeparatorChar => serializeContentArgs.SeparatorChar;

    /// <summary>
    /// Parses a line and ensures it contains exactly the required number of elements, padding with empty strings if needed.
    /// </summary>
    /// <param name="text">The input line to parse.</param>
    /// <param name="requiredCount">The required number of elements.</param>
    /// <returns>List of parsed elements with exactly requiredCount items.</returns>
    public static List<string> ParseUpToRequiredElementsLine(string text, int requiredCount)
    {
        var elements = GetAllElementsLine(text);
        if (elements.Count > requiredCount)
            throw new Exception($"elements have {elements.Count} elements, max is {requiredCount}");
        if (elements.Count < requiredCount)
            for (var i = elements.Count - 1; i < requiredCount; i++)
                elements.Add(string.Empty);
        return elements;
    }

    /// <summary>
    /// Converts a list of string element lists into a dictionary by parsing each pair.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="list">The list of string element lists where each inner list should have exactly 2 elements.</param>
    /// <returns>Dictionary populated from the parsed elements.</returns>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(List<List<string>> list) where TKey : notnull
    {
        var keyParser = (Func<string, TKey>)BTS.MethodForParse<TKey>()!;
        var valueParser = (Func<string, TValue>)BTS.MethodForParse<TValue>()!;
        var dictionary = new Dictionary<TKey, TValue>();
        foreach (var item in list)
        {
            if (item.Count != 2)
            {
                continue;
            }

            var key = keyParser.Invoke(item[0]);
            var value = valueParser.Invoke(item[1]);
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    /// <summary>
    /// Gets all elements from all lines of a file, with header included as first row.
    /// </summary>
    /// <param name="filePath">Path to the file to read.</param>
    /// <returns>List of element lists, where each inner list represents elements from one line.</returns>
    public static List<List<string>> GetAllElementsFile(string filePath)
    {
        return GetAllElementsFile(filePath, "|");
    }

    /// <summary>
    /// Removes comment lines (starting with #) and whitespace-only lines from the input.
    /// </summary>
    /// <param name="list">The list of lines to filter.</param>
    /// <returns>Filtered list with comments and empty lines removed.</returns>
    public static List<string> RemoveComments(List<string> list)
    {
        list = list.Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
        list = list.Where(item => !item.StartsWith('#')).ToList();
        return list;
    }

    /// <summary>
    /// Gets all elements from all lines of a file using the specified separator.
    /// </summary>
    /// <param name="filePath">Path to the file to read.</param>
    /// <param name="separator">The separator string used between elements.</param>
    /// <returns>List of element lists, where each inner list represents elements from one line.</returns>
    public static List<List<string>> GetAllElementsFile(string filePath, string separator = "|")
    {
        var (header, rows) = GetAllElementsFileAdvanced(filePath, separator);
        if (header.Count > 0)
            rows.Insert(0, header);
        return rows;
    }

    /// <summary>
    /// Writes a dictionary to a file in serialized format.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="filePath">Path to the output file.</param>
    /// <param name="dictionary">The dictionary to serialize and write.</param>
    public static
#if ASYNC
        async Task
#else
    void
#endif
    Dictionary<TKey, TValue>(string filePath, Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in dictionary)
            stringBuilder.AppendLine(PrepareToSerialization(item.Key.ToString()!, item.Value?.ToString() ?? ""));
#if ASYNC
        await
#endif
        File.WriteAllTextAsync(filePath, stringBuilder.ToString());
    }

    /// <summary>
    /// Writes all elements from a dictionary to a file in serialized format.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="filePath">Path to the output file.</param>
    /// <param name="dictionary">The dictionary to serialize and write.</param>
    public static async Task WriteAllElementsToFile<TKey, TValue>(string filePath, Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var list = ListFromDictionary(dictionary);
        await WriteAllElementsToFile(filePath, list);
    }

    /// <summary>
    /// Writes all element lists to a file in serialized format.
    /// </summary>
    /// <param name="filePath">Path to the output file.</param>
    /// <param name="list">The list of element lists to serialize and write.</param>
    public static async Task WriteAllElementsToFile(string filePath, List<List<string>> list)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in list)
            stringBuilder.AppendLine(PrepareToSerialization(item));
        await File.WriteAllTextAsync(filePath, stringBuilder.ToString());
    }

    /// <summary>
    /// Converts a dictionary into a list of string lists for serialization.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="dictionary">The dictionary to convert.</param>
    /// <returns>List of string lists where each inner list contains key and value as strings.</returns>
    public static List<List<string>> ListFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var result = new List<List<string>>();
        foreach (var item in dictionary)
        {
            result.Add([item.Key.ToString()!, item.Value?.ToString() ?? ""]);
        }

        return result;
    }

    /// <summary>
    /// Appends a dictionary to an existing file in serialized format.
    /// </summary>
    /// <param name="filePath">Path to the file to append to.</param>
    /// <param name="dictionary">The dictionary to serialize and append.</param>
    public static
#if ASYNC
        async Task
#else
    void
#endif
    DictionaryAppend(string filePath, Dictionary<int, string> dictionary)
    {
        var entries = ListFromDictionary(dictionary);
        var normalizedDictionary = ToDictionary<int, string>(entries);
        var stringBuilder = new StringBuilder();
        foreach (var item in normalizedDictionary)
            stringBuilder.AppendLine(PrepareToSerialization(item.Key.ToString(), item.Value));
#if ASYNC
        await
#endif
        File.AppendAllTextAsync(filePath, stringBuilder + Environment.NewLine);
    }

    /// <summary>
    /// Gets the element at specified indices from a parsed file.
    /// </summary>
    private static string? GetElementAtIndex(List<List<string>> list, int elementIndex, int lineIndex)
    {
        if (list.Count > lineIndex)
        {
            var lineElements = list[lineIndex];
            if (lineElements.Count > elementIndex)
                return lineElements[elementIndex];
        }

        return null;
    }

    /// <summary>
    /// Appends a line to a file and returns all parsed elements.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="line">The line to append.</param>
    /// <returns>All elements from the file after appending.</returns>
    public static
#if ASYNC
        async Task<List<List<string>>>
#else
    List<List<string>>
#endif
    AppendAllText(string filePath, string line)
    {
        var content = (await File.ReadAllLinesAsync(filePath)).ToList();
        CA.Trim(content);
        content.Add(line);
        var result = GetAllElementsLines(content);
#if ASYNC
        await
#endif
        File.WriteAllLinesAsync(filePath, content);
        return result;
    }

    private static List<List<string>> GetAllElementsLines(List<string> list)
    {
        list = RemoveComments(list);
        var result = new List<List<string>>();
        foreach (var item in list)
            if (!string.IsNullOrWhiteSpace(item))
                result.Add(GetAllElementsLine(item));
        return result;
    }
}
