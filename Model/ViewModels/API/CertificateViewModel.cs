
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pi.PiManager.Model.ViewModels.API
{

    public class CertificateViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 项目名
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 服务商ID
        /// </summary>
        public string ProviderID { get; set; }
        /// <summary>
        /// 服务商名
        /// </summary>
        public string ProviderName { get; set; }
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
