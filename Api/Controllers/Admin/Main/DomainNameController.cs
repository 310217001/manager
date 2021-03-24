using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model;
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
    /// 域名
    /// </summary>
    /// <returns></returns>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    public class DomainNameController : Controller
    {

        private readonly IDomainNameService DomaNamedb;
        private readonly IProjectInfoService Projectdb;
        private readonly IMapper IMapper;
        public DomainNameController(IDomainNameService DomaNamedb, IMapper IMapper, IProjectInfoService Projectdb)
        {
            this.DomaNamedb = DomaNamedb;
            this.Projectdb = Projectdb;
            this.IMapper = IMapper;
        }
        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="pageIndex">页码/param>
        /// <param name="Title">域名</param>
        /// <param name="IsEnable">启用状态</param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<ApiResult<IEnumerable<DomainNameViewModel>>> List(int pageIndex,string Title,bool? IsEnable)
        {
            var res = new ApiResult<IEnumerable<DomainNameViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            var parm = Expressionable.Create<DomainName>()
            .AndIF(!string.IsNullOrEmpty(Title), m => m.Title.Contains(Title))
            .AndIF(IsEnable != null, m => m.IsEnable == IsEnable);
            var list = DomaNamedb.GetPages(parm.ToExpression(), new PageParm(pageIndex));
            List<DomainNameViewModel> list2 = new List<DomainNameViewModel>();
            if (list != null)
            {
                foreach (var item in list.DataSource)
                {
                    list2.Add(new DomainNameViewModel
                    {
                        ID=item.ID,
                        Title = item.Title,
                        ProjectID=item.ProjectID,
                        ProjectNames = Projectdb.Get("Projects", item.ProjectID).Names,
                        Note = item.Note,
                        RegisterDate = Utility.GetDateFormat(item.RegisterDate),
                        ExpireDate = Utility.GetDateFormat(item.ExpireDate),
                        IsEnable = item.IsEnable,
                    });
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
       /// <param name="ID">域名ID</param>
       /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<DomainNameViewModel>> Item(int ID)
        {
            var res = new ApiResult<DomainNameViewModel>();
            DomainName m;
            DomainNameViewModel n = new DomainNameViewModel();
            if (ID > 0)
            {
                m = DomaNamedb.GetId(ID);
                if (m != null)
                {
                    n = IMapper.Map<DomainNameViewModel>(m);
                    n.ProjectNames = Projectdb.Get("Projects", m.ProjectID).Names;
                    res.success = true;
                }
            }
            res.data = n;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add(APIDomainNameViewModel domainName)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(domainName.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {
                    DomainName m = new DomainName()
                    {
                        Title = domainName.Title,
                        ProjectID = domainName.ProjectID,
                        Note = domainName.Note,
                        RegisterDate = C.DateTimes(domainName.RegisterDate),
                        ExpireDate = C.DateTimes(domainName.ExpireDate),
                        AddDate = DateTime.Now,
                        IsEnable = true
                    };
                    res.success = DomaNamedb.Add(m) > 0;
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
        /// <param name="id"></param>
        /// <param name="domainName"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update(int id, APIDomainNameViewModel domainName)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(domainName.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {

                    DomainName m = DomaNamedb.GetId(id);
                    m.Title = domainName.Title;
                    m.ProjectID = domainName.ProjectID;
                    m.Note = domainName.Note;
                    m.RegisterDate = C.DateTimes(domainName.RegisterDate);
                    m.ExpireDate = C.DateTimes(domainName.ExpireDate);
                    m.IsEnable = domainName.IsEnable;
                    res.success = DomaNamedb.Update(m) > 0;
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
        /// <param name="ID"></param>
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
                    if (DomaNamedb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new DomainName { IsEnable = false }) > 0)
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
