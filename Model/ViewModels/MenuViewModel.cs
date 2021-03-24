using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels
{
    /// <summary>
    /// 用于绑定页面上的菜单
    /// </summary>
    public class MenuViewModel
    {
        public string name { get; set; }

        public string url { get; set; }

        public string icon { get; set; }

        public IEnumerable<MenuViewModel> childMenus { get; set; }
    }
}
