using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.Common
{
    /// <summary>
    /// 分页传参
    /// </summary>
    public class DBPage
    {
        public DBPage(int PageIndex, int PageSize)
        {
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
        }
        public DBPage(int PageIndex)
        {
            this.PageIndex = PageIndex;
            this.PageSize = 10;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }
    }
    /// <summary>
    /// 排序传参
    /// </summary>
    public class DBSort
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 顺序 True正序 Flase倒序
        /// </summary>
        public bool Asc { get; set; }
    }
}
