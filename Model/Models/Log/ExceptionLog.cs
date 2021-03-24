using SqlSugar;
using System;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 异常日志
    /// </summary>
    [SugarTable("t_exceptionlog")]
    public class ExceptionLog : Log
    {
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
