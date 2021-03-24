using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Main
{
    /// <summary>
    /// 项目类型
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    public class ProjectTypeController : Controller
    {
        private readonly IProjectTypeService ProjectTypedb;
        public ProjectTypeController(IProjectTypeService ProjectTypedb)
        { 
            this.ProjectTypedb = ProjectTypedb; 
        }
        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetType")]
        public async Task<ApiResult<object>> GetadvertViewModel()
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
            FilterDefinition<ProjectTypeInfo> filter = Builders<ProjectTypeInfo>.Filter.Empty;
            var list = ProjectTypedb.GetList("ProjectType", filter);
            List<BaseViewModel1> advertlist = new List<BaseViewModel1>();
            foreach (var item in list)
            {
                advertlist.Add(new BaseViewModel1(C.String(item._id), item.Names));
            }
            res.data = advertlist;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="Title">项目类型标题</param>
        /// <param name="State">状态</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<ProjectTypeViewModel>>> List(int pageIndex, string Title, int? State)
        {
            var res = new ApiResult<IEnumerable<ProjectTypeViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            PageParm page = new PageParm(pageIndex);
            FilterDefinition<ProjectTypeInfo> filter = FilterDefinition<ProjectTypeInfo>.Empty;           
            var list = new List<FilterDefinition<ProjectTypeInfo>>();
            if (!string.IsNullOrEmpty(Title))
            {
                list.Add(Builders<ProjectTypeInfo>.Filter.Where(s => s.Names.Contains(Title)));
            }
            if (State != null && State != 0)
            {
                list.Add(Builders<ProjectTypeInfo>.Filter.Eq("State", State));
            }
            list.Add(Builders<ProjectTypeInfo>.Filter.Where(m=>m._id!=null));
            filter = Builders<ProjectTypeInfo>.Filter.And(list);
            long c = 0;
            var list1 = ProjectTypedb.GetList("ProjectType", filter, page.PageIndex, page.PageSize, out c);         
            List<ProjectTypeViewModel> list2 = new List<ProjectTypeViewModel>();
            if (list1 != null)
            {
                foreach (var item in list1)
                {
                    list2.Add(new ProjectTypeViewModel
                    {
                        ID = C.String(item._id),
                        Names = item.Names,
                        Sequence = item.Sequence,
                        Remarks = item.Remarks,
                        State = item.State,
                        AddDate = Utility.GetDateFormat(item.AddDate),
                    });
                }
                res.success = true;
                res.data = list2;
                res.index = pageIndex;
                res.count = C.Int(c);
                res.size = page.PageSize;
                res.pages = C.Int(c) / page.PageSize + 1;
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
        /// <param name="ID">项目类型ID</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<ProjectTypeViewModel>> Item(string ID)
        {
            var res = new ApiResult<ProjectTypeViewModel>();
            ProjectTypeInfo m;
            ProjectTypeViewModel n = new ProjectTypeViewModel();
            if (!string.IsNullOrEmpty(ID))
            {
                m = ProjectTypedb.Get("ProjectType", ID);
                if (m != null)
                {
                    n.ID = C.String(m._id);
                    n.Names = m.Names;
                    n.Sequence = m.Sequence;
                    n.Remarks = m.Remarks;
                    n.State = m.State;
                    n.AddDate = Utility.GetDateFormat(m.AddDate);
                    res.success = true;
                }
            }
            res.data = n;
            return await Task.Run(() => res);
        }
    }
}
