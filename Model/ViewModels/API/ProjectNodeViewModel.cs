using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class ProjectNodeViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 编号 从1开始
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 项目名
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目分类ID
        /// </summary>
        public string ProjectTypeID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        public string CompleteDate { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string LastUpdateDate { get; set; }
        /// <summary>
        /// 计划数量
        /// </summary>
        public int PlanCount { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 组长
        /// </summary>
        public List<APITeamMembersViewModel> Teamleades { get; set; }
        /// <summary>
        /// 组员
        /// </summary>
        public List<APITeamMembersViewModel> Teammembers { get; set; }
        /// <summary>
        /// 完成状态 0.未完成 1.提前完成 2.按时完成 3.延期完成 4.延期未完成
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary> 
        public bool IsEnable { get; set; }
    }
}
