using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models.Main;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Main
{
    /// <summary>
    /// 服务商
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    public class ProviderController : Controller
    {
        private readonly IProviderService providerdb;
        private readonly IMapper IMapper;

        public ProviderController(IProviderService providerdb, IMapper IMapper)
        {
            this.providerdb = providerdb;
            this.IMapper = IMapper;
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetType")]
        public async Task<ApiResult<object>> GetadvertViewModel()
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
            var list = providerdb.GetAll();
            list.OrderBy(a => a.Sorting);
            List<BaseViewModel1> advertlist = new List<BaseViewModel1>();
            foreach (var item in list)
            {
                advertlist.Add(new BaseViewModel1(C.String(item.ID), item.Title));
            }
            res.data = advertlist;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="Title">服务商名</param>
        /// <param name="IsEnable">启用状态</param>
        /// <returns></returns>       
        [HttpGet]
        public async Task<ApiResult<IEnumerable<ProviderViewModel>>> List(int pageIndex, string Title, bool? IsEnable)
        {
            var res = new ApiResult<IEnumerable<ProviderViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            var parm = Expressionable.Create<Provider>()
            .AndIF(!string.IsNullOrEmpty(Title), m => m.Title.Contains(Title))
            .AndIF(IsEnable != null, m => m.IsEnable == IsEnable);
            var list = providerdb.GetPages(parm.ToExpression(), new PageParm(pageIndex), a => a.Sorting, "Asc");
            List<ProviderViewModel> list2 = new List<ProviderViewModel>();
            if (list != null)
            {
                foreach (var item in list.DataSource)
                {
                    list2.Add(IMapper.Map<ProviderViewModel>(item));
                }
                res.success = true;
                res.data = list2;
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
        /// <param name="ID">服务商ID</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<ProviderViewModel>> Item(int ID)
        {
            var res = new ApiResult<ProviderViewModel>();
            Provider m;
            ProviderViewModel n = new ProviderViewModel();
            if (ID > 0)
            {
                m = providerdb.GetId(ID);
                n = IMapper.Map<ProviderViewModel>(m);
                if (n != null)
                {
                    res.success = true;
                }
            }
            else 
            {
                n.IsEnable = true;
            }
            res.data = n;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Note">备注</param>
        /// <param name="Sorting">顺序</param>
        /// <param name="IsEnable">启用状态</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add(string Title, string Note, int Sorting,bool IsEnable)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(Title))
            {
                res.msg = "请填写标题";
            }
            else
            {
                try
                {
                    Provider m = new Provider()
                    {
                        Title = Title,
                        Sorting = Sorting,
                        Note = Note,                       
                        AddDate = DateTime.Now,
                        IsEnable = IsEnable
                    };
                    res.success = providerdb.Add(m) > 0;
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
        /// <param name="Title">标题</param>
        /// <param name="Note">备注</param>
        /// <param name="Sorting">顺序</param>
        /// <param name="IsEnable">启用状态</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update(int ID, string Title, string Note, int Sorting, bool IsEnable)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(Title))
            {
                res.msg = "请填写标题";
            }
            else
            {
                try
                {

                    Provider m = providerdb.GetId(ID);
                    m.Title = Title;
                    m.Note = Note;
                    m.Sorting = Sorting;
                    m.IsEnable = IsEnable;
                    res.success = providerdb.Update(m) > 0;
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
        /// 删除
        /// </summary>
        /// <param name="ID">服务商ID</param>
        /// <returns></returns>
        [HttpDelete("SetState")]
        public async Task<ApiResult<string>> SetState(string ID)
        {
            var res = new ApiResult<string>();
            if (!string.IsNullOrWhiteSpace(ID))
            {
                string[] array = ID.Trim(',').Split(',');
                int i = 0;
                foreach (string item in array)
                {
                    if (providerdb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new Provider { IsEnable = false }) > 0)
                        i++;
                }
                res.success = i > 0;
                if (res.success)
                {
                    res.msg = "删除成功";
                }
                else
                {
                    res.msg = "删除失败";
                    res.statusCode = (int)ApiEnum.Status;
                }
                res.count = i;
            }
            return await Task.Run(() => res);
        }
    }
}
