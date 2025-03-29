using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class WeixinConditionalMenuJson : WeixinErrorJson
{
    [JsonPropertyName("menu")]
    public MenuClass Menu { get; set; } = new();

    [JsonPropertyName("conditionalmenu")]
    public InnerConditionalMenuClass ConditionalMenu { get; set; } = new();

    public class MenuClass
    {
        [JsonPropertyName("button")]
        public List<WeixinMenuJson.Button> Buttons { get; set; } = [];

        /// <summary>
        /// 208396938
        /// </summary>
        [JsonPropertyName("menuid")]
        public int? MenuId { get; set; }
    }

    public class InnerConditionalMenuClass
    {
        [JsonPropertyName("button")]
        public List<WeixinMenuJson.Button> Buttons { get; set; } = [];


        [JsonPropertyName("matchrule")]
        public MatchRule MatchRule { get; set; } = new();

        /// <summary>
        /// 208396938
        /// </summary>
        [JsonPropertyName("menuid")]
        public int? MenuId { get; set; }
    }

    public class MatchRule
    {
        /// <summary>
        /// 用户标签的id，可通过用户标签管理接口获取
        /// </summary>
        [JsonPropertyName("tag_id")]
        public string TagId { get; set; }

        /// <summary>
        /// 2
        /// </summary>
        [JsonPropertyName("group_id")]
        public int? GroupId { get; set; }

        /// <summary>
        /// 已废除。性别：男（1）女（2），不填则不做匹配
        /// </summary>
        [JsonPropertyName("sex")]
        public int? Sex { get; set; }

        /// <summary>
        /// 已废除。国家信息，是用户在微信中设置的地区，具体请参考地区信息表
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// 已废除。省份信息，是用户在微信中设置的地区，具体请参考地区信息表
        /// </summary>
        [JsonPropertyName("province")]
        public string Province { get; set; }

        /// <summary>
        /// 已废除。城市信息，是用户在微信中设置的地区，具体请参考地区信息表
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// 客户端版本，当前只具体到系统型号：IOS(1), Android(2),Others(3)，不填则不做匹配
        /// </summary>
        [JsonPropertyName("client_platform_type")]
        public int? ClientPlatformType { get; set; }

        /// <summary>
        /// 已废除。语言信息，是用户在微信中设置的语言，具体请参考语言表：
        /// <para>1、简体中文 "zh_CN" 2、繁体中文TW "zh_TW" 3、繁体中文HK "zh_HK" 4、英文 "en" 
        /// 5、印尼 "id" 6、马来 "ms" 7、西班牙 "es" 8、韩国 "ko" 9、意大利 "it" 10、日本 "ja" 
        /// 11、波兰 "pl" 12、葡萄牙 "pt" 13、俄国 "ru" 14、泰文 "th" 15、越南 "vi" 
        /// 16、阿拉伯语 "ar" 17、北印度 "hi" 18、希伯来 "he" 19、土耳其 "tr" 20、德语 "de" 21、法语 "fr"</para>
        /// </summary>
        [JsonPropertyName("language")]
        public string Language{get;set;}
    }
}
