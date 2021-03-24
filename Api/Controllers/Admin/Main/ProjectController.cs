using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model;
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
    /// 项目
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    public class ProjectController : Controller
    {
        private readonly IProjectInfoService Projectdb;
        private readonly IMapper IMapper;
        private readonly IProjectTypeService ProjectTypedb;
        public ProjectController(IProjectInfoService Projectdb, IMapper IMapper, IProjectTypeService ProjectTypedb)
        {
            this.Projectdb = Projectdb;
            this.IMapper = IMapper;
            this.ProjectTypedb = ProjectTypedb;
        }
        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetType")]
        public async Task<ApiResult<object>> GetProjectViewModel()
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
            FilterDefinition<ProjectInfo> filter = Builders<ProjectInfo>.Filter.Empty;
            var list = Projectdb.GetList("Projects", filter);
            List<BaseViewModel1> advertlist = new List<BaseViewModel1>();
            foreach (var item in list)
            {
                advertlist.Add(new BaseViewModel1(C.String(item._id), item.Names));
            }
            res.data = advertlist;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 通过项目分类获取项目
        /// </summary>
        /// <param name="ProjectTypeID"></param>
        /// <returns></returns>
        [HttpGet("GetProjects")]
        public async Task<ApiResult<object>> GetProjects(string ProjectTypeID)
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
            FilterDefinition<ProjectInfo> filter = Builders<ProjectInfo>.Filter.Empty;
            var list = new List<FilterDefinition<ProjectInfo>>();
            list.Add(Builders<ProjectInfo>.Filter.Where(s => s.ProjectTypeID == ProjectTypeID));
            filter = Builders<ProjectInfo>.Filter.And(list);
            var list1 = Projectdb.GetList("Projects", filter);
            List<BaseViewModel1> advertlist = new List<BaseViewModel1>();
            foreach (var item in list1)
            {
                advertlist.Add(new BaseViewModel1(C.String(item._id), item.Names));
            }
            res.success = true;
            res.data = advertlist;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="Title">标题</param>
        /// <param name="ProjectTypeID">项目类型</param>
        /// <param name="State">状态</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<ProjectViewModel>>> List(int pageIndex, string Title, string ProjectTypeID, int? State)
        {
            var res = new ApiResult<IEnumerable<ProjectViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            PageParm page = new PageParm(pageIndex);
            FilterDefinition<ProjectInfo> filter = FilterDefinition<ProjectInfo>.Empty;
            var list = new List<FilterDefinition<ProjectInfo>>();
            if (!string.IsNullOrWhiteSpace(Title))
            {
                list.Add(Builders<ProjectInfo>.Filter.Where(s => s.Names.Contains(Title)));
            }
            if (!string.IsNullOrWhiteSpace(ProjectTypeID))
            {
                list.Add(Builders<ProjectInfo>.Filter.Eq("ProjectTypeID", ProjectTypeID));
            }
            if (State != null && State != 0)
            {
                list.Add(Builders<ProjectInfo>.Filter.Eq("State", State));
            }
            list.Add(Builders<ProjectInfo>.Filter.Where(m=>m._id!=null));
            filter = Builders<ProjectInfo>.Filter.And(list);
            long c = 0;
            var list1 = Projectdb.GetList("Projects", filter, page.PageIndex, page.PageSize, out c);
            List<ProjectViewModel> list2 = new List<ProjectViewModel>();

            if (list1 != null)
            {
                foreach (var item in list1)
                {
                    list2.Add(new ProjectViewModel
                    {
                        ID = C.String(item._id),
                        Names = item.Names,
                        URL = item.URL,
                        Remarks = item.Remarks,
                        State = item.State,
                        ProjectTypeName = ProjectTypedb.Get("ProjectType", item.ProjectTypeID).Names,
                        ProductionDate = Utility.GetDateFormat(item.ProductionDate),
                        CompletionDate = Utility.GetDateFormat(item.CompletionDate),
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
        /// <param name="ID">项目ID</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<APIProjectViewModel>> Item(string ID)
        {
            var res = new ApiResult<APIProjectViewModel>();
            ProjectInfo m;
            APIProjectViewModel n = new APIProjectViewModel();
            if (!string.IsNullOrEmpty(ID))
            {
                m = Projectdb.Get("Projects", ID);
                if (m != null)
                {
                    n.ID = C.String(m._id);
                    n.Names = m.Names;
                    n.URL = m.URL;
                    n.Remarks = m.Remarks;
                    n.State = m.State;
                    n.ProjectTypeName = ProjectTypedb.Get("ProjectType", m.ProjectTypeID).Names;
                    n.ProductionDate = Utility.GetDateFormat(m.ProductionDate);
                    n.CompletionDate = Utility.GetDateFormat(m.CompletionDate);
                    res.success = true;
                }
            }
            res.data = n;
            return await Task.Run(() => res);
        }
    }
}
