using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    /// <summary>
    /// 服务商模型
    /// </summary>
    public class ProviderViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
