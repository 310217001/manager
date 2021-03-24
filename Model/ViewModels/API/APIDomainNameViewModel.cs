using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class APIDomainNameViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        [Required(ErrorMessage = "项目不能为空")]
        public string ProjectID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegisterDate { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public string ExpireDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
