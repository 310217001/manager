using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.Common
{
    public class PageParm
    {
        public PageParm(int PageIndex, int PageSize = 15)
        {
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页总条数
        /// </summary>
        public int PageSize { get; set; } = 15;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string Sort { get; set; }

    }
}
