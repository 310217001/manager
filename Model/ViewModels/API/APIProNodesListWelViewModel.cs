using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class APIProNodesListWelViewModel
    {
        /// <summary>
        /// 完成项目
        /// </summary>
        public List<APIProNodesWelViewModel> CompleteList { get; set; }
        /// <summary>
        /// 未完成项目
        /// </summary>
        public List<APIProNodesWelViewModel> UnfinishedList { get; set; }
        /// <summary>
        /// 已延期项目
        /// </summary>
        public List<APIProNodesWelViewModel> DelayList { get; set; }

    }


    public class APIProNodesWelViewModel
    {
        /// <summary>
        /// 项目名
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string ProNodeTitle { get; set; }
    }
}
