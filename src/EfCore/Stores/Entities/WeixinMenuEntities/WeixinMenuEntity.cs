using System;
using System.Collections.Generic;
using System.Linq;

namespace Myvas.AspNetCore.Weixin.Entities;

public class WeixinMenuEntity
{
    protected WeixinMenuEntity()
    {
        Id = Guid.NewGuid().ToString();
    }
    public WeixinMenuEntity(string id, string name)
    {
        Id = id;
        Name = name;
    }
    public WeixinMenuEntity(string name) : this()
    {
        Name = name;
    }
    public string Id { get; set; }
    /// <summary>
    /// 名称，编辑管理时用
    /// </summary>
    public string Name { get; set; }

    public IReadOnlyList<WeixinMenuEntityItem> Items { get { return (IReadOnlyList<WeixinMenuEntityItem>)_items; } }
    private readonly IList<WeixinMenuEntityItem> _items = new List<WeixinMenuEntityItem>();

    public TMenuItem AddItem<TMenuItem>(TMenuItem item)
        where TMenuItem : WeixinMenuEntityItem
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(item.Name), "菜单标题不能为空");
        }
        if (item.Name.Length > 60)
        {
            throw new ArgumentException("菜单标题不能超过60个字节", nameof(item.Name));
        }
        if (item is IWeixinMenuEntityItemHasKey itemHasKey)
        {
            var key = itemHasKey?.Key ?? "";
            if (key.Length > 128)
            {
                throw new ArgumentException("菜单KEY值不能超过128个字节", nameof(IWeixinMenuEntityItemHasKey.Key));
            }
        }
        if (item is IWeixinMenuEntityItemHasUrl itemHasUrl)
        {
            var url = itemHasUrl?.Url ?? "";
            if (url.Length > 1024)
            {
                throw new ArgumentException("网页链接不能超过1024个字节", nameof(IWeixinMenuEntityItemHasKey.Key));
            }
        }

        // 检查现有菜单项
        var level1Ids = _items.Where(x => x.ParentId == null).Select(x => x.Id).ToList();
        var level2Ids = _items.Where(x => level1Ids.Contains(x.ParentId)).Select(x => x.Id).ToList();
        if (item.ParentId == null)
        {
            if (level1Ids.Count() >= 3)
            {
                throw new ArgumentNullException(nameof(item.ParentId), $"一级菜单不能超过3个");
            }
        }
        else
        {
            if (!level1Ids.Contains(item.ParentId))
            {
                if (level2Ids.Contains(item.ParentId))
                {
                    throw new ArgumentNullException(nameof(item.ParentId), $"菜单层数最多只能为2层");
                }
                else
                {
                    throw new ArgumentNullException(nameof(item.ParentId), $"上一级菜单不存在");
                }
            }
            else
            {
                if (level2Ids.Count() >= 5)
                {
                    throw new ArgumentNullException(nameof(item.ParentId), $"同一节点下不能超过5个");
                }
            }
        }

        // 自动添加可选数据
        if (string.IsNullOrEmpty(item.Id))
        {
            item.Id = Guid.NewGuid().ToString();
        }
        if (item.Order == 0)
        {
            var currentMaxOrder = _items.Where(x => x.ParentId == item.ParentId).Select(x=>x.Order).DefaultIfEmpty().Max();
            item.Order = Math.Min(currentMaxOrder, int.MaxValue - 2) + 1;
        }
        item.Order = Math.Min(item.Order, int.MaxValue - 1);

        _items.Add(item);
        return item;
    }

    public bool RemoveItem(string id)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return true;
        }
        return _items.Remove(item);
    }
}
