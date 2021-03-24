using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Api.Helper;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.System
{
    /// <summary>
    /// 欢迎页
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    //[Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class WelcomeController : Controller
    {
        private readonly IArticleService Articledb;
        private readonly IUserService Userdb;
        private readonly ILoginLogService LoginLogdb;
        private readonly IExceptionLogService ExLogdb;
        private readonly IAdvertService Advertdb;
        public WelcomeController(IArticleService Articledb, IUserService Userdb, ILoginLogService LoginLogdb, IExceptionLogService ExLogdb,IAdvertService Advertdb)
        {
            this.Articledb = Articledb;
            this.Userdb = Userdb;
            this.LoginLogdb = LoginLogdb;
            this.ExLogdb = ExLogdb;
            this.Advertdb = Advertdb;
        }
        /// <summary>
        /// 文章数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("ArticleCount")]
        public async Task<ApiResult<object>> ArticleCount()
        {
            var res = new ApiResult<object>();
            int count = Articledb.GetCount(a=>a.State==2);
            res.data = count;
            res.success = true;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 用户数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("UsersCount")]
        public async Task<ApiResult<object>> UsersCount()
        {
            var res = new ApiResult<object>();
            int count = Userdb.GetCount(a => a.State==2);
            res.data = count;
            res.success = true;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 广告数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("AdvertCount")]
        public async Task<ApiResult<object>> AdvertCount()
        {
            var res = new ApiResult<object>();
            int count = Advertdb.GetCount(a => a.IsEnable==true);
            res.data = count;
            res.success = true;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 用户登录次数
        /// </summary>
        /// <returns></returns>
        [HttpGet("UsersLoginCount")]
        public async Task<ApiResult<object>> UsersLoginCount()
        {
            var res = new ApiResult<object>();
            WelCount welCount = new WelCount();
            Lastweek lastweek = APIHelper.GetLastweek();//获取上星期八个时间戳
            if (LoginLogdb.GetCount(o => o.AddDate > lastweek.Mon && o.AddDate < lastweek.NextMon) >0)
            {
                welCount.Count1 = LoginLogdb.GetCount(o => o.AddDate > lastweek.Mon && o.AddDate < lastweek.Tue);
                welCount.Count2 = LoginLogdb.GetCount(o => o.AddDate > lastweek.Tue && o.AddDate < lastweek.Wed);
                welCount.Count3 = LoginLogdb.GetCount(o => o.AddDate > lastweek.Wed && o.AddDate < lastweek.Thu);
                welCount.Count4 = LoginLogdb.GetCount(o => o.AddDate > lastweek.Thu && o.AddDate < lastweek.Fir);
                welCount.Count5 = LoginLogdb.GetCount(o => o.AddDate > lastweek.Fir && o.AddDate < lastweek.Sat);
                welCount.Count6 = LoginLogdb.GetCount(o => o.AddDate > lastweek.Sat && o.AddDate < lastweek.Sun);
                welCount.Count7 = LoginLogdb.GetCount(o => o.AddDate > lastweek.Sun && o.AddDate < lastweek.NextMon);
                res.data = welCount;
                res.success = true;
            }
            else
            {
                res.msg = "无用户登录数据";
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 程序异常次数
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExLoginCount")]
        public async Task<ApiResult<object>> ExLoginCount()
        {
            var res = new ApiResult<object>();
            WelCount welCount = new WelCount();
            Lastweek lastweek = APIHelper.GetLastweek();//获取上星期八个时间戳
            if (ExLogdb.GetCount(o => o.AddDate > lastweek.Mon && o.AddDate < lastweek.NextMon) > 0)
            {
                welCount.Count1 = ExLogdb.GetCount(o => o.AddDate > lastweek.Mon && o.AddDate < lastweek.Tue);
                welCount.Count2 = ExLogdb.GetCount(o => o.AddDate > lastweek.Tue && o.AddDate < lastweek.Wed);
                welCount.Count3 = ExLogdb.GetCount(o => o.AddDate > lastweek.Wed && o.AddDate < lastweek.Thu);
                welCount.Count4 = ExLogdb.GetCount(o => o.AddDate > lastweek.Thu && o.AddDate < lastweek.Fir);
                welCount.Count5 = ExLogdb.GetCount(o => o.AddDate > lastweek.Fir && o.AddDate < lastweek.Sat);
                welCount.Count6 = ExLogdb.GetCount(o => o.AddDate > lastweek.Sat && o.AddDate < lastweek.Sun);
                welCount.Count7 = ExLogdb.GetCount(o => o.AddDate > lastweek.Sun && o.AddDate < lastweek.NextMon);
                res.data = welCount;
                res.success = true;
            }
            else
            {
                res.msg = "无程序异常数据";
            }
            return await Task.Run(() => res);
        }
    }
}
