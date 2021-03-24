using SqlSugar;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [SugarTable("t_menu")]
    public class Menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int FID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsEnable { get; set; }
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
