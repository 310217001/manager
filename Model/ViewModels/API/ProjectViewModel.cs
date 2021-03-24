using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class ProjectViewModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 项目访问地址
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态 0未审核 1已审核 2失败 3被标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 网站类型名
        /// </summary>
        [Required(ErrorMessage = "项目类型不能为空")]
        public string ProjectTypeName { get; set; }
        /// <summary>
        /// 制作时间
        /// </summary>
        public string ProductionDate { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public string CompletionDate { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDate { get; set; }
    }
}
