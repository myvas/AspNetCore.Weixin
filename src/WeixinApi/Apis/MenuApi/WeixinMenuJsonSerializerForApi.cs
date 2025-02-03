using Newtonsoft.Json;
using System.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinMenuJsonSerializerForApi
    {
        public static string Serialize(WeixinMenu menu)
        {
            var items = menu.Items;
            var level1s = items.Where(x => x.ParentId == null).ToList();
            var level1Ids = level1s.Select(x => x.Id).ToList();
            foreach (var level1 in level1s)
            {
                level1.SubItems = items.Where(x => x.ParentId == level1.Id).ToList();
            }
            var data = new
            {
                button = level1s
            };
            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return json;
        }
    }
}
