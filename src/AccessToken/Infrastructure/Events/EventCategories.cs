namespace Myvas.AspNetCore.Weixin.Events;

/// <summary>
/// Categories for events
/// </summary>
public static class EventCategories
{
    /// <summary>
    /// Authentication related events
    /// </summary>
    public const string Authentication = "Authentication";

    /// <summary>
    /// Backchannel authentication related events
    /// </summary>
    public const string BackchannelAuthentication = "BackchannelAuthentication";

    /// <summary>
    /// Token related events
    /// </summary>
    public const string Token = "Token";

    /// <summary>
    /// Grants related events
    /// </summary>
    public const string Grants = "Grants";

    /// <summary>
    /// Error related events
    /// </summary>
    public const string Error = "Error";

    /// <summary>
    /// Device flow related events
    /// </summary>
    public const string DeviceFlow = "Device";
}