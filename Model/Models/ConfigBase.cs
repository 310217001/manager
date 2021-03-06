using SqlSugar;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 网站基本配置
    /// </summary>
    [SugarTable("T_ConfigBase")]
    public class ConfigBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 首页Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 首页关键字
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 首页描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 首页JS代码
        /// </summary>
        public string JSCode { get; set; }
    }
}
