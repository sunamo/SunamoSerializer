namespace SunamoSerializer._sunamo.SunamoBts;

/// <summary>
/// Provides base type system utilities for parsing and type conversion.
/// </summary>
internal class BTS
{
    /// <summary>
    /// Returns a parsing function delegate for the specified type, or null if the type is not supported.
    /// </summary>
    internal static object? MethodForParse<T>()
    {
        var type = typeof(T);

        if (type == Types.StringType) return new Func<string, string>(text => text);
        if (type == Types.BoolType) return new Func<string, bool>(bool.Parse);

        if (type == Types.FloatType) return new Func<string, float>(float.Parse);
        if (type == Types.DoubleType) return new Func<string, double>(double.Parse);
        if (type == typeof(int)) return new Func<string, int>(int.Parse);
        if (type == Types.LongType) return new Func<string, long>(long.Parse);
        if (type == Types.ShortType) return new Func<string, short>(short.Parse);
        if (type == Types.DecimalType) return new Func<string, decimal>(decimal.Parse);
        if (type == Types.SbyteType) return new Func<string, sbyte>(sbyte.Parse);

        if (type == Types.ByteType) return new Func<string, byte>(byte.Parse);
        if (type == Types.UshortType) return new Func<string, ushort>(ushort.Parse);
        if (type == Types.UintType) return new Func<string, uint>(uint.Parse);
        if (type == Types.UlongType) return new Func<string, ulong>(ulong.Parse);

        if (type == Types.DateTimeType) return new Func<string, DateTime>(DateTime.Parse);
        if (type == Types.GuidType) return new Func<string, Guid>(Guid.Parse);
        if (type == Types.CharType) return new Func<string, char>(text => text[0]);

        return null;
    }
}
