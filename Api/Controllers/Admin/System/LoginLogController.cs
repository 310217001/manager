using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels.Log;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.System
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class LoginLogController : ControllerBase
    {
       
        private readonly ILoginLogService db;
        public LoginLogController(ILoginLogService db)
        {
            this.db = db;
        }

        /// <summary>
        /// 列表显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<LoginLogViewModel>>> List(int pageIndex, string Title, DateTime? Date1, DateTime? Date2)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<LoginLogViewModel>>();
            if (pageIndex == 0) pageIndex = 1;
            List<LoginLogViewModel> list = new List<LoginLogViewModel>();
            var parm = Expressionable.Create<LoginLog>()
                 .AndIF(!string.IsNullOrEmpty(Title), m => m.Title.Contains(Title))
                 .AndIF(Date1 != null, a => a.AddDate >= Date1)
                 .AndIF(Date2 != null, a => a.AddDate < Date2);
            var Paged = db.GetPages(parm.ToExpression(), new PageParm(pageIndex), "ID DESC");
            if (Paged.DataSource != null)
            {
                foreach (var item in Paged.DataSource)
                {
                    list.Add(new LoginLogViewModel
                    {
                        AddDate = Utility.GetDateFormat(item.AddDate),
                        Browser = item.Browser,
                        ID = item.ID,
                        Os = item.Os,
                        R_H = item.R_H,
                        R_W = item.R_W,
                        Title = item.Title,
                        UserID = item.UserID
                    });
                }
                res.data = list;
                if (res.data != null)
                {
                    res.success = true;
                    res.index = pageIndex;
                    res.count = Paged.TotalCount;
                    res.size = Paged.PageSize;
                }
                else
                {
                    res.msg = "无数据";
                    res.statusCode = (int)ApiEnum.Status;
                }
            }
            else
            {
                res.msg = "参数丢失";
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<ApiResult<IEnumerable<LoginLog>>> Delete(string ID)
        {
            var res = new ApiResult<IEnumerable<LoginLog>>() { statusCode = (int)ApiEnum.Status };
            if (!string.IsNullOrWhiteSpace(ID))
            {
                string[] array = ID.Trim(',').Split(',');
                if (array != null && array.Length > 0)
                {
                    int[] array2 = Array.ConvertAll(array, int.Parse);
                    res.success = db.Delete(array2) > 0;
                    if (res.success)
                    {
                        res.msg = "删除成功";
                        res.count = array2.Length;
                    }
                    else
                    {
                        res.msg = "删除失败";
                        res.statusCode = (int)ApiEnum.Status;
                    }
                }
            }
            return await Task.Run(() => res);
        }

    }
}
