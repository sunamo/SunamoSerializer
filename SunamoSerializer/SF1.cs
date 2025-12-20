// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoSerializer;
public static partial class SF
{
    /// <summary>
    ///     If index won't founded, return null.
    /// </summary>
    /// <param name = "element"></param>
    /// <param name = "line"></param>
    public static string GetElementAtIndexFile(string file, int element, int line)
    {
        var elements = GetAllElementsFile(file);
        return GetElementAtIndex(elements, element, line);
    }

    /// <summary>
    ///     G null if first element on any lines A2 dont exists
    /// </summary>
    /// <param name = "file"></param>
    /// <param name = "element"></param>
    public static List<string> GetFirstWhereIsFirstElement(string file, string element)
    {
        var elementsLines = GetAllElementsFile(file);
        for (var i = 0; i < elementsLines.Count; i++)
            if (elementsLines[i][0] == element)
                return elementsLines[i];
        return null;
    }

    /// <summary>
    ///     G null if first element on any lines A2 dont exists
    /// </summary>
    /// <param name = "file"></param>
    /// <param name = "element"></param>
    public static List<string> GetLastWhereIsFirstElement(string file, string element)
    {
        var elementsLines = GetAllElementsFile(file);
        for (var i = elementsLines.Count - 1; i >= 0; i--)
            if (elementsLines[i][0] == element)
                return elementsLines[i];
        return null;
    }

    /// <summary>
    ///     Read text with first delimitech which automatically delimite
    /// </summary>
    /// <param name = "fileNameOrPath"></param>
    public static void ReadFileOfSettingsOther(string fileNameOrPath, Func<string, string> appDataCiReadFileOfSettingsOther)
    {
        // COmmented, app data not should be in *.web. pass directly as arg
        List<string> lines = null;
        lines = SHGetLines.GetLines(appDataCiReadFileOfSettingsOther(fileNameOrPath));
        if (lines.Count > 1)
        {
            int delimiterInt;
            if (int.TryParse(lines[0], out delimiterInt))
                separatorString = ((char)delimiterInt).ToString();
        }
    }

    public static async Task WriteAllElementsToFile(string VybranySouborLogu, List<string>[] parameter)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in parameter)
            stringBuilder.AppendLine(PrepareToSerialization(item));
        await File.WriteAllTextAsync(VybranySouborLogu, stringBuilder.ToString());
    }

    /// <summary>
    ///     Without last |
    ///     DateTime is format with DTHelperEn.ToString
    /// </summary>
    /// <param name = "o"></param>
    public static string PrepareToSerialization(params string[] o)
    {
        return PrepareToSerialization(o.ToList(), dDeli);
    }

    public static string PrepareToSerialization(List<string> list, string separator = "|")
    {
        if (separator == replaceForSeparatorString)
            throw new Exception("replaceForSeparatorString is the same as separator");
        CA.Replace(list, separator, replaceForSeparatorString);
        CA.Replace(list, Environment.NewLine, "");
        CA.Trim(list);
        var vr = string.Join(separator, list);
        return vr;
    }

    ///// <summary>
    ///// Return without last
    ///// DateTime is serialize always in english format
    ///// Opposite method: DTHelperEn.ToString<>DTHelperEn.ParseDateTimeUSA
    ///// </summary>
    ///// <param name="pr"></param>
    //public static string PrepareToSerialization2(params string[] pr)
    //{
    //    var ts = new List<string>(pr);
    //    return PrepareToSerializationWorker(ts, true, separatorString);
    //}
    /// <summary>
    ///     Get all elements from A1
    ///     A2 byl object ale dal jsem ho jako string
    ///     nemůžu to dávat jako object protože SHSplit.Split musí být typový. Např. když mám allWhiteChars který je List
    ///     <object> a po přenesení do params string[] mi vytvoří new string[]{}
    /// </summary>
    /// <param name = "var"></param>
    public static List<string> GetAllElementsLine(string var, string oddelovaciZnak = null)
    {
        if (oddelovaciZnak == null)
            oddelovaciZnak = "|";
        // Musí tu být none, protože pak když někde nic nebylo, tak mi to je nevrátilo a progran vyhodil IndexOutOfRangeException
        return SHSplit.Split(var, oddelovaciZnak);
    }

    /// <summary>
    ///     In result A1 is not
    /// </summary>
    /// <param name = "file"></param>
    /// <param name = "hlavicka"></param>
    /// <param name = "oddelovaciZnak"></param>
    public static (List<string> header, List<List<string>> rows) GetAllElementsFileAdvanced(string file, string oddelovaciZnak = "|")
    {
        if (oddelovaciZnak == null)
            oddelovaciZnak = "|";
        var hlavicka = new List<string>();
        var oz = oddelovaciZnak;
        var vr = new List<List<string>>();
        // Sync protože mám v deklaraci out
        var lines = File.ReadAllLines(file).ToList();
        CA.Trim(lines);
        if (lines.Count > 0)
        {
            hlavicka = GetAllElementsLine(lines[0], oddelovaciZnak);
            var musiByt = lines[0].Split(new[] { oz }, StringSplitOptions.None).Length - 1;
            //int nalezeno = 0;
            var jedenRadek = new StringBuilder();
            for (var i = 1; i < lines.Count; i++)
            {
                if (lines[i].Trim().Length == 0)
                    continue;
                //nalezeno += SH.OccurencesOfStringIn(lines[i], oz);
                jedenRadek.AppendLine(lines[i]);
                //if (nalezeno == musiByt)
                //{
                //nalezeno = 0;
                var columns = GetAllElementsLine(jedenRadek.ToString(), oddelovaciZnak);
                CA.Trim(columns);
                jedenRadek.Clear();
                vr.Add(columns);
            //}
            }
        }

        return (hlavicka, vr);
    }
}