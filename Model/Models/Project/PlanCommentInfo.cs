﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.Models.Project
{
    /// <summary>
    /// 工作日志
    /// </summary>
    [SugarTable("t_plancomment")]
    public class PlanCommentInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        public int ProjectNodeID { get; set; }
        /// <summary>
        /// 计划
        /// </summary>
        public int PlanID { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 是否确认
        /// </summary>
        public bool IsConfirm { get; set; }
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
