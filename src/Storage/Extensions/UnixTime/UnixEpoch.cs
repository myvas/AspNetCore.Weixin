/// <summary>
/// Unix epoch
/// </summary>
public static class UnixEpoch
{

#if NETSTANDARD2_0

    /// <summary>
    /// Unix epoch to DateTime
    /// </summary>
    /// <returns></returns>
    public static readonly DateTime DateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Unix epoch to DateTimeOffset
    /// </summary>
    /// <returns></returns>
    public static readonly DateTimeOffset DateTimeOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

#else // NETSTANDARD2_1

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

#endif

}