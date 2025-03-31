using System;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinErrorJson
{

    #region deprecated
    [Obsolete("Use ErrorCode instead.")]
    [JsonIgnore]
    public int? errcode { get => ErrorCode; }

    [Obsolete("Use ErrorMessage instead.")]
    [JsonIgnore]
    public string errmsg { get => ErrorMessage; }
    #endregion

}