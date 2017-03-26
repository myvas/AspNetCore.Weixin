using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.HechinaSmsService.Sample.Models
{
    public class SendSmsViewModel
    {
        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
