using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class SearchViewModel : ReturnableViewModel
    {
        [Display(Name = "搜索")]
        public string SearchString { get; set; }
    }

    public class SearchViewModel<TEntity> : SearchViewModel<TEntity, string>
        where TEntity : IEntity<string>
    {
    }


    public class SearchViewModel<TEntity, TKey> : ReturnableViewModel
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
    {
        [Display(Name = "搜索")]
        public string SearchString { get; set; }

        public IList<TEntity> Items { get; set; }

        public SearchViewModel()
        {
            Items = new List<TEntity>();
        }
    }
}
