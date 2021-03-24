using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models.Main;
using Pi.PiManager.Model.ViewModels.API;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Main
{
    /// <summary>
    /// 服务器
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    public class ServerController : Controller
    {
        private readonly IServerInfoService ServerInfodb;
        private readonly IProviderService providerdb;
        private readonly ICertificateService Certificatedb;
        private readonly IProjectInfoService Projectdb;
        private readonly IMapper IMapper;
        public ServerController(IServerInfoService ServerInfodb, IProviderService providerdb, ICertificateService Certificatedb, IMapper IMapper, IProjectInfoService Projectdb)
        {
            this.ServerInfodb = ServerInfodb;
            this.providerdb = providerdb;
            this.Certificatedb = Certificatedb;
            this.Projectdb = Projectdb;
            this.IMapper = IMapper;
        }
        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="Title">服务器名</param>
        /// <param name="ProjectID">项目ID</param>
        /// <param name="ProviderID">服务商ID</param>
        /// <param name="System">操作系统</param>
        /// <param name="RegisterDate">注册时间</param>
        /// <param name="ExpireDate">到期时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<ServerInfoViewModel>>> List(int pageIndex, string Title, string ProjectID, int ProviderID, string System, string RegisterDate, string ExpireDate)
        {
            var res = new ApiResult<IEnumerable<ServerInfoViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            DateTime? RegisterDate1 = null;
            DateTime? ExpireDate1 = null;
            if (!string.IsNullOrWhiteSpace(RegisterDate))
            {
                RegisterDate1 = C.DateTimes(RegisterDate);
            }
            if (!string.IsNullOrWhiteSpace(ExpireDate))
            {
                ExpireDate1 = C.DateTimes(ExpireDate);
            }
            var parm = Expressionable.Create<ServerInfo>()
             .AndIF(!string.IsNullOrEmpty(Title), m => m.Title.Contains(Title))
             .AndIF(!string.IsNullOrEmpty(ProjectID), m => m.ProjectID == ProjectID)
             .AndIF(ProviderID != 0, m => m.ProviderID == ProviderID)
             .AndIF(!string.IsNullOrEmpty(System), m => m.System.Contains(System))
             .AndIF(RegisterDate1 != null, m => m.RegisterDate >= RegisterDate1)
             .AndIF(ExpireDate1 != null, m => m.ExpireDate <= ExpireDate1);
            var list = ServerInfodb.GetPages(parm.ToExpression(), new PageParm(pageIndex));
            List<ServerInfoViewModel> list2 = new List<ServerInfoViewModel>();
            if (list != null)
            {
                foreach (var item in list.DataSource)
                {
                    ServerInfoViewModel n = IMapper.Map<ServerInfoViewModel>(item);
                    n.ProjectName = Projectdb.Get("Projects", item.ProjectID).Names;
                    n.ProviderName = providerdb.GetId(item.ProviderID).Title;
                    list2.Add(n);
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
        /// <param name="ID">项目类型ID</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<ServerInfoViewModel>> Item(string ID)
        {
            var res = new ApiResult<ServerInfoViewModel>();
            ServerInfo m;
            ServerInfoViewModel n = new ServerInfoViewModel();
            if (!string.IsNullOrEmpty(ID))
            {
                m = ServerInfodb.GetId(ID);
                if (m != null)
                {
                    n = IMapper.Map<ServerInfoViewModel>(m);
                    n.ProjectName = Projectdb.Get("Projects", m.ProjectID).Names;
                    n.ProviderName = providerdb.GetId(m.ProviderID).Title;
                    res.success = true;
                }
            }
            res.data = n;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add(APIServerInfoViewModel serverInfo)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(serverInfo.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {
                    ServerInfo m = new ServerInfo()
                    {
                        Title = serverInfo.Title,
                        ProjectID = serverInfo.ProjectID,
                        ProviderID = serverInfo.ProviderID,
                        ProviderAccount = serverInfo.ProviderAccount,
                        ProviderPassword = serverInfo.ProviderPassword,
                        // ProviderPassword = !string.IsNullOrWhiteSpace(serverInfo.ProviderPassword) ? MD5Encode.GetEncrypt(serverInfo.ProviderPassword) : MD5Encode.GetEncrypt("888888"),
                        ServerAccount = serverInfo.ServerAccount,
                        ServerPassword = serverInfo.ServerPassword,
                        System = serverInfo.System,
                        Remarks = serverInfo.Remarks,
                        IP = serverInfo.IP,
                        RegisterDate = C.DateTimes(serverInfo.RegisterDate),
                        ExpireDate = C.DateTimes(serverInfo.ExpireDate),
                        IsEnable = serverInfo.IsEnable
                    };
                    res.success = ServerInfodb.Add(m) > 0;
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
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update(int id, APIServerInfoViewModel serverInfo)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(serverInfo.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {

                    ServerInfo m = ServerInfodb.GetId(id);

                    m.Title = serverInfo.Title;
                    m.ProjectID = serverInfo.ProjectID;
                    m.ProviderID = serverInfo.ProviderID;
                    m.ProviderAccount = serverInfo.ProviderAccount;
                    m.ProviderPassword = serverInfo.ProviderPassword;
                    m.ServerAccount = serverInfo.ServerAccount;
                    m.ServerPassword = serverInfo.ServerPassword;
                    m.System = serverInfo.System;
                    m.Remarks = serverInfo.Remarks;
                    m.IP = serverInfo.IP;
                    m.RegisterDate = C.DateTimes(serverInfo.RegisterDate);
                    m.ExpireDate = C.DateTimes(serverInfo.ExpireDate);
                    m.IsEnable = serverInfo.IsEnable;
                    res.success = ServerInfodb.Update(m) > 0;
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
                    if (ServerInfodb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new ServerInfo { IsEnable = false }) > 0)
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