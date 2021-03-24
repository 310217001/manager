using SqlSugar;
using System;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 广告
    /// </summary>
    [SugarTable("T_Advert")]
    public class Advert
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public string BackgroundColor { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 地区：省级 ,ID,
        /// </summary>
        public string AreasID1 { get; set; }
        /// <summary>
        /// 地区：市级
        /// </summary>
        public string AreasID2 { get; set; }
        /// <summary>
        /// 地区：县级
        /// </summary>
        public string AreasID3 { get; set; }
        /// <summary>
        /// 文章
        /// </summary>
        public int ArticleID { get; set; }
    }
}
