using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Entities;

#pragma warning disable 1591

public class PersistedToken
{
    public string Id { get; set; }
    public string AppId { get; set; }
    public string Type { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? ConsumedDate { get; set; }
    public string Data { get; set; }
}
