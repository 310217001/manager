using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class Lastweek
    {
        /// <summary>
        /// 星期一
        /// </summary>
        public DateTime Mon { get; set; }
        /// <summary>
        /// 星期二
        /// </summary>
        public DateTime Tue { get; set; }
        /// <summary>
        /// 星期三
        /// </summary>
        public DateTime Wed { get; set; }
        /// <summary>
        /// 星期四
        /// </summary>
        public DateTime Thu { get; set; }
        /// <summary>
        /// 星期五
        /// </summary>
        public DateTime Fir { get; set; }
        /// <summary>
        /// 星期六
        /// </summary>
        public DateTime Sat { get; set; }
        /// <summary>
        /// 星期天
        /// </summary>
        public DateTime Sun { get; set; }
        /// <summary>
        /// 下星期一
        /// </summary>
        public DateTime NextMon { get; set; }
    }
}
