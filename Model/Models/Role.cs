using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 角色
    /// </summary>
    [SugarTable("T_Role")]
    public class Role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Names { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        [DefaultValue(0)]
        public int Sorting { get; set; }
        /// <summary>
        /// 菜单权限
        /// </summary>
        public string Menus { get; set; }
        /// <summary>
        /// 栏目权限
        /// </summary>
        public string ArticleMenus { get; set; }
        /// <summary>
        /// 课程权限
        /// </summary>
        //@ApiModelProperty(hidden = true)
        //@JsonIgnore
        //[ApiModelProperty(hidden = true)]
        public string Courses { get; set; }
    }
}
