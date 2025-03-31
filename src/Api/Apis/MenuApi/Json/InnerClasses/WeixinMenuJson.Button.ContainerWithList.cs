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
        public class ContainerWithList : Button
        {
            public ContainerWithList() : base() { }
            public ContainerWithList(string name) : base(name) { }

            [JsonPropertyName("sub_button")]
            public SubButtonClass SubButton { get; set; } = new();

            public class SubButtonClass
            {
                [JsonPropertyName("list")]
                public List<Button> Buttons { get; set; } = [];
            }
        }
    }
}