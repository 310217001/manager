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

namespace Pi.PiManager.Api.Controllers.Admin.Info
{
    /// <summary>
    /// 广告类型
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminInfo)]
    public class AdvertTypeController : ControllerBase
    {
        private readonly IAdvertTypeService db;
        public AdvertTypeController(IAdvertTypeService db)
        {
            this.db = db;
        }
        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<AdvertType>>> List(int pageIndex, string Title)
        {
            var res = new ApiResult<IEnumerable<AdvertType>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            var parm = Expressionable.Create<AdvertType>().
                AndIF(!string.IsNullOrEmpty(Title), m => m.Names.Contains(Title));
            var list = db.GetPages(parm.ToExpression(), new PageParm(pageIndex), a => a.Sorting, "Asc");
            if (list != null)
            {
                res.success = true;
                res.data = list.DataSource;
                res.index = pageIndex;
                res.count = list.TotalCount;
                res.size = list.PageSize;
                res.pages = list.TotalPages;
            }
            else
            {
                res.success = false;
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 显示详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<AdvertType>> Item(int id)
        {
            var res = new ApiResult<AdvertType>();
            AdvertType m;
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
                m = new AdvertType();
            }
            res.data = m;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Names">名称</param>
        /// <param name="Sorting">顺序</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add(string Names, int Sorting, string Remark)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(Names))
            {
                res.msg = "请填写标题";
            }
            else
            {
                try
                {
                    AdvertType m = new AdvertType()
                    {
                        Names = Names,
                        Sorting = Sorting,
                        Remark = Remark
                    };
                    res.success = db.Add(m) > 0;
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
        /// 修改
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Names"></param>
        /// <param name="Sorting"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update(int ID, string Names, int Sorting, string Remark)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(Names))
            {
                res.msg = "请填写标题";
            }
            else
            {
                try
                {
                    AdvertType m = new AdvertType()
                    {
                        ID = ID,
                        Names = Names,
                        Sorting = Sorting,
                        Remark = Remark
                    };
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
    }
}
