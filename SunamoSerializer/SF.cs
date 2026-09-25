namespace SunamoSerializer;

public static partial class SF
{
    public const string ReplaceForSeparatorString = "_";

    private static readonly SerializeContentArgs serializeContentArgs = new();

    public static readonly char ReplaceForSeparatorChar = '_';

    public static string DefaultDelimiter { get; set; } = "|";

    public static string SeparatorString { get => serializeContentArgs.SeparatorString; set => serializeContentArgs.SeparatorString = value; }

    public static int KeyCodeSeparator => serializeContentArgs.SeparatorChar;

    public static char SeparatorChar => serializeContentArgs.SeparatorChar;

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

    public static List<List<string>> GetAllElementsFile(string filePath)
    {
        return GetAllElementsFile(filePath, "|");
    }

    public static List<string> RemoveComments(List<string> list)
    {
        list = list.Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
        list = list.Where(item => !item.StartsWith('#')).ToList();
        return list;
    }

    public static List<List<string>> GetAllElementsFile(string filePath, string separator = "|")
    {
        var (header, rows) = GetAllElementsFileAdvanced(filePath, separator);
        if (header.Count > 0)
            rows.Insert(0, header);
        return rows;
    }

    public static
        async Task
    Dictionary<TKey, TValue>(string filePath, Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in dictionary)
            stringBuilder.AppendLine(PrepareToSerialization(item.Key.ToString()!, item.Value?.ToString() ?? ""));
        await FileAsync.WriteAllTextAsync(filePath, stringBuilder.ToString());
    }

    public static async Task WriteAllElementsToFile<TKey, TValue>(string filePath, Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var list = ListFromDictionary(dictionary);
        await WriteAllElementsToFile(filePath, list);
    }

    public static async Task WriteAllElementsToFile(string filePath, List<List<string>> list)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in list)
            stringBuilder.AppendLine(PrepareToSerialization(item));
        await FileAsync.WriteAllTextAsync(filePath, stringBuilder.ToString());
    }

    public static List<List<string>> ListFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var result = new List<List<string>>();
        foreach (var item in dictionary)
        {
            result.Add([item.Key.ToString()!, item.Value?.ToString() ?? ""]);
        }

        return result;
    }

    public static
        async Task
    DictionaryAppend(string filePath, Dictionary<int, string> dictionary)
    {
        var entries = ListFromDictionary(dictionary);
        var normalizedDictionary = ToDictionary<int, string>(entries);
        var stringBuilder = new StringBuilder();
        foreach (var item in normalizedDictionary)
            stringBuilder.AppendLine(PrepareToSerialization(item.Key.ToString(), item.Value));
        await FileAsync.AppendAllTextAsync(filePath, stringBuilder + Environment.NewLine);
    }

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

    public static
        async Task<List<List<string>>>
    AppendAllText(string filePath, string line)
    {
        var content = (
            await FileAsync.ReadAllLinesAsync(filePath)
            ).ToList();
        CA.Trim(content);
        content.Add(line);
        var result = GetAllElementsLines(content);
        await FileAsync.WriteAllLinesAsync(filePath, content);
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
