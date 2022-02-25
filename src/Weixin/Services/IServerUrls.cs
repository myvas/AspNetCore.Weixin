﻿namespace Myvas.AspNetCore.Weixin.Services;

/// <summary>
/// Configures the per-request URLs and paths into the current server
/// </summary>
public interface IServerUrls
{
    /// <summary>
    /// Gets or sets the origin for IdentityServer. For example, "https://server.acme.com:5001".
    /// </summary>
    string Origin { get; set; }

    /// <summary>
    /// Gets or sets the base path of IdentityServer.
    /// </summary>
    string BasePath { get; set; }

    /// <summary>
    /// Gets the base URL for IdentityServer.
    /// </summary>
    string BaseUrl { get => Origin + BasePath; }
}