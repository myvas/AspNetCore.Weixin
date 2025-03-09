namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信WiFi无线访问点（AP）信息
/// </summary>
public class ApInfo
{
    /// <summary>
    /// 系统门店唯一标识ID,如果设备非系统指定安装，该项为空
    /// </summary>
    /// <example>例如：SHOPID1</example>
    //public string shopId { get; set; }

    /// <summary>
    /// 为 ap 无线 mac 地址,  即手机终端扫描 ap 获取的 mac 地址
    /// </summary>
    public string bssid { get; set; }

    /// <summary>
    /// 门店编号（Wi-Fi服务提供商的门店id，非系统门店id）
    /// </summary>
    /// <example>例如：123456789</example>
    public string storeId { get; set; }

    /// <summary>
    /// 门店名称
    /// </summary>
    public string storeName { get; set; }

    /// <summary>
    /// 门店经营的品牌名称
    /// </summary>
    public string brandName { get; set; }

    /// <summary>
    /// 门店所属省份，例如：广东省
    /// </summary>
    public string storeProvince { get; set; }

    /// <summary>
    /// 门店所属城市，例如：深圳市
    /// </summary>
    public string storeCity { get; set; }

    /// <summary>
    /// 门店详细地址
    /// </summary>
    public string storeAddress { get; set; }

    /// <summary>
    /// 门店所属行业
    /// </summary>
    public string storeField { get; set; }

    /// <summary>
    /// AP设备编码
    /// </summary>
    /// <example>例如：10</example>
    public string deviceNo { get; set; }

    /// <summary>
    /// AP设备型号
    /// </summary>
    public string deviceModel { get; set; }

    /// <summary>
    /// 设备MAC地址
    /// <para>V0.8接口变更为：格式冒号分隔，并且字母大写，例如84:23:B3:A2:D3:C9</para>
    /// </summary>
    /// <example></example>
    public string deviceMac { get; set; }
    /// <summary>
    /// 将其他格式MAC地址转换为本协议指定的格式。
    /// <para>例如：70:3A:D8:1F:38:F8，将转换为70-3a-d8-1f-38-f8</para>
    /// </summary>
    /// <param name="macAddress"></param>
    public void SetDeviceMac(string macAddress)
    {
        if (macAddress.IndexOf(":") > -1)
        {
            macAddress = macAddress.Replace(':', '-');
        }
        deviceMac = macAddress.ToLower();
    }

    /// <summary>
    /// AP设备的SSID（无线访问点的名字）
    /// </summary>
    /// <example>例如：WIFI1</example>
    public string ssid { get; set; }

    /// <summary>
    /// 关于带宽的描述，例如：4M 广州电信
    /// </summary>
    public string bandWidth { get; set; }

    /// <summary>
    /// AP带宽运营商，例如：中国电信、中国联通
    /// </summary>
    public string bandOpr { get; set; }

    /// <summary>
    /// 商户运营的微信公众号的appId，设备商需要向商户索取。
    /// <para>商户可用对应的公众号帐号登录微信管理后台。</para>
    /// <para>非必须</para>
    /// </summary>
    public string mpAppId { get; set; }

    /// <summary>
    /// 联系方式-Email
    /// </summary>
    /// <example>例如：demo@test.com</example>
    public string storeMail { get; set; }

    /// <summary>
    /// 联系方式-电话
    /// </summary>
    /// <example>例如：400 886 0106</example>
    public string storePhone { get; set; }

    /// <summary>
    /// 门店联系人
    /// </summary>
    public string storeContact { get; set; }

    /// <summary>
    /// 门店位置经度，例如：113.886993
    /// </summary>
    public string storeLongitude { get; set; }

    /// <summary>
    /// 门店位置纬度，例如：22.548933
    /// </summary>
    public string storeLatitude { get; set; }
}
