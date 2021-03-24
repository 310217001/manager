using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 广告类型
    /// </summary>
    [SugarTable("t_adverttype")]
    public class AdvertType
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
