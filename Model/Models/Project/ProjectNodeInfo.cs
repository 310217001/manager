using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.Models.Project
{
    /// <summary>
    /// 项目节点
    /// </summary>
    [SugarTable("t_projectnode")]
    public class ProjectNodeInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 类型：1.正常 2.维护
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 编号 从1开始
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime CompleteDate { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 完成状态 0.未完成 1.提前完成 2.按时完成 3.延期完成 4.延期未完成
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 是否结算
        /// </summary>
        public bool IsSettlement { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /********** 验收单内容 **********/
        /// <summary>
        /// 申请验收时间
        /// </summary>
        public DateTime ApplyDate { get; set; }
        /// <summary>
        /// 验收时间
        /// </summary>
        public DateTime CheckDate { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        public int CheckUserID { get; set; }
        /// <summary>
        /// 验收评价
        /// </summary>
        public string CheckComment { get; set; }
        /********** 验收单内容结束 **********/
        /// <summary>
        /// 申请延期时间
        /// </summary>
        public DateTime CheckDate2 { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
