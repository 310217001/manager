using System;
using SqlSugar;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 文章
    /// </summary>
    [SugarTable("t_article")]
    public class Article
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Synopsis { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 显示方式,1.文字 2.图片 3.视频 4.音频 5.软件
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int PageView { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 来源 转载或原创
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileFormat { get; set; }
        /// <summary>
        /// 视频时长（支部秒）
        /// </summary>
        public int VideoTime { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 添加用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 状态 1.未审核 2.已审核 3.标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 是否显示（显示在首页）
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 企业 用于企业发布公告
        /// </summary>
        public int EnterpriseID { get; set; }
        /// <summary>
        /// 活动 用于活动文章
        /// </summary>
        public int ActivityID { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
