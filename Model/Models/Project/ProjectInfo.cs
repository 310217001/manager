using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pi.PiManager.Model
{
    /// <summary>
    /// 网站类 用户类需要网站ID来表明这个用户来源
    /// </summary>
    public class ProjectInfo
    {
        public ObjectId _id { get; set; }
        [BsonIgnore]
        public string ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 项目访问地址
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态 0未审核 1已审核 2失败 3被标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 网站类型ID
        /// </summary>
        public string ProjectTypeID { get; set; }
        /// <summary>
        /// 制作时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ProductionDate { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CompletionDate { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime AddDate { get; set; }
    }
}
