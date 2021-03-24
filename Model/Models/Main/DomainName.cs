using SqlSugar;
using System;

namespace Pi.PiManager.Model.Models.Main
{
    /// <summary>
    /// 域名
    /// 只记录一级域名
    /// </summary>
    [SugarTable("T_DomainName")]
    public class DomainName
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterDate { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }
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
