using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Interfaces;

/// <summary>
/// Abstraction for the operational data context.
/// </summary>
/// <seealso cref="IDisposable"/>
public interface IPersistedTokenDbContext : IDisposable
{
    /// <summary>
    /// Gets or sets the persisted tokens.
    /// </summary>
    /// <value>
    /// The persisted tokens.
    /// </value>
    DbSet<Entities.PersistedToken> PersistedTokens { get; set; }

    /// <summary>
    /// Gets or sets the weixin users.
    /// </summary>
    DbSet<Entities.WeixinSubscriber> Subscribers { get; set; }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync() => SaveChangesAsync(CancellationToken.None);
}
