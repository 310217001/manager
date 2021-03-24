using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.Log
{
    public class ExceptionLogViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 触发的用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDate { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否解决
        /// </summary>
        public bool IsSolve { get; set; }
        /// <summary>
        /// 类型 1.异常 2.文件不存在 3.登录失败 4.其它
        /// </summary>
        public int Type { get; set; }
    }
}
