using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Models;

#pragma warning disable 1591

public class AuditEntry
{
    public AuditEntry()
    {
        Id = Guid.NewGuid().ToString("N");
        CreatedTime = DateTime.UtcNow;
    }

    public string Id { get; set; }

    public string TableName { get; set; }

    public DateTime CreatedTime { get; set; }

    public string KeyValue { get; set; }

    public string OldValue { get; set; }
    public string NewValue { get; set; }
}
