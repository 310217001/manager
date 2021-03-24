using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class ProjectPlanViewModel
    {
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
        /// 用户
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Principal { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplete { get; set; }
        /// <summary>
        /// 日志
        /// </summary>

        public List<PlanCommentInfoViewModel> planCommentInfoViews { get; set; }
    }
}
