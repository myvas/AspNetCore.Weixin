using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// A button container which contains one or more <see cref="Button"/>s. The container itself has not a "type" property, in other words, its value is null.
        /// </summary>
        public class Container : Button
        {
            public Container() : base() { }
            public Container(string name) : base(name) { }

            [JsonPropertyName("sub_button")]
            public List<Button> Buttons { get; set; } = [];
        }
    }
}