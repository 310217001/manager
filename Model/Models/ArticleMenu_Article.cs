using SqlSugar;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 文章和栏目关联表
    /// </summary>
    [SugarTable("t_articlemenu_article")]
    public class ArticleMenu_Article
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 栏目ID
        /// </summary>
        public int ArticleMenuID { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        public int ArticleID { get; set; }
    }
}
