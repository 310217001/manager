using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class BaseViewModel
    {
        public BaseViewModel(int ID,string Title)
        {
            this.ID = ID;
            this.Title = Title;
        }

        public int ID { get; set; }
        public string Title { get; set; }
    }
    public class BaseViewModel1
    {
        public BaseViewModel1(string ID, string Title)
        {
            this.ID = ID;
            this.Title = Title;
        }

        public string ID { get; set; }
        public string Title { get; set; }
    }
}
