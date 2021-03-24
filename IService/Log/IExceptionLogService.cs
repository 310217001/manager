using Pi.PiManager.IService.Base;
using Pi.PiManager.Model.Models;
using System;

namespace Pi.PiManager.IService
{
    public interface IExceptionLogService : IBaseService<ExceptionLog>
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="userID">用户ID</param>
        /// <param name="Type">类型 1.异常 2.文件不存在 3.登录失败 4.记录</param>
        /// <returns></returns>
        public bool AddBugLog(string title, string content = "", int userID = 0, int? Type = null);

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="e">系统异常类</param>
        /// <param name="title">标题</param>
        /// <param name="userID">用户ID</param>
        /// <param name="Type">类型 1.异常 2.文件不存在 3.登录失败 4.记录</param>
        /// <returns></returns>
        public bool AddBugLog(Exception e, string title = null, int userID = 0, int? Type = null);
    }
}
