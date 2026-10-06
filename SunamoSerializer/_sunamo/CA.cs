namespace SunamoSerializer._sunamo;

internal class CA
{
    static string Replace(string text, string what, string replacement)
    {
        return text.Replace(what, replacement);
    }

    internal static void Replace(List<string> list, string what, string replacement)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = Replace(list[i], what, replacement);
        }
    }

    internal static List<string> Trim(List<string> list)
    {
        for (var i = 0; i < list.Count; i++) list[i] = list[i].Trim();

        return list;
    }
}
