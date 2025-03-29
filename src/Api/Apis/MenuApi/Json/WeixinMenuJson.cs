using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/*************************************************
- selfmenu_info
  - button
    - (type=view, url=...)
    - (type=click, value=...)
    - (type=null)
      - sub_button
        - list
          - (type=view, url=...)
          - (type=click, value=...)
  - button
    - (type=view, url=...)
    - (type=text, value=...)
    - (type=img, value=...)
    - (type=null)
      - sub_button
        - list
          - (type=news, value=..., news_info=...)
          - (type=video, value=...)
          - (type=voice, value=...)
*************************************************/
/// <summary>
/// 自定义菜单。能够帮助公众号丰富界面，让用户更好更快地理解公众号的功能。
/// </summary>
/// <remarks>
/// 请注意：
/// <list type="number">
/// <item>定义菜单最多包括3个一级菜单，每个一级菜单最多包含5个二级菜单。</item>
/// <item>一级菜单最多4个汉字，二级菜单最多8个汉字，多出来的部分将会以“...”代替。</item>
/// <item>创建自定义菜单后，菜单的刷新策略是：在用户进入公众号会话页或公众号profile页时，如果发现上一次拉取菜单的请求在5分钟以前，就会拉取一下菜单。
/// 如果菜单有更新，就会刷新客户端的菜单。
/// 测试时可以尝试取消关注公众账号后再次关注，则可以看到创建后的效果。</item>
/// </list>
/// </remarks>
public partial class WeixinMenuJson { }
