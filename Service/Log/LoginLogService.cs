using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Service.Base;
using System;

namespace Pi.PiManager.Service
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginLogService : BaseService<LoginLog>, ILoginLogService
    {
        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="Title">标题（用户名）</param>
        /// <param name="UserID">触发的用户ID</param>
        /// <param name="Browser">触发的浏览器</param>
        /// <param name="Os">触发的系统</param>
        /// <param name="R_W">分辨率宽度</param>
        /// <param name="R_H">分辨率高度</param>
        /// <param name="Remark">备注</param>
        /// <param name="CompanyID">单位</param>
        /// <returns></returns>
        public bool AddLoginLog(string Title, int UserID = 0, string Browser = "", string Os = "", int R_W = 0, int R_H = 0, string Remark = "", int CompanyID = 0)
        {
            return Add(new LoginLog() { Title = Title, UserID = UserID, ProjectID = "", AddDate = DateTime.Now, Browser = Browser, R_W = R_W, R_H = R_H, Os = Os, CompanyID = CompanyID, Remark = Remark }) > 0;
        }
    }
}
