using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin
{
	public class BatchGetMaterialsJson
    {
        public int total_count { get; set; }
        public int item_count { get; set; }
        public List<MaterialItem> item { get; set; }

        public class MaterialItem
        {
            public string media_id { get; set; }
            public string update_time { get; set; }
        }
        public class NonNewsMaterialItem : MaterialItem
        {
            public string name { get; set; }
            public string url { get; set; }
        }
        public class NewsMaterialItem : MaterialItem
        {
            public NewsMaterialItemContent content { get; set; }
        }

        public class NewsMaterialItemContent
        {
            public List<NewsMaterialItemContentItem> news_item { get; set; }

        }

        public class NewsMaterialItemContentItem
        {
            public string title { get; set; }
            public string thumb_media_id { get; set; }
            public string show_cover_pic { get; set; }
            public string author { get; set; }
            public string digest { get; set; }
            public string content { get; set; }
            public string url { get; set; }
            public string content_source_url { get; set; }
        }

    }
}
