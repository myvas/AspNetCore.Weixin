using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ReturnableViewModel
    {
        //[Url] //相对路径不符合要求
        [Display(Name = "返回")]
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">不允许接口，必须可以实例化。可以是值类型、基础类型、无参构造自定义类。</typeparam>
    public class ReturnableViewModel<T> : ReturnableViewModel
        where T : class
    {
        public T Item { get; set; }
    }
}
