using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    /// <summary>
    /// 树状图
    /// </summary>
    public class ELTreeViewModel
    {
        public int id { get; set; }
        public string label { get; set; }
        public List<ELTreeViewModel> children { get; set; }
    }
}
