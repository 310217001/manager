using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels
{
    /// <summary>
    /// 绑定Tree结构
    /// </summary>
    public class TreeViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public int sort { get; set; }
        public bool open { get; set; }
        public List<TreeViewModel> children { get; set; }
    }
    /// <summary>
    /// View UI的Tree结构
    /// </summary>
    public class Tree2ViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string title2 { get; set; }
        public int sort { get; set; }
        public bool expand { get; set; }
        public bool Checked { get; set; }
        public List<Tree2ViewModel> children { get; set; }
    }
    /// <summary>
    /// vue-treeselect的Tree结构
    /// </summary>
    public class Tree3ViewModel
    {
        public int id { get; set; }
        public string label { get; set; }
        public List<Tree3ViewModel> children { get; set; }
    }
}
