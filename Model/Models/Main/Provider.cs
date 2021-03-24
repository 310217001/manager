using SqlSugar;
using System;

namespace Pi.PiManager.Model.Models.Main
{
    /// <summary>
    /// 服务商
    /// </summary>
    [SugarTable("T_Provider")]
    public class Provider
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
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
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}