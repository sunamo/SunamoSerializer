// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoSerializer._sunamo.SunamoData.Data;

internal class SerializeContentArgs
{
    internal string separatorString = "|";

    /// <summary>
    ///     Must be property - I can forget change value on three occurences.
    /// </summary>
    internal char separatorChar => separatorString[0];

    internal int keyCodeSeparator => separatorChar;
}