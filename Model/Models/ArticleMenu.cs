using SqlSugar;
using System;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 栏目
    /// </summary>
    [SugarTable("t_articlemenu")]
    public class ArticleMenu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 存英文名或副标题
        /// </summary>
        public string ENames { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 企业ID
        /// </summary>
        public int EnterpriseID { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 状态 1.未审核 2.已审核 3.标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 类型，自由定义
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 外链
        /// </summary>
        public string URL { get; set; }
    }
}
