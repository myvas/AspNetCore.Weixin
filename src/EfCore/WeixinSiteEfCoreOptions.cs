namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Used for store specific options
/// </summary>
public class WeixinSiteEfCoreOptions
{
    /// <summary>
    /// The min interval is limited to 3. It means that 480 times pulling per day at most conditions.
    /// </summary>
    /// <remarks>
    /// For a service account has less than 1000 subscribers, it is ok for common use cases.
    /// <list type="bullet">
    /// <item> the quota of user/get is 500 times per day</item>
    /// <item> the quota of user/info is 500000 times per day</item>
    /// </list>
    /// </remarks>
    public const int MinSyncIntervalInMinutesForWeixinSubscribers = 3;

    public bool EnableSyncForWeixinSubscribers { get; set; }

    public int SyncIntervalInMinutesForWeixinSubscribers { get; set; }

    /// <summary>
    /// If set to a positive number, the default OnModelCreating will use this value as the max length for any 
    /// properties used as keys, i.e. UserId, LoginProvider, ProviderKey.
    /// </summary>
    public int MaxLengthForKeys { get; set; }

    /// <summary>
    /// If set to true, the store must protect all personally identifying data for a user. 
    /// This will be enforced by requiring the store to implement <see cref="IProtectedUserStore{TUser}"/>.
    /// </summary>
    public bool ProtectPersonalData { get; set; }
}
