using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Controllers;
using Pi.PiManager.IService;
using Pi.PiManager.Model;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.Models.Project;
using Pi.PiManager.Model.ViewModels.API;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Main
{
    /// <summary>
    /// 项目节点
    /// </summary>
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    [Produces("application/json")]
    public class ProjectNodeController : BaseController
    {
        private readonly IProjectNodeService PNdb;
        private readonly IMapper IMapper;
        private readonly IProjectInfoService Projectdb;
        private readonly IUserService userdb;
        private readonly IProjectPlanService plandb;
        private readonly IProjectUserService projectUserdb;
        public ProjectNodeController(IProjectNodeService PNdb, IMapper IMapper, IProjectInfoService Projectdb, IUserService userdb, IProjectPlanService plandb, IProjectUserService projectUserdb)
        {
            this.PNdb = PNdb;
            this.IMapper = IMapper;
            this.Projectdb = Projectdb;
            this.userdb = userdb;
            this.plandb = plandb;
            this.projectUserdb = projectUserdb;
        }
        /// <summary>
        /// 项目节点列表显示
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="ProjectTypeID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsEnable"></param>
        /// <param name="State"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet("GetList")]
        public async Task<ApiResult<IEnumerable<ProjectNodeViewModel>>> GetList(int pageIndex, string? ProjectTypeID, string? ProjectID, bool? IsEnable, int? State, string? Title)
        {
            var res = new ApiResult<IEnumerable<ProjectNodeViewModel>>() { statusCode = (int)ApiEnum.Status };
            int UserID = GetUserID();
            if (UserID > 0)
            {
                if (pageIndex == 0)
                    pageIndex = 1;
                var parm = Expressionable.Create<ProjectNodeInfo>();
                PagedInfo<ProjectNodeInfo> list = null;
                UserInfo userInfo = userdb.GetId(UserID);
                if (userInfo != null)
                {
                    var parm2 = Expressionable.Create<ProjectUserInfo>()
                    .AndIF(userInfo.RoleID != 1, o => o.UserID == UserID)
                    .And(o => o.IsEnable == true);
                    List<ProjectUserInfo> projectUserInfo = projectUserdb.GetWhere(parm2.ToExpression());
                    int[] nodes;
                    if (projectUserInfo != null && projectUserInfo.Count > 0)
                    {
                        if (userInfo.RoleID != 1)
                        {
                            nodes = new int[projectUserInfo.Count];
                            int i = 0;
                            foreach (var item in projectUserInfo)
                            {
                                nodes[i] = item.ProjectNodeID;
                                i++;
                            }
                            parm.AndIF(nodes.Length > 0, o => nodes.Contains(o.ID));
                        }
                        parm.AndIF(!string.IsNullOrWhiteSpace(ProjectID), m => m.ProjectID == ProjectID);
                        parm.AndIF(IsEnable != null, m => m.IsEnable == IsEnable);
                        parm.AndIF(State != null, m => m.State == State);
                        parm.AndIF(!string.IsNullOrWhiteSpace(Title), m => m.Title.Contains(Title));
                        parm.And(m => m.TypeID == 1);
                        if (!string.IsNullOrWhiteSpace(ProjectTypeID))
                        {
                            List<ProjectInfo> projects = Projectdb.GetList("Projects", Builders<ProjectInfo>.Filter.Where(o => o.ProjectTypeID == ProjectTypeID), null, null);
                            string[] x = new string[projects.Count];
                            int a = 0;
                            foreach (var item in projects)
                            {
                                x[a] = item.ID;
                                a++;
                            }
                            parm.And(m => x.Contains(m.ProjectID));
                        }
                        list = PNdb.GetPages(parm.ToExpression(), new PageParm(pageIndex), "ID DESC");
                    }
                    List<ProjectNodeViewModel> list2 = new List<ProjectNodeViewModel>();
                    if (list != null)
                    {
                        foreach (var item in list.DataSource)
                        {
                            ProjectNodeViewModel projectNodeViewModel = IMapper.Map<ProjectNodeViewModel>(item);
                            projectNodeViewModel.PlanCount = plandb.GetCount(o => o.ProjectNodeID == item.ID && o.ProjectID == item.ProjectID);
                            ProjectInfo projectInfo = Projectdb.Get("Projects", item.ProjectID);
                            List<UserInfo> userInfos = userdb.GetWhere(a => SqlFunc.Subqueryable<ProjectUserInfo>().Where(b => b.UserID == a.ID && b.ProjectNodeID == item.ID && b.Role == 1 && b.IsEnable == true).Any());
                            if (userInfos != null)
                            {
                                List<APITeamMembersViewModel> ats = new List<APITeamMembersViewModel>();
                                foreach (var item2 in userInfos)
                                {
                                    ats.Add(new APITeamMembersViewModel()
                                    {
                                        ID = item2.ID,
                                        Name = userdb.GetNames(item2)
                                    });
                                }
                                projectNodeViewModel.Teamleades = ats;
                            }
                            List<UserInfo> userInfos2 = userdb.GetWhere(a => SqlFunc.Subqueryable<ProjectUserInfo>().Where(b => b.UserID == a.ID && b.ProjectNodeID == item.ID && b.Role == 2 && b.IsEnable == true).Any());
                            if (userInfos2 != null)
                            {
                                List<APITeamMembersViewModel> ats = new List<APITeamMembersViewModel>();
                                foreach (var item2 in userInfos2)
                                {
                                    ats.Add(new APITeamMembersViewModel()
                                    {
                                        ID = item2.ID,
                                        Name = userdb.GetNames(item2)
                                    });
                                }
                                projectNodeViewModel.Teammembers = ats;
                            }
                            if (projectInfo != null)
                            {
                                projectNodeViewModel.ProjectName = projectInfo.Names;
                                projectNodeViewModel.ProjectTypeID = projectInfo.ProjectTypeID;
                            }
                            list2.Add(projectNodeViewModel);
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
                        res.success = true;
                        res.statusCode = (int)ApiEnum.Status;
                    }
                }
                else
                {
                    res.msg = "无该用户信息";
                }

            }
            else
            {
                res.msg = "请重新登陆";
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 显示详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<ProjectNodeViewModel>> Item(int ID)
        {
            var res = new ApiResult<ProjectNodeViewModel>();
            ProjectNodeInfo m;
            ProjectNodeViewModel n = new ProjectNodeViewModel();
            if (ID > 0)
            {
                m = PNdb.GetId(ID);
                if (m != null)
                {
                    n = IMapper.Map<ProjectNodeViewModel>(m);
                    ProjectInfo projectInfo = Projectdb.Get("Projects", m.ProjectID);
                    if (projectInfo != null)
                    {
                        n.ProjectName = projectInfo.Names;
                        n.ProjectTypeID = projectInfo.ProjectTypeID;
                    }
                    res.success = true;
                }
            }
            res.data = n;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add([FromBody] APIProjectNodeViewModel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(p.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {
                    ProjectNodeInfo m = new ProjectNodeInfo()
                    {
                        Title = p.Title,
                        ProjectID = p.ProjectID,
                        StartDate = C.DateTimes(p.StartDate),
                        EndDate = C.DateTimes(p.EndDate),
                        CompleteDate = C.DateTimes(p.CompleteDate),
                        Content = p.Content,
                        State = p.State,
                        AddDate = DateTime.Now,
                        TypeID = 1,
                        IsEnable = p.IsEnable
                    };
                    res.success = PNdb.Add(m) > 0;
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
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update([FromBody] APIProjectNodeViewModel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(p.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {
                    ProjectNodeInfo m = PNdb.GetId(p.ID);
                    m.Title = p.Title;
                    m.StartDate = C.DateTimes(p.StartDate);
                    m.EndDate = C.DateTimes(p.EndDate);
                    m.CompleteDate = C.DateTimes(p.CompleteDate);
                    m.Content = p.Content;
                    m.State = p.State;
                    m.IsEnable = p.IsEnable;
                    res.success = PNdb.Update(m) > 0;
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
                    if (PNdb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new ProjectNodeInfo { IsEnable = false }) > 0)
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

        /// <summary>
        /// 项目节点维护列表显示
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="ProjectTypeID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsEnable"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet("GetMaintainList")]
        public async Task<ApiResult<IEnumerable<ProjectNodeViewModel>>> GetMaintainList(int pageIndex, string? ProjectTypeID, string? ProjectID, bool? IsEnable, string? Title)
        {
            var res = new ApiResult<IEnumerable<ProjectNodeViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            var parm = Expressionable.Create<ProjectNodeInfo>()
            .AndIF(!string.IsNullOrWhiteSpace(ProjectID), m => m.ProjectID == ProjectID)
            .AndIF(IsEnable != null, m => m.IsEnable == IsEnable)
            .AndIF(!string.IsNullOrWhiteSpace(Title), m => m.Title.Contains(Title))
            .And(m => m.TypeID == 2);
            if (!string.IsNullOrWhiteSpace(ProjectTypeID))
            {
                List<ProjectInfo> projects = Projectdb.GetList("Projects", Builders<ProjectInfo>.Filter.Where(o => o.ProjectTypeID == ProjectTypeID), null, null);
                string[] x = new string[projects.Count];
                int i = 0;
                foreach (var item in projects)
                {
                    x[i] = item.ID;
                    i++;
                }
                parm.And(m => x.Contains(m.ProjectID));
            }
            var list = PNdb.GetPages(parm.ToExpression(), new PageParm(pageIndex), "ID DESC");
            List<ProjectNodeViewModel> list2 = new List<ProjectNodeViewModel>();
            if (list != null)
            {
                foreach (var item in list.DataSource)
                {
                    ProjectNodeViewModel projectNodeViewModel = IMapper.Map<ProjectNodeViewModel>(item);
                    projectNodeViewModel.PlanCount = plandb.GetCount(o => o.ProjectNodeID == item.ID && o.ProjectID == item.ProjectID);
                    ProjectInfo projectInfo = Projectdb.Get("Projects", item.ProjectID);
                    List<UserInfo> userInfos = userdb.GetWhere(a => SqlFunc.Subqueryable<ProjectUserInfo>().Where(b => b.UserID == a.ID && b.ProjectNodeID == item.ID && b.Role == 1).Any());
                    if (userInfos != null)
                    {
                        List<APITeamMembersViewModel> ats = new List<APITeamMembersViewModel>();
                        foreach (var item2 in userInfos)
                        {
                            ats.Add(new APITeamMembersViewModel()
                            {
                                ID = item2.ID,
                                Name = userdb.GetNames(item2)
                            });
                        }
                        projectNodeViewModel.Teamleades = ats;
                    }
                    List<UserInfo> userInfos2 = userdb.GetWhere(a => SqlFunc.Subqueryable<ProjectUserInfo>().Where(b => b.UserID == a.ID && b.ProjectNodeID == item.ID && b.Role == 2).Any());
                    if (userInfos2 != null)
                    {
                        List<APITeamMembersViewModel> ats = new List<APITeamMembersViewModel>();
                        foreach (var item2 in userInfos2)
                        {
                            ats.Add(new APITeamMembersViewModel()
                            {
                                ID = item2.ID,
                                Name = userdb.GetNames(item2)
                            });
                        }
                        projectNodeViewModel.Teammembers = ats;
                    }
                    if (projectInfo != null)
                    {
                        projectNodeViewModel.ProjectName = projectInfo.Names;
                        projectNodeViewModel.ProjectTypeID = projectInfo.ProjectTypeID;
                    }
                    list2.Add(projectNodeViewModel);
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
        /// 添加维护
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost("AddMaintain")]
        public async Task<ApiResult> AddMaintain([FromBody]APIProjectNodeViewModel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(p.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {
                    ProjectNodeInfo m = new ProjectNodeInfo()
                    {
                        Title = p.Title,
                        ProjectID = p.ProjectID,
                        StartDate = C.DateTimes(p.StartDate),
                        EndDate = C.DateTimes(p.EndDate),
                        CompleteDate = C.DateTimes(p.CompleteDate),
                        Content = p.Content,
                        State = p.State,
                        AddDate = DateTime.Now,
                        TypeID = 2,
                        IsEnable = p.IsEnable
                    };
                    res.success = PNdb.Add(m) > 0;
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
