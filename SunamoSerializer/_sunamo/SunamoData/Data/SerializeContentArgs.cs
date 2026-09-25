namespace SunamoSerializer._sunamo.SunamoData.Data;

internal class SerializeContentArgs
{
    internal string SeparatorString { get; set; } = "|";

    internal char SeparatorChar => SeparatorString[0];

    internal int KeyCodeSeparator => SeparatorChar;
}
