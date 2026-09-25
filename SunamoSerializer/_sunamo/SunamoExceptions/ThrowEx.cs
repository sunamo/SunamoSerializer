namespace SunamoSerializer._sunamo.SunamoExceptions;

internal partial class ThrowEx
{
    internal static void Custom(string message)
    {
        Debugger.Break();
        throw new Exception(message);
    }
}
