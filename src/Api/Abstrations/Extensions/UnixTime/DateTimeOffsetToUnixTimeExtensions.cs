// Copyright (c) 2022 FrankH (4848285@qq.com)

namespace System;

/// <summary>
/// Converter from <see cref="DateTimeOffset"/> (and also its nullable value type) to Unix time.
/// </summary>
public static class DateTimeOffsetToUnixTimeExtensions
{
    /// <summary>
    /// Converts <see cref="DateTime"/> to Unix time.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The Unix time, which is number of seconds since the Unix epoch.</returns>
    public static long ToUnixTime(this DateTimeOffset value)
    {
        return (long)value.ToUniversalTime().Subtract(UnixEpoch.DateTimeOffset).TotalSeconds;
    }

    /// <summary>
    /// Converts <see cref="Nullable{DateTime}"/> to Unix time.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The Unix time, which is number of seconds since the Unix epoch.</returns>
    public static long? ToUnixTime(this DateTimeOffset? value)
    {
        if (value == null) return null;

        return value!.Value.ToUnixTime();
    }
}
