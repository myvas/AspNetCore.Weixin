// Copyright (c) 2022 FrankH (4848285@qq.com)

namespace System;

/// <summary>
/// The <see cref="DateTime"/> wrapper for Unix time seconds.
/// </summary>
public readonly struct UnixDateTime
{
    /// <summary>
    /// The value.
    /// </summary>
    public DateTime DateTime { get; }

    /// <summary>
    /// The basic functional constructor.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/>.</param>
    public UnixDateTime(DateTime dateTime)
    {
        DateTime = dateTime;
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public UnixDateTime() : this(new DateTime())
    {
    }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="unixTimeSeconds">The Unix time seconds.</param>
    public UnixDateTime(long unixTimeSeconds) : this(UnixEpoch.DateTime.AddSeconds(unixTimeSeconds))
    {
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="DateTime"/>.
    /// </summary>
    /// <param name="value">The <see cref="DateTime"/>.</param>
    public static implicit operator UnixDateTime(DateTime value)
    {
        return new UnixDateTime(value);
    }

    /// <summary>
    /// Implicit conversion operator to <see cref="DateTime"/>.
    /// </summary>
    /// <param name="value">The <see cref="UnixDateTime"/>.</param>
    public static implicit operator DateTime(UnixDateTime value)
    {
        return value.DateTime;
    }
}
