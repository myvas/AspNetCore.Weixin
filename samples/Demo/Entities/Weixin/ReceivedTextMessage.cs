using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Entities
{
    public class ReceivedTextMessage : Entity
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTimeOffset ReceivedTime { get; set; }
        public string Content { get; set; }
    }
}
