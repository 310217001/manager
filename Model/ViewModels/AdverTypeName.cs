using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class AdvertTypeName
    {
        public AdvertTypeName(int ID, string TypeName)
        {
            this.ID = ID;
            this.TypeName = TypeName;
        }
        public int ID { get; set; }
        public string TypeName { get; set; }

    }
}
