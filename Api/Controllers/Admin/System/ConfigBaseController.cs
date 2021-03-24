using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using System;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.System
{
    /// <summary>
    /// 网络基本配置
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class ConfigBaseController : ControllerBase
    {
        public readonly IConfigBaseService db;
        public ConfigBaseController(IConfigBaseService db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task<ApiResult<string>> Item(ConfigBase m)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.ParameterError };
            try
            {
                if (m.ID == 0)
                {
                    //添加、返回是否成功
                    res.success = db.Add(m) > 0;
                }
                else
                {
                    //更新功能
                    res.success = db.Update(m) > 0;

                }
                //如果是true返回成功的状态码
                if (res.success)
                {
                    res.statusCode = (int)ApiEnum.Status;
                }
            }
            catch (Exception e)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + e.Message;
            }
            return await Task.Run(() => res);
        }
    }
}
