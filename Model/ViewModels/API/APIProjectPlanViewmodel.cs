using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class APIProjectPlanViewmodel
    {
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        public int ProjectNodeID { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public int Principal { get; set; }
        /// <summary>
        /// 修改意见，无填0
        /// </summary>
        public int ProjectModifyID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        public string CompleteDate { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplete { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }
    }
}
