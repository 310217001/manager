using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels
{
    public class ArticleViewModel
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 栏目
        /// </summary>
        public List<ArticleMenuModel> ArticleMenu { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int PageView { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 状态 1.未审核 2.已审核 3.标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDate { get; set; }

    }
}
