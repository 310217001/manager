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

namespace Pi.PiManager.Controllers
{
    /// <summary>
    /// 角色接口
    /// </summary>
    //[Authorize(Roles = "user")]
    [Authorize(Policy = "Admin")]
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService db;
        public RoleController(IRoleService db)
        {
            this.db = db;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        //[Authorize(Roles = "system")]
        [HttpGet(template: "List2")]
        public async Task<ApiResult<List<Role>>> List2()
        {
            var res = new ApiResult<List<Role>>() { statusCode = (int)ApiEnum.ParameterError };
            res.success = true;
            res.statusCode = (int)ApiEnum.Status;
            res.data = db.GetWhere(a => true);
            return await Task.Run(() => res);
        }


        /// <summary>
        /// 列表显示带分页
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<ApiResult<IEnumerable<Role>>> List(int pageIndex, string title)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<Role>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0) pageIndex = 1;
            //DBPage dp = new DBPage(pageIndex, 15);
            var parm = Expressionable.Create<Role>();
            parm.AndIF(!string.IsNullOrEmpty(title), m => m.Names.Contains(title));
            var data = db.GetPages(parm.ToExpression(), new PageParm(pageIndex), "Sorting,ID DESC");
            res.success = true;
            res.data = data.DataSource;
            res.index = pageIndex;
            res.count = data.TotalCount;
            res.size = data.PageSize;
            return await Task.Run(() => res);
        }
        ///// <summary>
        ///// 详情显示
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //[HttpGet("{ID}")]
        //public IActionResult Item(int ID)
        //{
        //    return Ok(db.GetId(ID));
        //}

        /// <summary>
        /// 详情显示
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<Role>> Item(int ID)
        {
            var res = new ApiResult<Role>() { statusCode = (int)ApiEnum.ParameterError };
            if (ID > 0)
            {
                Role m = db.GetId(ID);
                if (m != null)
                {
                    res.success = true;
                    res.statusCode = (int)ApiEnum.Status;
                    res.data = m;
                }
                else
                    res.msg = "查询失败";
            }
            return await Task.Run(() => res);
        }



        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Names">名称</param>
        /// <param name="Sorting">顺序</param>
        /// <returns></returns>
        [HttpPost]
        //[ApiExplorerSettings(IgnoreApi = true)] //隐藏接口
        public async Task<ApiResult> Add(string Names, int Sorting)
        {
            // 以接口的形式返回数据
            var res = new ApiResult { statusCode = (int)ApiEnum.ParameterError };
            if (string.IsNullOrWhiteSpace(Names))
            {
                res.msg = "请填写标题";
            }
            else
            {
                try
                {
                    Role m = new Role();
                    m.Names = Names;
                    m.Sorting = Sorting;
                    res.success = db.Add(m) > 0;
                    if (res.success)
                        res.statusCode = (int)ApiEnum.Status;
                }
                catch (Exception ex)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            // {"success":true,"message":null,"statusCode":200,"data":null}
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPut]
        //[ApiExplorerSettings(IgnoreApi = true)] //隐藏接口
        //public async Task<ApiResult<string>> Update(Role m)
        public async Task<ApiResult> Update(Role m)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(m.Names))
            {
                res.msg = "请填写标题";
                res.statusCode = (int)ApiEnum.ParameterError;
            }
            else
            {
                try
                {
                    res.success = db.Update(m) > 0;
                }
                catch (Exception ex)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            // {"success":true,"message":null,"statusCode":200,"data":null}
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 部分编辑
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="Names">名称</param>
        /// <param name="Sorting">顺序</param>
        /// <returns></returns>
        [HttpPatch]
        //[ApiExplorerSettings(IgnoreApi = true)] //隐藏接口
        //public async Task<ApiResult<string>> Update(Role m)
        public async Task<ApiResult<string>> Update(int ID, string Names, int Sorting)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.ParameterError };
            if (string.IsNullOrWhiteSpace(Names))
            {
                res.msg = "请填写标题";
            }
            else
            {
                try
                {
                    Role m = db.GetId(ID);
                    if (m != null)
                    {
                        m.Names = Names;
                        m.Sorting = Sorting;
                        res.success = db.Update(m) > 0;
                        if (res.success)
                            res.statusCode = (int)ApiEnum.Status;
                    }
                    else
                        res.msg = "查询失败";
                }
                catch (Exception ex)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            // {"success":true,"message":null,"statusCode":200,"data":null}
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        public ApiResult Delete(int ID)
        {
            if (ID > 0)
                return new ApiResult { statusCode = (int)ApiEnum.Status, success = db.Delete(ID) > 0 };
            return new ApiResult { statusCode = (int)ApiEnum.ParameterError, msg = "参数丢失" };
        }
    }
}
