namespace SunamoSerializer;

public static class SF
{
    public const string replaceForSeparatorString = "_";
    private static readonly SerializeContentArgs s_contentArgs = new();

    private static Type type = typeof(SF);
    public static readonly char replaceForSeparatorChar = '_';
    public static string dDeli = "|";

    static SF()
    {
        s_contentArgs.separatorString = "|";
    }



    public static string separatorString
    {
        get => s_contentArgs.separatorString;

        set => s_contentArgs.separatorString = value;
    }

    public static int keyCodeSeparator => s_contentArgs.separatorChar;

    /// <summary>
    ///     Must be property - I can forget change value on three occurences.
    /// </summary>
    public static char separatorChar => s_contentArgs.separatorChar;

    //CASH
    public static List<string> ParseUpToRequiredElementsLine(string input, int requiredCount)
    {
        var p = GetAllElementsLine(input);
        if (p.Count > requiredCount)
            throw new Exception($"p have {p.Count} elements, max is {requiredCount}");
        if (p.Count < requiredCount)
            for (var i = p.Count - 1; i < requiredCount; i++)
                p.Add(string.Empty);

        return p;
    }

    public static Dictionary<T1, T2> ToDictionary<T1, T2>(List<List<string>> l)
    {
        var s1 = BTS.MethodForParse<T1>();
        var s2 = BTS.MethodForParse<T2>();

        var p1 = (Func<string, T1>)s1;
        var p2 = (Func<string, T2>)s2;

        var dict = new Dictionary<T1, T2>();

        var t1 = default(T1);
        var t2 = default(T2);

        var whereIsNotTwoEls = new Dictionary<int, List<string>>();

        var i = -1;

        foreach (var item in l)
        {
            i++;

            if (item.Count != 2)
            {
                whereIsNotTwoEls.Add(i, item);
                continue;
            }

            t1 = p1.Invoke(item[0]);
            t2 = p2.Invoke(item[1]);
            dict.Add(t1, t2);
        }

        foreach (var item in whereIsNotTwoEls)
        {
            var l2 = item.Value.ToList();
            l2.Insert(0, item.Key.ToString());
            //DebugLogger.Instance.WriteListOneRow(l2, "-");
        }

        if (whereIsNotTwoEls.Count != 0)
        {
        }

        return dict;
    }

    /// <summary>
    ///     In inner array is elements, in outer lines.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static List<List<string>> GetAllElementsFile(string file)
    {
        return GetAllElementsFile(file);
    }


    public static List<string> RemoveComments(List<string> tf)
    {
        //CA.RemoveStringsEmpty2(tf);

        tf = tf.Where(d => !string.IsNullOrWhiteSpace(d)).ToList();

        // Nevím vůbec co toto má znamenat ael nedává mi to smysl
        // Příště dopsat komentář pokud budu odkomentovávat
        //if (tf.Count > 0)
        //{
        //    if (tf[0].StartsWith("#"))
        //    {
        //        return tf[0];
        //    }
        //}


        //CA.RemoveStartingWith("#", tf);

        tf = tf.Where(d => !d.StartsWith("#")).ToList();
        return tf;
    }

    public static List<List<string>> GetAllElementsFile(string file,
        string oddelovaciZnak = "|")
    {
        var (header, rows) = GetAllElementsFileAdvanced(file, oddelovaciZnak);
        if (header.Count > 0) rows.Insert(0, header);

        return rows;
    }

    public static
#if ASYNC
        async Task
#else
    void
#endif
        Dictionary<T1, T2>(string file, Dictionary<T1, T2> artists)
    {
        var sb = new StringBuilder();
        foreach (var item in artists) sb.AppendLine(PrepareToSerialization(item.Key.ToString(), item.Value.ToString()));


#if ASYNC
        await
#endif
            File.WriteAllTextAsync(file, sb.ToString());
    }

    public static async Task WriteAllElementsToFile<Key, Value>(string coolPeopleShortcuts, Dictionary<Key, Value> d2)
    {
        var list = ListFromDictionary(d2);
        await WriteAllElementsToFile(coolPeopleShortcuts, list);
    }

    public static async Task WriteAllElementsToFile(string VybranySouborLogu, List<List<string>> p)
    {
        var sb = new StringBuilder();
        foreach (var item in p) sb.AppendLine(PrepareToSerialization(item));

        await File.WriteAllTextAsync(VybranySouborLogu, sb.ToString());
    }

