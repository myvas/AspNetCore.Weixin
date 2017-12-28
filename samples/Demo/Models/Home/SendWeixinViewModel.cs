using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class SendWeixinViewModel
    {
        public IList<ReceivedTextMessage> Received { get; set; }

        [Required]
        public string OpenId { get; set; }
        
        [Required]
        public string Content { get; set; }
    }
}
