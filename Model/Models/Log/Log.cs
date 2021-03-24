using SqlSugar;
using System;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 日志父类
    /// </summary>
    public class Log
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
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
        /// 触发的项目ID
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
