// Copyright (c) 2022 FrankH (4848285@qq.com)

namespace System;

/// <summary>
/// Converter from <see cref="DateTime"/> (and also its nullable value type) to Unix time seconds.
/// </summary>
public static class DateTimeToUnixTimeSecondsExtensions
{
    /// <summary>
    /// Converts <see cref="DateTime"/> to Unix time seconds.
    /// </summary>
    /// <param name="value">UTC time</param>
    /// <returns>The Unix time seconds.</returns>
    public static long ToUnixTimeSeconds(this DateTime value)
    {
        return (long)value.ToUniversalTime().Subtract(UnixEpoch.DateTime).TotalSeconds;
    }

    /// <summary>
    /// Converts <see cref="Nullable{DateTime}"/> to Unix time seconds.
    /// </summary>
    /// <param name="value">The <see cref="Nullable{DateTime}"/>.</param>
    /// <returns>The Unix time seconds.</returns>
    public static long? ToUnixTimeSeconds(this DateTime? value)
    {
        if (value.HasValue)
            return value!.Value.ToUnixTimeSeconds();
        return null;
    }
}
