namespace SunamoSerializer._sunamo.SunamoData.Data;

internal class SerializeContentArgs
{
    internal string separatorString = AllStrings.verbar;

    /// <summary>
    ///     Must be property - I can forget change value on three occurences.
    /// </summary>
    internal char separatorChar => separatorString[0];

    internal int keyCodeSeparator => separatorChar;
}