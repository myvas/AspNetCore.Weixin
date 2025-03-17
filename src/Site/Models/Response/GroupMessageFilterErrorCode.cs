using System.ComponentModel;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 群发消息返回状态(高级群发消息的状态)
/// </summary>
public enum GroupMessageFilterErrorCode
{
   [Description("涉嫌广告")]
   SuspectedAdvertisement = 10001,

   [Description("涉嫌政治")]
   SuspectedPoliticalContent = 20001,

   [Description("涉嫌社会")]
   SuspectedSocialContent = 20004,

   [Description("涉嫌色情")]
   SuspectedPornographicContent = 20002,

   [Description("涉嫌违法犯罪")]
   SuspectedIllegalActivity = 20006,

   [Description("涉嫌欺诈")]
   SuspectedFraud = 20008,

   [Description("涉嫌版权")]
   SuspectedCopyrightInfringement = 20013,

   [Description("涉嫌互推")]
   SuspectedMutualPromotion = 22000,

   [Description("涉嫌其他")]
   SuspectedOtherIssues = 21000
}