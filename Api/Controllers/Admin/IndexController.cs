using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.Common.Security;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Api.Helper;
using Pi.PiManager.Controllers;
using Pi.PiManager.IService;
using Pi.PiManager.Model;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.Models.Project;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Pi.PiManager.Api.Extensions.JwtHelper;

namespace Pi.PiManager.Api.Controllers.Admin
{
    /// <summary>
    /// 首页
    /// </summary>
    [Authorize(Policy = "Admin")]
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.AdminAuth)]
    public class IndexController : BaseController
    {
        private readonly IUserService db;
        private readonly IRoleService roleDB;
        private readonly IProjectInfoService Projectdb;
        private readonly IMenuService menuDB;
        private readonly IProjectUserService projectUserdb;
        private readonly IProjectNodeService PNdb;
        public IndexController(IMenuService menuDB, IRoleService roleDB, IUserService db, IProjectUserService projectUserdb, IProjectNodeService PNdb, IProjectInfoService Projectdb)
        {
            this.menuDB = menuDB;
            this.roleDB = roleDB;
            this.db = db;
            this.projectUserdb = projectUserdb;
            this.PNdb = PNdb;
            this.Projectdb = Projectdb;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdatePwd")]
        [Authorize(Policy = "Admin")]
        public async Task<ApiResult<object>> UpdatePwd(string Oldpassword, string Newpassword1, string Newpassword2)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<object>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                TokenModelJwt tokenModelJwt = TokenDecode();
                int UID = C.Int(tokenModelJwt.Uid);
                try
                {
                    if (Newpassword1 == Newpassword2)
                    {

                        UserInfo m = db.GetId(UID);
                        if (m != null)
                        {
                            var user = db.GetFirst(a => a.UserName == m.UserName && a.Password == MD5Encode.GetEncrypt(Oldpassword));
                            if (user == null)
                            {
                                res.statusCode = (int)ApiEnum.ParameterError;
                                res.msg = "原密码错误！";
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(Newpassword1))
                                {
                                    m.Password = MD5Encode.GetEncrypt(Newpassword1);
                                    res.success = db.Update(m) > 0;
                                }
                                else
                                {
                                    res.statusCode = (int)ApiEnum.ParameterError;
                                    res.msg = "密码不能为空！";
                                }
                                if (res.success)
                                    res.statusCode = (int)ApiEnum.Status;
                            }
                        }
                        else
                        {
                            res.statusCode = (int)ApiEnum.Status;
                            res.msg = "无该账号";
                        }
                    }
                    else
                    {
                        res.statusCode = (int)ApiEnum.ParameterError;
                        res.msg = "两次密码不一样！";
                    }
                }
                catch (Exception ex)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            else
            {
                res.statusCode = (int)ApiEnum.Status;
                res.msg = "无法获取用户信息！";
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 首页下拉菜单接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<object>> Index()
        {
            var res = new ApiResult<object>();
            // 获取登录用户数据
            int UserID = GetUserID();
            if (UserID > 0)
            {
                UserInfo user = db.GetId(UserID);
                if (user != null)
                {
                    if (user != null && user.RoleID > 0)
                    {
                        Role role = roleDB.GetId(user.RoleID);
                        if (role != null)
                        {
                            // 后台菜单
                            List<MenuViewModel> list = new List<MenuViewModel>();
                            var menuList = menuDB.GetWhere(a => a.IsEnable);
                            IEnumerable<Menu> list2 = menuList;
                            if (list2 != null && list2.Count() > 0)
                            {
                                list2 = list2.OrderBy(a => a.Sorting);
                                IEnumerable<Menu> mList1 = list2.Where(a => a.FID == 0);
                                if (mList1 != null && mList1.Count() > 0)
                                {
                                    foreach (var item in mList1)
                                    {
                                        if (role.ID == 1 || role.Menus.Contains("," + item.ID + ","))
                                        {
                                            list.Add(new MenuViewModel
                                            {
                                                name = item.Names,
                                                url = item.Url,
                                                icon = item.Icon,
                                                childMenus = APIHelper.GetMenu(item.ID, list2, role),
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    res.msg = "参数丢失";
                                }
                            }
                            else
                            {
                                res.msg = "参数丢失";
                            }
                            res.data = list;
                            if (res.data != null)
                            {
                                res.success = true;
                            }
                        }
                        else
                        {
                            res.msg = "参数丢失";
                        }
                    }
                    else
                    {
                        res.msg = "参数丢失";
                    }
                }
                else
                {
                    res.msg = "无该用户";
                }
            }
            return await Task.Run(() => res);
            //// 广告
            //List<AdverViewModel> adverList = new List<AdverViewModel>();
            //var adverData = adverDb.GetPages(a => a.IsEnable == true && a.TypeID == 1, new PageParm(1, 5), "Sorting,ID");
            //if (adverData.DataSource != null)
            //{
            //    foreach (var item in adverData.DataSource)
            //    {
            //        adverList.Add(IMapper.Map<AdverViewModel>(item));
            //    }
            //}
            //// 文章
            //List<ArticleViewModel> articleList = new List<ArticleViewModel>();
            //var articleData = articleService.GetPages(a => a.State == 2, new PageParm(1, 5), "Sorting,ID DESC");
            //if (articleData.DataSource != null)
            //{
            //    foreach (var item in articleData.DataSource)
            //    {
            //        articleList.Add(IMapper.Map<ArticleViewModel>(item));
            //    }
            //}
            //// 返回结果
            //res.success = true;
            //res.data = new
            //{
            //    adverList,
            //    articleList
            //};
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("Index2")]
        public async Task<ApiResult<object>> Index2()
        {
            var res = new ApiResult<object>();
            // 获取登录用户数据
            int UserID = GetUserID();
            if (UserID > 0)
            {
                UserInfo userInfo = db.GetId(UserID);
                if (userInfo != null)
                {
                    res.data = db.GetNames(userInfo);
                    res.success = true;
                }
                else
                {
                    res.msg = "参数丢失";
                }
            }
            else
            {
                res.msg = "无该用户";
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 欢迎页展示
        /// </summary>
        /// <returns></returns>
        [HttpGet("WelcomeNodesList")]
        public async Task<ApiResult<APIProNodesListWelViewModel>> WelcomeNodesList()
        {
            var res = new ApiResult<APIProNodesListWelViewModel>() { statusCode = (int)ApiEnum.Status };
            int UserID = GetUserID();
            if (UserID > 0)
            {
                var parm = Expressionable.Create<ProjectNodeInfo>();
                parm.And(o => o.IsEnable == true);
                List<ProjectNodeInfo> list = null;
                UserInfo userInfo = db.GetId(UserID);
                List<ProjectUserInfo> projectUserInfo = projectUserdb.GetWhere(o => o.UserID == UserID && o.IsEnable == true);
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

                }
                list = PNdb.GetNopage(parm.ToExpression(), "ID DESC");
                APIProNodesListWelViewModel aPIProNodesListWelViewModel = new APIProNodesListWelViewModel();
                if (list != null && list.Count > 0)
                {
                    IEnumerable<ProjectNodeInfo> list2 = list.Where(o => o.State == 1 || o.State == 2);
                    List<APIProNodesWelViewModel> CompleteList = new List<APIProNodesWelViewModel>();
                    List<APIProNodesWelViewModel> UnfinishedList = new List<APIProNodesWelViewModel>();
                    List<APIProNodesWelViewModel> DelayList = new List<APIProNodesWelViewModel>();
                    foreach (var item in list2)
                    {
                        ProjectInfo projectInfo = Projectdb.Get("Projects", item.ProjectID);
                        if (projectInfo != null)
                        {
                            //APIProNodesWelViewModel aPIProNodesWelViewModel = new APIProNodesWelViewModel();
                            //aPIProNodesWelViewModel.ProName = projectInfo.Names;
                            //aPIProNodesWelViewModel.ProNodeTitle = item.Title;
                            //aPIProNodesListWelViewModel.CompleteList.Add(aPIProNodesWelViewModel);
                            CompleteList.Add(new APIProNodesWelViewModel
                            {
                                ProName = projectInfo.Names,
                                ProNodeTitle = item.Title
                            });
                        }
                    }
                    aPIProNodesListWelViewModel.CompleteList = CompleteList;
                    IEnumerable<ProjectNodeInfo> list3 = list.Where(o => o.State == 0);
                    foreach (var item in list3)
                    {
                        ProjectInfo projectInfo = Projectdb.Get("Projects", item.ProjectID);
                        if (projectInfo != null)
                        {
                            UnfinishedList.Add(new APIProNodesWelViewModel
                            {
                                ProName = projectInfo.Names,
                                ProNodeTitle = item.Title
                            });
                        }
                    }
                    aPIProNodesListWelViewModel.UnfinishedList = UnfinishedList;
                    IEnumerable<ProjectNodeInfo> list4 = list.Where(o => o.State == 3 || o.State == 4);
                    foreach (var item in list4)
                    {
                        ProjectInfo projectInfo = Projectdb.Get("Projects", item.ProjectID);
                        if (projectInfo != null)
                        {
                            DelayList.Add(new APIProNodesWelViewModel
                            {
                                ProName = projectInfo.Names,
                                ProNodeTitle = item.Title
                            });
                        }
                    }
                    aPIProNodesListWelViewModel.DelayList = DelayList;
                }
                res.data = aPIProNodesListWelViewModel;
                res.success = true;
            }
            else
            {
                res.msg = "请重新登陆";
            }
            return await Task.Run(() => res);
        }
    }
}
