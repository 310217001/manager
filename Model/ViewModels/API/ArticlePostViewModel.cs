using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class ArticlePostViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDate { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 显示方式,1.文字 2.图片 3.视频 4.音频 5.软件
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int PageView { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// 状态 1.未审核 2.已审核 3.标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Synopsis { get; set; }
        /// <summary>
        /// 来源 转载或原创
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 栏目类型
        /// </summary>
        public string ArticleMenuIDs { get; set; }
    }
}
