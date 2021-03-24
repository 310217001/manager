using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class ProjectTypeViewModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        ///  顺序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态 0未审核 1已审核 2禁用 3被标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        public string AddDate { get; set; }
    }
}
