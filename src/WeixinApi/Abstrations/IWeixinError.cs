namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinError
    {
        bool Succeeded { get; }

        /// <summary>
        /// 微信错误代码
        /// </summary>
        int? errcode { get; }

        /// <summary>
        /// 微信错误描述
        /// </summary>
        string errmsg { get; }
    }
}