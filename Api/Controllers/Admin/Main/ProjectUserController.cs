using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
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
    /// 项目成员
    /// </summary>
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    [Produces("application/json")]
    public class ProjectUserController : Controller
    {
        private readonly IProjectUserService projectUserdb;
        private readonly IUserService userdb;
        public ProjectUserController(IProjectUserService projectUserdb, IUserService userdb)
        {
            this.projectUserdb = projectUserdb;
            this.userdb = userdb;
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTeamMembersViewModel")]
        public async Task<ApiResult<object>> GetTeamMembersViewModel()
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
            var advertypename = userdb.GetWhere(o => o.State == 2);
            List<APITeamMembersViewModel2> atlist = new List<APITeamMembersViewModel2>();
            foreach (var item in advertypename)
            {
                atlist.Add(new APITeamMembersViewModel2(item.ID, item.NickName));
            }
            res.data = atlist;
            return await Task.Run(() => res);
        }


        ///// <summary>
        ///// 获取分类2
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("GetTeamMembersViewModel2")]
        //public async Task<ApiResult<object>> GetTeamMembersViewModel2()
        //{
        //    var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
        //    List<ProjectUserInfo> prousers = projectUserdb.GetWhere(o => o.IsEnable == true);
        //    List<UserInfo> advertypename = userdb.GetWhere(o => o.State == 2);
        //    List<APITeamMembersViewModel2> atlist = new List<APITeamMembersViewModel2>();
        //    foreach (var item in advertypename)
        //    {
        //        atlist.Add(new APITeamMembersViewModel2(item.ID, item.NickName));
        //    }
        //    res.data = atlist;
        //    return await Task.Run(() => res);
        //}

        /// <summary>
        /// 成员列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="Title"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ProjectNodeID"></param>
        /// <param name="IsEnable"></param>
        /// <returns></returns>
        [HttpGet("TeamList")]
        public async Task<ApiResult<IEnumerable<ProUserInfoViewModel>>> TeamList(int pageIndex, string Title, string ProjectID, int ProjectNodeID, bool? IsEnable, int? Role)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<ProUserInfoViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            if (Role == null)
                Role = 0;
            var parm = Expressionable.Create<ProjectUserInfo>()
                    .AndIF(!string.IsNullOrEmpty(Title), m => SqlFunc.Subqueryable<UserInfo>().Where(a => a.ID == m.UserID && a.NickName.Contains(Title)).Any())
                    .AndIF(IsEnable != null, m => m.IsEnable == IsEnable)
                    .AndIF(!string.IsNullOrEmpty(ProjectID), m => m.ProjectID == ProjectID)
                    .AndIF(ProjectNodeID != 0, m => m.ProjectNodeID == ProjectNodeID)
                    .AndIF(Role != 0, m => m.Role == Role);
            var list = projectUserdb.GetPages(parm.ToExpression(), new PageParm(pageIndex), "Role,Sorting DESC");
            List<ProUserInfoViewModel> list2 = new List<ProUserInfoViewModel>();
            if (list.DataSource != null)
            {
                foreach (var item in list.DataSource)
                {
                    ProUserInfoViewModel pm = new ProUserInfoViewModel();
                    pm.ID = item.ID;
                    pm.ProjectID = item.ProjectID;
                    pm.ProjectNodeID = item.ProjectNodeID;
                    pm.UserID = item.UserID;
                    UserInfo userInfo = userdb.GetId(item.UserID);
                    if (userInfo != null)
                    {
                        pm.UserName = userdb.GetNames(userInfo);
                    }
                    pm.Role = item.Role;
                    pm.Proportion = item.Proportion;
                    pm.AddDate = Utility.GetDateFormat(item.AddDate);
                    pm.IsEnable = item.IsEnable;
                    list2.Add(pm);
                }
                res.success = true;
                res.data = list2;
                res.index = pageIndex;
                res.count = list.TotalCount;
                res.size = list.PageSize;
                res.pages = list.TotalPages;
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
        /// 添加成员
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost("Addusers")]
        public async Task<ApiResult> Addusers(APIProUserViewModel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            List<ProjectUserInfo> projectUserInfos = projectUserdb.GetWhere(o => o.ProjectID == p.ProjectID && o.ProjectNodeID == p.ProjectNodeID && o.IsEnable == true && o.UserID == p.UserID);
            List<ProjectUserInfo> projectUserInfos2 = projectUserdb.GetWhere(o => o.ProjectID == p.ProjectID && o.ProjectNodeID == p.ProjectNodeID && o.IsEnable == true && o.Role == 1);
            if (projectUserInfos.Count > 0)
            {
                res.msg = "已有此人";
                res.success = false;
            }
            else if (projectUserInfos2.Count > 0 && p.Role == 1 && p.IsEnable == true)
            {
                res.msg = "已有组长，请修改";
                res.success = false;
            }
            else
            {
                try
                {
                    ProjectUserInfo m = new ProjectUserInfo()
                    {
                        ProjectID = p.ProjectID,
                        ProjectNodeID = p.ProjectNodeID,
                        UserID = p.UserID,
                        Role = p.Role,
                        Proportion = p.Proportion,
                        Sorting = p.Sorting,
                        AddDate = DateTime.Now,
                        IsEnable = p.IsEnable
                    };
                    res.success = projectUserdb.Add(m) > 0;
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
        /// 显示成员信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("TeamItem")]
        public async Task<ApiResult<ProUserInfoViewModel>> TeamItem(int ID)
        {
            var res = new ApiResult<ProUserInfoViewModel>();
            ProjectUserInfo m;
            ProUserInfoViewModel pm = new ProUserInfoViewModel();
            if (ID > 0)
            {
                m = projectUserdb.GetId(ID);
                if (m != null)
                {
                    pm.ID = m.ID;
                    pm.ProjectID = m.ProjectID;
                    pm.ProjectNodeID = m.ProjectNodeID;
                    pm.UserID = m.UserID;
                    UserInfo userInfo = userdb.GetId(m.UserID);
                    if (userInfo != null)
                    {
                        pm.UserName = userdb.GetNames(userInfo);
                    }
                    pm.Role = m.Role;
                    pm.Proportion = m.Proportion;
                    pm.AddDate = Utility.GetDateFormat(m.AddDate);
                    pm.IsEnable = m.IsEnable;

                    res.success = true;
                }
            }
            res.data = pm;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 修改成员
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPut("Updateusers")]
        public async Task<ApiResult> Updateusers(int id, APIProUserViewModel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            List<ProjectUserInfo> projectUserInfos = projectUserdb.GetWhere(o => o.ProjectID == p.ProjectID && o.ProjectNodeID == p.ProjectNodeID && o.IsEnable == true && o.UserID == p.UserID);
            List<ProjectUserInfo> projectUserInfos2 = projectUserdb.GetWhere(o => o.ProjectID == p.ProjectID && o.ProjectNodeID == p.ProjectNodeID && o.IsEnable == true && o.Role == 1);
            List<ProjectUserInfo> projectUserInfos3 = projectUserdb.GetWhere(o => o.ID == id && o.IsEnable == true && o.UserID == p.UserID);
            List<ProjectUserInfo> projectUserInfos4 = projectUserdb.GetWhere(o => o.ID == id && o.IsEnable == true && o.Role == 1&& o.UserID == p.UserID);
            if (projectUserInfos.Count > 0 && projectUserInfos3.Count == 0 && p.IsEnable == true)
            {
                res.msg = "已有此人";
                res.success = false;
            }
            else if (projectUserInfos2.Count > 0 && p.Role == 1 && p.IsEnable == true && projectUserInfos4.Count==0)
            {
                res.msg = "已有组长，请修改";
                res.success = false;
            }
            else
            {
                try
                {
                    ProjectUserInfo m = projectUserdb.GetId(id);
                    if (m != null)
                    {
                        m.ProjectID = p.ProjectID;
                        m.ProjectNodeID = p.ProjectNodeID;
                        m.UserID = p.UserID;
                        m.Role = p.Role;
                        m.Proportion = p.Proportion;
                        m.Sorting = p.Sorting;
                        m.IsEnable = p.IsEnable;
                        res.success = projectUserdb.Update(m) > 0;
                    }
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
        /// 成员删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("SetTeamState")]
        public async Task<ApiResult<string>> SetTeamState(string ID)
        {
            var res = new ApiResult<string>();
            if (!string.IsNullOrWhiteSpace(ID))
            {
                string[] array = ID.Trim(',').Split(',');
                int i = 0;
                foreach (string item in array)
                {
                    if (projectUserdb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new ProjectUserInfo { IsEnable = false }) > 0)
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
