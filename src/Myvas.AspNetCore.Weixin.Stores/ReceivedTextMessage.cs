using System;

namespace Myvas.AspNetCore.Weixin
{
    public class ReceivedTextMessage
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTimeOffset? ReceivedTime { get; set; }
        public string Content { get; set; }
    }
}