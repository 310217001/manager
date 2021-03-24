using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Api.Helper;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.System
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class PermissionsController : ControllerBase
    {
        private readonly IUserService db;
        private readonly IRoleService roleDb;
        private readonly IMenuService menuDb;
        public PermissionsController(IUserService db, IRoleService roleDb, IMenuService menuDb)
        {
            this.db = db;
            this.roleDb = roleDb;
            this.menuDb = menuDb;
        }
        /// <summary>
        /// 获取角色数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRole")]
        public async Task<ApiResult<object>> GetRoles()
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.ParameterError };
            var roleList = roleDb.GetAll();
            if (roleList != null)
            {
                List<BaseViewModel> roles = new List<BaseViewModel>();
                foreach (var item in roleList)
                roles.Add(new BaseViewModel(item.ID, item.Names));
                res.success = true;
                res.data = roles;
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenu")]
        public async Task<ApiResult<object>> GetMenus()
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.ParameterError };
            List<ELTreeViewModel> list = new List<ELTreeViewModel>();
            var data = menuDb.GetWhere(a => a.IsEnable, a => a.Sorting);
            if (data != null)
            {
                list.Add(new ELTreeViewModel
                {
                    label = "根目录",
                    children = GetMenusHelps.GetChildren(data, 0)
                });
                res.success = true;
                res.data = list;
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 根据id显示权限
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpGet("UserList")]
        public async Task<ApiResult<IEnumerable<BaseViewModel>>> UserList(int RoleID)
        {
            var res = new ApiResult<IEnumerable<BaseViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (RoleID > 0)
            {
                var list = db.GetPages(a => a.RoleID == RoleID && a.State == 2, new PageParm(1, 50));
                if (list.DataSource != null)
                {
                    List<BaseViewModel> userList = new List<BaseViewModel>();
                    foreach (var item in list.DataSource)
                        userList.Add(new BaseViewModel(item.ID, item.UserName + (item.FullName != "" ? "(" + item.FullName + ")" : item.NickName)));
                    res.data = userList;
                    res.count = list.TotalCount;
                }
                res.success = true;

                // 获取菜单权限
                Role role = roleDb.GetId(RoleID);
                if (role != null)
                    res.msg = role.Menus;
            }
            else
                res.msg = "参数丢失";
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 更新权限菜单
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="Menus"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> SetMenus(int RoleID, string Menus)
        {
            var res = new ApiResult();
            if (RoleID > 0)
            {
                if (!string.IsNullOrWhiteSpace(Menus))
                    Menus = "," + Menus + ",";
                res.success = roleDb.Update(a => a.ID == RoleID, a => new Role { Menus = Menus }) > 0;
            }
            else
                res.msg = "参数丢失";
            return await Task.Run(() => res);
        }
    }
}
