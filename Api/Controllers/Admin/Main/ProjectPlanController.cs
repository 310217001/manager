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
    /// 项目计划
    /// </summary>
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Produces("application/json")]
    [Authorize(Policy = "Admin")]
    public class ProjectPlanController : Controller
    {
        private readonly IProjectNodeService PNdb;
        private readonly IUserService userdb;
        private readonly IProjectPlanService plandb;
        private readonly IPlanCommentService plancomdb;
        public ProjectPlanController(IUserService userdb, IProjectPlanService plandb, IPlanCommentService plancomdb, IProjectNodeService PNdb)
        {
            this.userdb = userdb;
            this.plandb = plandb;
            this.plancomdb = plancomdb;
            this.PNdb = PNdb;
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPlanViewModel")]
        public async Task<ApiResult<object>> GetPlanViewModel(string? ProjectID)
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
            var advertypename = PNdb.GetWhere(o=>o.IsEnable==true&&o.ProjectID==ProjectID);
            List<APITeamMembersViewModel2> atlist = new List<APITeamMembersViewModel2>();
            if (advertypename!=null)
            {
                foreach (var item in advertypename)
                {
                    atlist.Add(new APITeamMembersViewModel2(item.ID, item.Title));
                }
            }
            res.data = atlist;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 计划列表
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ProjectNodeID"></param>
        /// <param name="IsComplete"></param>
        /// <param name="IsEnable"></param>
        /// <returns></returns>
        [HttpGet("PlanList")]
        public async Task<ApiResult<IEnumerable<ProjectPlanViewModel>>> PlanList(string ProjectID, int? ProjectNodeID, bool? IsComplete, bool? IsEnable)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<ProjectPlanViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (ProjectNodeID==null)
            {
                ProjectNodeID = 0;
            }
            var parm = Expressionable.Create<ProjectPlanInfo>()
                    .AndIF(IsComplete != null,o=>o.IsComplete== IsComplete)
                    .AndIF(IsEnable != null, m => m.IsEnable == IsEnable)
                    .AndIF(!string.IsNullOrEmpty(ProjectID), m => m.ProjectID == ProjectID)
                    .AndIF(ProjectNodeID != 0, m => m.ProjectNodeID == ProjectNodeID);
            var list = plandb.GetNopage(parm.ToExpression(), "Sorting,ID DESC");
            List<ProjectPlanViewModel> list2 = new List<ProjectPlanViewModel>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    ProjectPlanViewModel pm = new ProjectPlanViewModel();
                    pm.ID = item.ID;
                    pm.ProjectID = item.ProjectID;
                    pm.ProjectNodeID = item.ProjectNodeID;
                    pm.UserID = item.UserID;
                    UserInfo userInfo = userdb.GetId(item.UserID);
                    if (userInfo != null)
                    {
                        pm.Principal = userdb.GetNames(userInfo);
                    }
                    pm.Contents = item.Contents;
                    pm.BeginDate = Utility.GetDateFormat(item.BeginDate);
                    pm.EndDate = Utility.GetDateFormat(item.EndDate);
                    pm.IsEnable = item.IsEnable;
                    pm.IsComplete = item.IsComplete;

                    var parm2 = Expressionable.Create<PlanCommentInfo>()
                    .AndIF(!string.IsNullOrEmpty(ProjectID), m => m.ProjectID == ProjectID)
                    .AndIF(ProjectNodeID != 0, m => m.ProjectNodeID == ProjectNodeID);
                    var list3 = plancomdb.GetNopage(parm2.ToExpression(), "ID DESC");
                    List<PlanCommentInfoViewModel> list4 = new List<PlanCommentInfoViewModel>();
                    if (list3 != null && list3.Count > 0)
                    {
                        foreach (var item2 in list3)
                        {
                            if (item2.UserID==item.UserID&&item.ID==item2.PlanID)
                            {
                                PlanCommentInfoViewModel pm2 = new PlanCommentInfoViewModel();
                                pm2.ID = item2.ID;
                                pm2.ProjectID = item2.ProjectID;
                                pm2.ProjectNodeID = item2.ProjectNodeID;
                                pm2.PlanID = item2.PlanID;
                                pm2.UserID = item2.UserID;
                                pm2.Title = item2.Title;
                                UserInfo userInfo2 = userdb.GetId(item2.UserID);
                                if (userInfo2 != null)
                                {
                                    pm2.UserName = userdb.GetNames(userInfo2);
                                }
                                pm2.AddDate = Utility.GetDateFormat(item2.AddDate);
                                pm2.IsEnable = item2.IsEnable;
                                pm2.IsConfirm = item2.IsConfirm;
                                list4.Add(pm2);
                            }
                        }
                        pm.planCommentInfoViews = list4;
                    }

                    list2.Add(pm);
                }
                res.success = true;
                res.data = list2;
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
        /// 添加计划
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost("PlanItemCha")]
        public async Task<ApiResult> PlanItemCha(APIProjectPlanViewmodel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            try
            {
                ProjectPlanInfo m = new ProjectPlanInfo()
                {
                    ProjectID = p.ProjectID,
                    ProjectNodeID = p.ProjectNodeID,
                    Principal = p.Principal,
                    UserID = p.Principal,
                    ProjectModifyID = p.ProjectModifyID,
                    BeginDate = C.DateTimes(p.BeginDate),
                    EndDate = C.DateTimes(p.EndDate),
                    CompleteDate = C.DateTimes(p.CompleteDate),
                    Sorting = p.Sorting,
                    IsComplete = p.IsComplete,
                    Contents = p.Contents,
                    AddDate = DateTime.Now,
                    IsEnable = p.IsEnable
                };
                res.success = plandb.Add(m) > 0;
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            // {"success":true,"message":null,"statusCode":200,"data":null}
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 计划显示详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("ItemPlan")]
        public async Task<ApiResult<ProjectPlanViewModel>> ItemPlan(int ID)
        {
            var res = new ApiResult<ProjectPlanViewModel>();
            ProjectPlanInfo m;
            ProjectPlanViewModel pm = new ProjectPlanViewModel();
            if (ID > 0)
            {
                m = plandb.GetId(ID);
                if (m != null)
                {
                    pm.ID = m.ID;
                    pm.ProjectID = m.ProjectID;
                    pm.ProjectNodeID = m.ProjectNodeID;
                    pm.UserID = m.UserID;
                    UserInfo userInfo = userdb.GetId(m.UserID);
                    if (userInfo != null)
                    {
                        pm.Principal = userdb.GetNames(userInfo);
                    }
                    pm.Contents = m.Contents;
                    pm.BeginDate = Utility.GetDateFormat(m.BeginDate);
                    pm.EndDate = Utility.GetDateFormat(m.EndDate);
                    pm.IsEnable = m.IsEnable;
                    pm.IsComplete = m.IsComplete;
                    res.success = true;
                }
            }
            res.data = pm;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 修改计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPut("PlanItemCha")]
        public async Task<ApiResult> PlanItemCha(int id, APIProjectPlanViewmodel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            try
            {
                ProjectPlanInfo m = plandb.GetId(id);
                //m.ProjectID = p.ProjectID;
                //m.ProjectNodeID = p.ProjectNodeID;
                m.Principal = p.Principal;
                m.ProjectModifyID = p.ProjectModifyID;
                m.BeginDate = C.DateTimes(p.BeginDate);
                m.EndDate = C.DateTimes(p.EndDate);
                m.CompleteDate = C.DateTimes(p.CompleteDate);
                m.IsComplete = p.IsComplete;
                m.IsEnable = p.IsEnable;
                m.Sorting = p.Sorting;
                m.Contents = p.Contents;
                res.success = plandb.Update(m) > 0;
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            // {"success":true,"message":null,"statusCode":200,"data":null}
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 计划删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("SetPlanState")]
        public async Task<ApiResult<string>> SetPlanState(string ID)
        {
            var res = new ApiResult<string>();
            if (!string.IsNullOrWhiteSpace(ID))
            {
                string[] array = ID.Trim(',').Split(',');
                int i = 0;
                foreach (string item in array)
                {
                    if (plandb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new ProjectPlanInfo { IsEnable = false }) > 0)
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
        /// 添加计划日志
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost("PlanComCha")]
        public async Task<ApiResult> PlanComCha(APIPlanCommentViewModel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            try
            {
                PlanCommentInfo m = new PlanCommentInfo()
                {
                    ProjectID = p.ProjectID,
                    ProjectNodeID = p.ProjectNodeID,
                    PlanID = p.PlanID,
                    UserID = p.UserID,
                    Title = p.Title,
                    IsConfirm = p.IsConfirm,
                    IsEnable = p.IsEnable,
                    AddDate = DateTime.Now,
                };
                res.success = plancomdb.Add(m) > 0;
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            // {"success":true,"message":null,"statusCode":200,"data":null}
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 计划日志显示详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("ItemPlanCom")]
        public async Task<ApiResult<PlanCommentInfoViewModel>> ItemPlanCom(int ID)
        {
            var res = new ApiResult<PlanCommentInfoViewModel>();
            PlanCommentInfo m;
            PlanCommentInfoViewModel pm = new PlanCommentInfoViewModel();
            if (ID > 0)
            {
                m = plancomdb.GetId(ID);
                if (m != null)
                {
                    pm.ID = m.ID;
                    pm.ProjectID = m.ProjectID;
                    pm.ProjectNodeID = m.ProjectNodeID;
                    pm.PlanID = m.PlanID;
                    pm.UserID = m.UserID;
                    pm.Title = m.Title;
                    UserInfo userInfo = userdb.GetId(m.UserID);
                    if (userInfo != null)
                    {
                        pm.UserName = userdb.GetNames(userInfo);
                    }
                    pm.AddDate = Utility.GetDateFormat(m.AddDate);
                    pm.IsEnable = m.IsEnable;
                    pm.IsConfirm = m.IsConfirm;
                    res.success = true;
                }
            }
            res.data = pm;
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 修改计划日志
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPut("PlanComCha")]
        public async Task<ApiResult> PlanComCha(int id, APIPlanCommentViewModel p)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            try
            {
                PlanCommentInfo m = plancomdb.GetId(id);
                m.ProjectID = p.ProjectID;
                m.ProjectNodeID = p.ProjectNodeID;
                m.PlanID = p.PlanID;
                m.UserID = p.UserID;
                m.Title = p.Title;
                m.IsConfirm = p.IsConfirm;
                m.IsEnable = p.IsEnable;
                res.success = plancomdb.Update(m) > 0;
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            // {"success":true,"message":null,"statusCode":200,"data":null}
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 计划日志删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("SetPlanComState")]
        public async Task<ApiResult<string>> SetPlanComState(string ID)
        {
            var res = new ApiResult<string>();
            if (!string.IsNullOrWhiteSpace(ID))
            {
                string[] array = ID.Trim(',').Split(',');
                int i = 0;
                foreach (string item in array)
                {
                    if (plancomdb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new PlanCommentInfo { IsEnable = false }) > 0)
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
