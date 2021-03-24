using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels
{
    public class AdverViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public string BackgroundColor { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

    }
}