    public static List<List<string>> ListFromDictionary<Key, Value>(Dictionary<Key, Value> d2)
    {
        var vs = new List<List<string>>();
        foreach (var item in d2)
        {
            vs.Add([item.Key.ToString(), item.Value.ToString()]);
        }

        return vs;
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

    public static
#if ASYNC
        async Task
#else
    void
#endif
        DictionaryAppend(string v, Dictionary<int, string> toSave)
    {
        var c = await File.ReadAllTextAsync(v);
        var s = ListFromDictionary(toSave);
        var s2 = ToDictionary<int, string>(s);

        var sb = new StringBuilder();
        foreach (var item in s2) sb.AppendLine(PrepareToSerialization(item.Key.ToString(), item.Value));

#if ASYNC
        await
#endif
            File.AppendAllTextAsync(v, sb + Environment.NewLine);
    }

    /// <param name="element"></param>
    /// <param name="line"></param>
    /// <param name="elements"></param>
    private static string GetElementAtIndex(List<List<string>> elements, int element, int line)
    {
        if (elements.Count > line)
        {
            var lineElements = elements[line];
            if (lineElements.Count > element) return lineElements[element];
        }

        return null;
    }


    public static
#if ASYNC
        async Task<List<List<string>>>
#else
 List<List<string>>
#endif
        AppendAllText(string path, string line)
    {
        var content = (await
                File.ReadAllLinesAsync(path)).ToList();
        CA.Trim(content);
        //content += Environment.NewLine + line + Environment.NewLine;
        content.Add(line);

        var vr = GetAllElementsLines(content);


#if ASYNC
        await
#endif
            File.WriteAllLinesAsync(path, content);

        return vr;
    }



    private static List<List<string>> GetAllElementsLines(List<string> lines)
    {
        lines = RemoveComments(lines);

        var vr = new List<List<string>>();
        foreach (var var in lines)
            if (!string.IsNullOrWhiteSpace(var))
                vr.Add(GetAllElementsLine(var));
        return vr;
    }

    /// <summary>
    ///     If index won't founded, return null.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="line"></param>
    public static string GetElementAtIndexFile(string file, int element, int line)
    {
        var elements = GetAllElementsFile(file);
        return GetElementAtIndex(elements, element, line);
    }

    /// <summary>
    ///     G null if first element on any lines A2 dont exists
    /// </summary>
    /// <param name="file"></param>
    /// <param name="element"></param>
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
    /// <param name="file"></param>
    /// <param name="element"></param>
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
    /// <param name="fileNameOrPath"></param>
    public static void ReadFileOfSettingsOther(string fileNameOrPath, Func<string, string> appDataCiReadFileOfSettingsOther)
    {
        // COmmented, app data not should be in *.web. pass directly as arg
        List<string> lines = null;
        lines = SHGetLines.GetLines(appDataCiReadFileOfSettingsOther(fileNameOrPath));
        if (lines.Count > 1)
        {
            int delimiterInt;
            if (int.TryParse(lines[0], out delimiterInt)) separatorString = ((char)delimiterInt).ToString();
        }
    }

    public static async Task WriteAllElementsToFile(string VybranySouborLogu, List<string>[] p)
    {
        var sb = new StringBuilder();
        foreach (var item in p) sb.AppendLine(PrepareToSerialization(item));

        await File.WriteAllTextAsync(VybranySouborLogu, sb.ToString());
    }


    /// <summary>
    ///     Without last |
    ///     DateTime is format with DTHelperEn.ToString
    /// </summary>
    /// <param name="o"></param>
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
    /// <param name="var"></param>
    public static List<string> GetAllElementsLine(string var, string oddelovaciZnak = null)
    {
        if (oddelovaciZnak == null) oddelovaciZnak = "|";
        // Musí tu být none, protože pak když někde nic nebylo, tak mi to je nevrátilo a progran vyhodil IndexOutOfRangeException
        return SHSplit.Split(var, oddelovaciZnak);
    }


    /// <summary>
    ///     In result A1 is not
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hlavicka"></param>
    /// <param name="oddelovaciZnak"></param>
    public static (List<string> header, List<List<string>> rows)
        GetAllElementsFileAdvanced(string file,
            string oddelovaciZnak = "|")
    {
        if (oddelovaciZnak == null) oddelovaciZnak = "|";

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
                if (lines[i].Trim().Length == 0) continue;
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