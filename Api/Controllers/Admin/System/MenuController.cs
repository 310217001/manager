using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Info
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService db;
        public MenuController(IMenuService db)
        {
            this.db = db;
        }

        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="IsEnable">是否启用</param>
        /// <param name="Title">标题</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<Menu>>> List(bool? IsEnable, string Title)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<Menu>>() { statusCode = (int)ApiEnum.Status };
            try
            {
                var parm = Expressionable.Create<Menu>()
                    .AndIF(!string.IsNullOrEmpty(Title), m => m.Names.Contains(Title))
                    .AndIF(IsEnable != null, m => m.IsEnable == IsEnable);
                var data = db.GetWhere(parm.ToExpression(), a => a.Sorting);
                List<Menu> list = new List<Menu>();
                if (data != null && data.Count() > 0)
                {
                    int fid = data.Min(a => a.FID);
                    IEnumerable<Menu> list1 = data.Where(a => a.FID == fid);  // 获得第一级
                    IEnumerable<Menu> list2 = null;
                    // 循环第一级
                    foreach (var item in list1)
                    {
                        item.Names = "└ " + item.Names;
                        list.Add(item);

                        // 增加子级
                        list2 = data.Where(a => a.FID == item.ID);
                        if (list2 != null && list2.Count() > 0)
                        {
                            foreach (var item2 in list2)
                            {
                                item2.Names = "　├ " + item2.Names;
                            }
                            list.AddRange(list2);
                        }
                    }
                }
                res.success = true;
                res.data = list;
                res.index = 1;
                res.count = data.Count;
                res.size = 1;
            }
            catch
            {
                res.success = false;
                res.msg = "查询失败";
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IsEnable"></param>
        /// <returns></returns>
        [HttpPut("Enable")]
        public async Task<ApiResult<IEnumerable<Menu>>> Enable(string ID, bool IsEnable)
        {
            var res = new ApiResult<IEnumerable<Menu>>();
            if (!string.IsNullOrWhiteSpace(ID))
            {
                try
                {
                    string[] array = ID.Trim(',').Split(',');
                    int i = 0;
                    foreach (string item in array)
                    {
                        if (db.Update(a => a.ID == Convert.ToInt32(item), a => new Menu { IsEnable = IsEnable }) > 0)
                            i++;
                    }
                    res.success = i > 0;
                    res.count = i;
                }
                catch (Exception ex)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            else
            {
                res.success = false;
                res.msg = "参数丢失";
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 详情显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResult<Menu>> Item(int id)
        {
            var res = new ApiResult<Menu>();
            if (id > 0)
            {
                Menu m = db.GetId(id);
                if (m != null)
                {
                    res.success = true;
                    res.data = m;
                }
                else
                    res.msg = "查询失败";
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 查询下拉框菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("Menu")]
        public async Task<ApiResult<IEnumerable<Menu>>> GetMenu()
        {
            var res = new ApiResult<IEnumerable<Menu>>();
            List<Menu> list0 = db.GetAll();
            List<Menu> list = new List<Menu>();
            IEnumerable<Menu> list1 = list0.Where(m => m.FID == 0);
            IEnumerable<Menu> list2 = null;
            foreach (var item in list1)
            {
                item.Names = "└ " + item.Names;
                list.Add(item);

                // 增加子级
                list2 = list0.Where(a => a.FID == item.ID);
                if (list2 != null && list2.Count() > 0)
                {
                    foreach (var item2 in list2)
                    {
                        item2.Names = "　├ " + item2.Names;
                    }
                    list.AddRange(list2);
                }
            }
            res.data = list;
            res.success = true;
            res.statusCode = (int)ApiEnum.Status;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="Names">名称</param>
        /// <param name="Icon">图标</param>
        /// <param name="Url">地址</param>
        /// <param name="Remark">备注</param>
        /// <param name="IsEnable">是否显示</param>
        /// <param name="Sorting">顺序</param>
        /// <param name="FID">父ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add(string Names, string Icon, string Url, string Remark, bool IsEnable = false, int Sorting = 0, int FID = 0)
        {
            // 以接口的形式返回数据
            var res = new ApiResult { statusCode = (int)ApiEnum.ParameterError };
            if (string.IsNullOrWhiteSpace(Names) && string.IsNullOrWhiteSpace(Icon) && string.IsNullOrWhiteSpace(Url) && string.IsNullOrWhiteSpace(Remark) && Sorting == 0 && FID == 0)
            {
                res.msg = "参数丢失";
            }
            else
            {
                try
                {
                    Menu m = new Menu();
                    m.FID = FID;
                    m.Icon = Icon;
                    m.Names = Names;
                    m.Url = Url;
                    m.IsEnable = IsEnable;
                    m.Sorting = Sorting;
                    m.Remark = Remark;
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
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update(Menu m)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(m.Names))
            {
                res.msg = "请填写名称";
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
            return await Task.Run(() => res);
        }
    }
}
