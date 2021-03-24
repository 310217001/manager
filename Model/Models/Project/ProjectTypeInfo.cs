using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pi.PiManager.Model
{
    /// <summary>
    /// 项目类 用户类需要网站ID来表明这个用户来源
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ProjectTypeInfo
    {
        public ObjectId _id { get; set; }
        [BsonIgnore]
        public string ID { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        ///  顺序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态 0未审核 1已审核 2禁用 3被标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime AddDate { get; set; }
    }
}
