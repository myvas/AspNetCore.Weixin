// Copyright (c) 2022 FrankH (4848285@qq.com)

namespace System;

/// <summary>
/// The <see cref="System.DateTimeOffset"/> wapper for Unix time seconds.
/// </summary>
public readonly struct UnixDateTimeOffset
{
    /// <summary>
    /// The value.
    /// </summary>
    public DateTimeOffset DateTimeOffset { get; }

    /// <summary>
    /// The basic functional constructor.
    /// </summary>
    /// <param name="value">The <see cref="DateTime"/>.</param>
    public UnixDateTimeOffset(DateTimeOffset value)
    {
        DateTimeOffset = value;
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public UnixDateTimeOffset() : this(new DateTimeOffset())
    {
    }

    /// <summary>
    /// The copy constructor.
    /// </summary>
    /// <param name="unixTimeSeconds">The Unix time seconds.</param>
    public UnixDateTimeOffset(long unixTimeSeconds) : this(UnixEpoch.DateTimeOffset.AddSeconds(unixTimeSeconds))
    {
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="value">The <see cref="System.DateTimeOffset"/>.</param>
    public static implicit operator UnixDateTimeOffset(DateTimeOffset value)
    {
        return new UnixDateTimeOffset(value);
    }

    /// <summary>
    /// Implicit conversion operator to <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="value">The <see cref="UnixDateTimeOffset"/>.</param>
    public static implicit operator DateTimeOffset(UnixDateTimeOffset value)
    {
        return value.DateTimeOffset;
    }
}
