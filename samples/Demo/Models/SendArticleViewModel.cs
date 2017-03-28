using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class SendArticleViewModel
    {
        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
