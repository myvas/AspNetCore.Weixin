using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Entities;

#pragma warning disable 1591

public class PersistedToken
{
    public string AppId { get; set; }
    public string AccessToken { get; set; }
    public DateTime ExpirationTime { get; set; }
}
