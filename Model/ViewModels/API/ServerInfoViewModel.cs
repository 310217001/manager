using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    /// <summary>
    /// 服务器模型
    /// </summary>
    public class ServerInfoViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 服务器名
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
        public int ProviderID { get; set; }
        /// <summary>
        /// 服务商名
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// 服务商账号
        /// </summary>
        public string ProviderAccount { get; set; }
        /// <summary>
        /// 服务商密码
        /// </summary>
        public string ProviderPassword { get; set; }
        /// <summary>
        /// 服务器账号
        /// </summary>
        public string ServerAccount { get; set; }
        /// <summary>
        /// 服务器密码
        /// </summary>
        public string ServerPassword { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 操作系统
        /// </summary>
        public string System { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
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
