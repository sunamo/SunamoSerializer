// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoSerializer._sunamo.SunamoExceptions;

internal partial class ThrowEx
{


    #region Other


    #endregion
    internal static void Custom(string v)
    {
        Debugger.Break();
        throw new Exception(v);
    }
}