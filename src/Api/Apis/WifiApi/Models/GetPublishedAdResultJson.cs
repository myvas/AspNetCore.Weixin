﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class GetPublishedAdResultJson : WifiErrorJson
    {
        /// <summary>
        /// 商家主页线上的url
        /// <para>例如：“http://imgurl”</para>
        /// </summary>
        public string adUrl;
    }
}
