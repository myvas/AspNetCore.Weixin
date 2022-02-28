using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// If an entry class implaments this interface, its *IMPORTANT* data changes will be automatically saved into the table <see cref="AuditEntry"/>.
/// </summary>
public interface IAuditable
{
}
