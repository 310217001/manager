using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.System
{
    /// <summary>
    /// 全局配置
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class ConfigController : ControllerBase
    {

        private readonly IConfigService db;
        public ConfigController(IConfigService db)
        {
            this.db = db;
        }
        /// <summary>
        /// 列表显示
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<ApiResult<IEnumerable<Config>>> List(int pageIndex, string Title)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<Config>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;

            var parm = Expressionable.Create<Config>();
            parm = parm.AndIF(!string.IsNullOrEmpty(Title), m => m.Title.Contains(Title));
            var list = db.GetPages(parm.ToExpression(), new PageParm(pageIndex));
            if (list != null)
            {
                res.success = true;
                res.data = list.DataSource;
                res.index = pageIndex;
                res.count = list.TotalCount;
                res.size = list.PageSize;
                res.statusCode = (int)ApiEnum.Status;
            }
            else
            {
                res.success = false;
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 详情显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ID")]
        public async Task<ApiResult<Config>> Item(int id)
        {
            var res = new ApiResult<Config>();
            Config m;
            if (id > 0)
            {
                m = db.GetId(id);
                if (m != null)
                {
                    res.success = true;
                }
            }
            else
            {
                m = new Config();
            }
            res.data = m;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Number"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add(string Title, string Number, string Value)
        {
            var res = new ApiResult<string>();
            try
            {
                Config m = new Config()
                {
                    Title = Title,
                    Number = Number,
                    Value = Value

                };
                res.success = db.Add(m) > 0;
            }
            catch (Exception e)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + e.Message;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update(Config m)
        {
            var res = new ApiResult();
            try
            {
                res.success = db.Update(m) > 0;
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
