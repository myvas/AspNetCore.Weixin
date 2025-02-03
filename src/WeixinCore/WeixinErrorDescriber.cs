using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinErrorDescriber
    {
        public virtual WeixinError DefaultError()
        {
            return new WeixinError
            {
                Code = nameof(DefaultError),
                Description = Resources.DefaultError
            };
        }


        public virtual WeixinError DuplicateOpenId(string openId)
        {
            return new WeixinError
            {
                Code = nameof(DuplicateOpenId),
                Description = string.Format(Resources.DuplicateOpenId, openId)
            };

        }

        public virtual WeixinError ConcurrencyFailure()
        {
            return new WeixinError
            {
                Code = nameof(ConcurrencyFailure),
                Description = Resources.ConcurrencyFailure
            };
        }
    }
}
