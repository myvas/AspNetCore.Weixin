﻿namespace Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;

/// <summary>
/// Represents a filter used when accessing the persisted token store.
/// Setting multiple properties is interpreted as a logical 'AND' to further filter the query.
/// At least one value must be supplied.
/// </summary>
public class LocationEventReceivedEntryFilter : EventReceivedEntryFilter
{
}
