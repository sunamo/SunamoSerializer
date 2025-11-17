// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoSerializer._sunamo.SunamoBts;

internal class BTS
{
    internal static string ToString<T>(Type type)
    {
        return type.ToString();
    }

    internal static object MethodForParse<T1>()
    {
        var type = typeof(T1);

        #region Same seria as in DefaultValueForTypeT

        #region MyRegion

        if (type == Types.tString) return new Func<string, string>(s => s);
        if (type == Types.tBool) return new Func<string, bool>(bool.Parse);

        #endregion

        #region Signed numbers

        if (type == Types.tFloat) return new Func<string, float>(float.Parse);
        if (type == Types.tDouble) return new Func<string, double>(double.Parse);
        if (type == typeof(int)) return new Func<string, int>(int.Parse);
        if (type == Types.tLong) return new Func<string, long>(long.Parse);
        if (type == Types.tShort) return new Func<string, short>(short.Parse);
        if (type == Types.tDecimal) return new Func<string, decimal>(decimal.Parse);
        if (type == Types.tSbyte) return new Func<string, sbyte>(sbyte.Parse);

        #endregion

        #region Unsigned numbers

        if (type == Types.tByte) return new Func<string, byte>(byte.Parse);
        if (type == Types.tUshort) return new Func<string, ushort>(ushort.Parse);
        if (type == Types.tUint) return new Func<string, uint>(uint.Parse);
        if (type == Types.tUlong) return new Func<string, ulong>(ulong.Parse);

        #endregion

        if (type == Types.tDateTime) return new Func<string, DateTime>(DateTime.Parse);
        if (type == Types.tGuid) return new Func<string, Guid>(Guid.Parse);
        if (type == Types.tChar) return new Func<string, char>(s => s[0]);

        #endregion

        return null;
    }
}