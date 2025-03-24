using System;

namespace Myvas.AspNetCore.Weixin;

public static class TypeHelper
{
    public static bool IsBasicType(Type t)
    {
        if (t == null)
            throw new ArgumentNullException(nameof(t));

        // Check for primitive types, string, decimal, and object
        return t.IsPrimitive ||
               t == typeof(string) ||
               t == typeof(decimal) ||
               t == typeof(object);
    }

    // Alternatively, you could use a more explicit approach:
    public static bool IsBasicTypeExplicit(Type t)
    {
        if (t == null)
            throw new ArgumentNullException(nameof(t));

        return t == typeof(sbyte) || t == typeof(byte) ||
               t == typeof(short) || t == typeof(ushort) ||
               t == typeof(int) || t == typeof(uint) ||
               t == typeof(long) || t == typeof(ulong) ||
               t == typeof(float) || t == typeof(double) ||
               t == typeof(decimal) ||
               t == typeof(bool) ||
               t == typeof(char) ||
               t == typeof(string) ||
               t == typeof(object);
    }
}
