using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPJ.Model
{
    public class Page<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page;
        /// <summary>
        /// 页大小
        /// </summary>
        public int psize;
        /// <summary>
        /// 总页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount;
        /// <summary>
        /// 是否更多
        /// </summary>
        public int hasnext;
        /// <summary>
        /// 数据项
        /// </summary>
        public List<T> Items { get; set; }
        public object Context { get; set; }

    }
}
