namespace SunamoSerializer._sunamo.SunamoData.Data;

/// <summary>
/// Holds configuration for serialization content including separator settings.
/// </summary>
internal class SerializeContentArgs
{
    /// <summary>
    /// Gets or sets the separator string used for serialization.
    /// </summary>
    internal string SeparatorString { get; set; } = "|";

    /// <summary>
    /// Gets the separator character derived from the first character of the separator string.
    /// </summary>
    internal char SeparatorChar => SeparatorString[0];

    /// <summary>
    /// Gets the key code of the separator character.
    /// </summary>
    internal int KeyCodeSeparator => SeparatorChar;
}
