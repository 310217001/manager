using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Pi.Common;
using Pi.Common.Security;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Controllers;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.System
{
    /// <summary>
    /// 用户
    /// </summary>
    [Authorize(Policy = "Admin")]
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.AdminSystem)]
    public class UserController : BaseController
    {
        private readonly IUserService db;
        private readonly IRoleService roleDb;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(IUserService db, IRoleService roleDb, IWebHostEnvironment _webHostEnvironment)
        {
            this.db = db;
            this.roleDb = roleDb;
            this._webHostEnvironment = _webHostEnvironment;
        }

        /// <summary>
        /// 自己详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("Item")]
        public async Task<ApiResult<object>> Item()
        {
            var res = new ApiResult<object>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                res.data = db.GetId(UserID);
                if (res.data != null)
                    res.success = true;
                else
                {
                    res.msg = "无数据";
                    res.statusCode = (int)ApiEnum.Status;
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
        /// 选择用户查看
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResult<object>> Item(int id)
        {
            var res = new ApiResult<object>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                UserInfo m;
                SysUserViewModel d;
                if (id > 0)
                {
                    d = new SysUserViewModel();
                    m = db.GetId(id);
                    if (m != null)
                    {
                        d.ID = m.ID;
                        d.UserName = m.UserName;
                        d.Password = m.Password;
                        d.HeadPortrait = m.HeadPortrait;
                        d.NickName = m.NickName;
                        d.FullName = m.FullName;
                        d.Position = m.Position;
                        d.IdCard = Utility.IdCardUnEncrypt(m.IdCard);
                        d.Gender = m.Gender;
                        d.MobilePhone = m.MobilePhone;
                        d.QQ = m.QQ;
                        d.Mail = m.Mail;
                        d.State = m.State;
                        d.RoleID = m.RoleID;
                    }
                    else
                    {
                        res.msg = "参数丢失";
                        res.statusCode = (int)ApiEnum.Status;
                    }
                }
                else
                {
                    d = new SysUserViewModel();
                    // 设置默认值用于页面显示
                    d.RoleID = 0;
                    d.State = 2;
                }
                res.data = d;
                if (res.data!=null)
                {
                    res.success = true;
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
        /// 获取角色全部信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoleViewModel")]
        public async Task<ApiResult<IEnumerable<BaseViewModel>>> GetRoleViewModel()
        {
            var roleList = roleDb.GetAll();
            var res = new ApiResult<IEnumerable<BaseViewModel>>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                List<BaseViewModel> roles = new List<BaseViewModel>();
                if (roleList != null)
                {
                    foreach (var item in roleList)
                    {
                        roles.Add(new BaseViewModel(item.ID, item.Names));
                    }
                    res.data = roles;
                    if (res.data != null)
                    {
                        res.success = true;
                        res.statusCode = (int)ApiEnum.Status;
                    }
                    else
                    {
                        res.msg = "无数据";
                    }
                }
                else
                {
                    res.msg = "参数丢失";
                }
            }
            else
            {
                res.msg = "无法获取用户信息！";
                res.statusCode = (int)ApiEnum.Status;
            }

            return await Task.Run(() => res);
        }

        /// <summary>
        /// 列表显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<SysUserViewModel>>> List(int pageIndex, int RoleID, int StateID, string Title, string Phone)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<SysUserViewModel>>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                if (pageIndex == 0) pageIndex = 1;
                List<SysUserViewModel> list = new List<SysUserViewModel>();
                var parm = Expressionable.Create<UserInfo>()
                    .AndIF(!string.IsNullOrEmpty(Title), m => m.FullName.Contains(Title) || m.NickName.Contains(Title))
                    .AndIF(RoleID != 0, m => m.RoleID == RoleID)
                    .AndIF(StateID != 0, m => m.State == StateID)
                    .AndIF(!string.IsNullOrEmpty(Phone), m => m.MobilePhone.Contains(Phone));
                var Paged = db.GetPages(parm.ToExpression(), new PageParm(pageIndex), "ID DESC");
                var data = Paged.DataSource;
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        SysUserViewModel sysUserView = new SysUserViewModel();
                        sysUserView.ID = item.ID;
                        sysUserView.UserName = item.UserName;
                        sysUserView.Password = item.Password;
                        sysUserView.HeadPortrait = item.HeadPortrait;
                        sysUserView.NickName = item.NickName;
                        sysUserView.FullName = item.FullName;
                        sysUserView.Position = item.Position;
                        sysUserView.IdCard = item.IdCard;
                        sysUserView.Gender = item.Gender;
                        sysUserView.MobilePhone = item.MobilePhone;
                        sysUserView.QQ = item.QQ;
                        sysUserView.Mail = item.Mail;
                        sysUserView.State = item.State;
                        sysUserView.AddDate = Utility.GetDateFormat(item.AddDate);
                        if (item.RoleID == 0)
                        {
                            sysUserView.RoleName = "";
                        }
                        else
                        {
                            sysUserView.RoleName = roleDb.GetId(item.RoleID).Names;
                        }
                        list.Add(sysUserView);
                    }
                    res.data = list;
                    if (res.data != null)
                    {
                        res.success = true;
                        res.index = pageIndex;
                        res.count = Paged.TotalCount;
                        res.size = Paged.PageSize;
                    }
                    else
                    {
                        res.msg = "无数据";
                        res.statusCode = (int)ApiEnum.Status;
                    }
                }
                else
                {
                    res.msg = "参数丢失";
                    res.statusCode = (int)ApiEnum.Status;
                }
            }
            else
            {
                res.msg = "无法获取用户信息！";
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<ApiResult<string>> Delete(string ID)
        {
            var res = new ApiResult<string>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    string[] array = ID.Trim(',').Split(',');
                    if (array != null && array.Length > 0)
                    {
                        int[] array2 = Array.ConvertAll(array, int.Parse);
                        foreach (int item in array2)
                        {
                            if (db.Update(a => a.ID == item, a => new UserInfo { State = 4 }) > 0)
                                res.count++;
                        }
                        res.success = res.count > 0;
                        if (res.success)
                        {
                            res.msg = "删除成功";
                        }
                        else
                        {
                            res.msg = "删除失败";
                            res.statusCode = (int)ApiEnum.Status;
                        }
                    }
                }
            }
            else
            {
                res.msg = "无法获取用户信息！";
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> SysItem(APIUserViewModel vm)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<string>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                if (!string.IsNullOrWhiteSpace(vm.UserName))
                {
                    try
                    {
                        UserInfo m = new UserInfo();
                        m.UserName = vm.UserName;
                        m.HeadPortrait = vm.HeadPortrait;
                        m.NickName = vm.NickName;
                        m.FullName = vm.FullName;
                        m.Position = vm.Position;
                        m.IdCard = Utility.IdCardEncrypt(vm.IdCard);
                        m.Gender = vm.Gender;
                        m.MobilePhone = vm.MobilePhone;
                        m.QQ = vm.QQ;
                        m.Mail = vm.Mail;
                        m.State = vm.State;
                        m.RoleID = vm.RoleID;

                        // 设置默认密码
                        if (string.IsNullOrWhiteSpace(vm.Password))
                            vm.Password = "888888";
                        // 如果设置了密码、就进行加密
                        if (!string.IsNullOrWhiteSpace(vm.Password))
                            m.Password = MD5Encode.GetEncrypt(vm.Password);
                        m.HeadPortrait = "";
                        m.PiUserID = ""; ;
                        m.LastLoginData = m.AddDate = DateTime.Now;
                        res.success = db.Add(m) > 0;
                        if (res.success)
                        {
                            res.msg = "添加成功";
                        }
                        else
                        {
                            res.msg = "添加失败";
                            res.statusCode = (int)ApiEnum.Status;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.statusCode = (int)ApiEnum.Error;
                        res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                    }
                }
                else
                    res.msg = "参数丢失";
            }
            else
            {
                res.msg = "无法获取用户信息！";
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<string>> SysItem2(int ID, APIUserViewModel vm)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<string>();
            int UserID = GetUserID();
            if (UserID > 0)
            {
                if (!string.IsNullOrWhiteSpace(vm.UserName))
                {
                    UserInfo m = db.GetId(ID);
                    if (m != null)
                    {
                        m.ID = ID;
                        m.UserName = vm.UserName;
                        m.HeadPortrait = vm.HeadPortrait;
                        m.NickName = vm.NickName;
                        m.FullName = vm.FullName;
                        m.Position = vm.Position;
                        m.IdCard = Utility.IdCardEncrypt(vm.IdCard);
                        m.Gender = vm.Gender;
                        m.MobilePhone = vm.MobilePhone;
                        m.QQ = vm.QQ;
                        m.Mail = vm.Mail;
                        m.State = vm.State;
                        m.RoleID = vm.RoleID;

                        // 设置默认密码
                        if (string.IsNullOrWhiteSpace(vm.Password))
                            vm.Password = "888888";
                        // 如果设置了密码、就进行加密
                        if (!string.IsNullOrWhiteSpace(vm.Password))
                            m.Password = MD5Encode.GetEncrypt(vm.Password);
                    }
                    else
                    {
                        res.msg = "参数丢失";
                    }
                    try
                    {
                        res.success = db.Update(m) > 0;
                        if (res.success)
                        {
                            res.msg = "修改成功";
                        }
                        else
                        {
                            res.msg = "修改失败";
                            res.statusCode = (int)ApiEnum.Status;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.statusCode = (int)ApiEnum.Error;
                        res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                    }
                }
                else
                    res.msg = "参数丢失";
            }
            else
            {
                res.msg = "无法获取用户信息！";
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// a标签页面跳转导出
        /// </summary>
        /// <param name="RoleID">选择用户</param>
        /// <param name="StateID">选择状态</param>
        /// <param name="Title">选择标题</param>
        /// <param name="Phone">选择手机号</param>
        /// <returns></returns>
        [HttpGet("Export")]
        public IActionResult Export(int RoleID, int StateID, string Title, string Phone)
        {
            // 方法2：文件是生成在内存中
            string fileName = $"{Guid.NewGuid().ToString()}.xlsx";
            //store in memory rather than pysical directory
            var stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                // add worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                // 表头
                worksheet.Cells[1, 1].Value = "用户名";
                worksheet.Cells[1, 2].Value = "昵称";
                worksheet.Cells[1, 3].Value = "姓名";
                worksheet.Cells[1, 4].Value = "手机";

                // 获取数据
                List<SysUserViewModel> list = new List<SysUserViewModel>();
                var parm = Expressionable.Create<UserInfo>()
                    .AndIF(RoleID != 0, m => m.RoleID == RoleID)
                    .AndIF(StateID != 0, m => m.State == StateID)
                    .AndIF(!string.IsNullOrEmpty(Title), m => m.FullName.Contains(Title) || m.NickName.Contains(Title))
                    .AndIF(!string.IsNullOrEmpty(Phone), m => m.MobilePhone.Contains(Phone));
                List<UserInfo> list1 = db.GetWhere(parm.ToExpression());
                if (list1 != null && list1.Count > 0)
                {
                    int a = 2;
                    foreach (var item in list1)
                    {
                        worksheet.Cells["A" + a].Value = item.UserName;
                        worksheet.Cells["B" + a].Value = item.NickName;
                        worksheet.Cells["C" + a].Value = item.FullName;
                        worksheet.Cells["D" + a].Value = item.MobilePhone;
                        a++;
                    }
                }
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("Import")]
        public ApiResult<IEnumerable<SysUserViewModel>> Import(IFormFile excelfile)
        {
            var res = new ApiResult<IEnumerable<SysUserViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (excelfile != null)
            {
                List<UserInfo> userInfos = new List<UserInfo>();    // 更新的列表
                List<UserInfo> userInfos2 = new List<UserInfo>();   // 添加的列表
                UserInfo sysUserView;
                string sWebRootFolder = _webHostEnvironment.WebRootPath + "\\UploadFiles\\excel\\import";
                string sFileName = $"{Guid.NewGuid()}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                try
                {
                    using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
                    {
                        excelfile.CopyTo(fs);
                        fs.Flush();
                    }
                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        int rowCount = worksheet.Dimension.Rows;
                        int ColCount = worksheet.Dimension.Columns;
                        DateTime now = DateTime.Now;
                        string password = MD5Encode.GetEncrypt("888888");
                        for (int row = 2; row <= rowCount; row++)
                        {
                            // 查询账号是否已经存在 - 如果单次数据超过100条建议把所有用户取出来对比
                            sysUserView = db.GetFirst(o => o.UserName == C.String(worksheet.Cells[row, 2].Value));
                            if (sysUserView != null)
                            {
                                userInfos.Add(new UserInfo
                                {
                                    UserName = C.String(worksheet.Cells[row, 1].Value),
                                    NickName = C.String(worksheet.Cells[row, 2].Value),
                                    FullName = C.String(worksheet.Cells[row, 3].Value),
                                    MobilePhone = C.String(worksheet.Cells[row, 4].Value),
                                });
                            }
                            else
                            {
                                userInfos2.Add(new UserInfo
                                {
                                    UserName = C.String(worksheet.Cells[row, 1].Value),
                                    NickName = C.String(worksheet.Cells[row, 2].Value),
                                    FullName = C.String(worksheet.Cells[row, 3].Value),
                                    MobilePhone = C.String(worksheet.Cells[row, 4].Value),
                                    RoleID = 4,    // 用户
                                    State = 2,     // 已审
                                    AddDate = now,
                                    Password = password,
                                });
                            }
                        }
                    }
                    // 新增或更新
                    int addCount = 0, updateCount = 0;
                    if (userInfos.Count > 0)
                    {
                        addCount = db.Update(userInfos);
                        res.msg += $"导入{addCount}条，";
                    }
                    if (userInfos2.Count > 0)
                    {
                        updateCount = db.Add(userInfos2);
                        res.msg += $"更新{addCount}条，";
                    }
                    // 结果
                    res.success = addCount > 0 || updateCount > 0;
                    if (res.msg != null)
                        res.msg = res.msg.TrimEnd('，');
                }
                catch (Exception ex)
                {
                    res.success = false;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            return res;
        }
    }
}
