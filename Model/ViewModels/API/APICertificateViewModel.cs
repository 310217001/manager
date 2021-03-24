using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class APICertificateViewModel
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        [Required(ErrorMessage = "项目不能为空")]
        public string ProjectID { get; set; }
        /// <summary>
        /// 服务商
        /// </summary>
        [Required(ErrorMessage = "服务商不能为空")]
        public int ProviderID { get; set; }
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
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
