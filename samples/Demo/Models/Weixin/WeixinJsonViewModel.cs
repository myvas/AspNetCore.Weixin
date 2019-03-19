using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{

    public class WeixinJsonViewModel
    {
        public string AppId { get; set; }

        public string Token { get; set; }

        [MaxLength(102400)]
        public string Json { get; set; }
    }
}
