using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.ApiGroup
{
    /// <summary>
    /// 系统分组枚举值
    /// </summary>
    public enum ApiGroupNames
    {
        /// <summary>
        /// 后台登录、主页
        /// </summary>
        [GroupInfo(Title = "后台登录、主页", Description = "后台登录、主页接口", Version = "v1")]
        AdminAuth,
        /// <summary>
        /// 后台信息、广告
        /// </summary>
        [GroupInfo(Title = "后台信息、广告", Description = "后台信息、广告接口", Version = "v1")]
        AdminInfo,
        /// <summary>
        /// 后台系统管理
        /// </summary>
        [GroupInfo(Title = "后台系统管理", Description = "后台系统管理接口", Version = "v1")]
        AdminSystem,
        /// <summary>
        /// 项目管理
        /// </summary>
        [GroupInfo(Title = "项目管理", Description = "项目管理", Version = "v1")]
        Main,
    }
}
