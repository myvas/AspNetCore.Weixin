namespace System;

/// <summary>
/// Unix epoch, which is January 1, 1970, 00:00:00 UTC.
/// </summary>
public static class UnixEpoch
{

#if NETSTANDARD2_1_OR_GREATER

    /// <summary>
    /// Unix epoch to DateTime
    /// </summary>
    /// <returns></returns>
    public static readonly DateTime DateTime = DateTime.UnixEpoch;

    /// <summary>
    /// Unix epoch to DateTimeOffset
    /// </summary>
    /// <returns></returns>
    public static readonly DateTimeOffset DateTimeOffset = DateTimeOffset.UnixEpoch;

#else

    /// <summary>
    /// Unix epoch to DateTime
    /// </summary>
    /// <returns></returns>
    public static readonly DateTime DateTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Unix epoch to DateTimeOffset
    /// </summary>
    /// <returns></returns>
    public static readonly DateTimeOffset DateTimeOffset = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

#endif

}