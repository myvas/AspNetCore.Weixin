using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class GetAuditStatusResultJson : WifiErrorJson
    {
        /// <summary>
        /// 审核结果
        /// </summary>
        /// <value>
        /// <para>0-待审核</para>
        /// <para>1-审核通过。审核通过的会自动上线</para>
        /// <para>2-审核不通过</para>
        /// </value>
        public string auditResult;

        /// <summary>
        /// 审核意见
        /// </summary>
        public string auditComment;

        /// <summary>
        /// 
        /// </summary>
        public string auditTime;
    }
}
