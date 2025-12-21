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