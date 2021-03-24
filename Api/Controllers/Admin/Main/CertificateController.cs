using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.IService;
using Pi.PiManager.Model;
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
    /// 证书
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [ApiGroup(ApiGroupNames.Main)]
    [Authorize(Policy = "Admin")]
    public class CertificateController : Controller
    {
        private readonly ICertificateService Certificatedb;
        private readonly IProjectInfoService Projectdb;
        private readonly IProviderService providerdb;
        private readonly IMapper IMapper;
        public CertificateController(ICertificateService Certificatedb, IProjectInfoService Projectdb, IProviderService providerdb, IMapper IMapper)
        {
            this.Certificatedb = Certificatedb;
            this.Projectdb = Projectdb;
            this.providerdb = providerdb;
            this.IMapper = IMapper;
        }
        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="Title">证书名</param>
        /// <param name="ProviderID">服务商</param>
        /// <param name="ProjectID">项目</param>
        /// <param name="IsEnable"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<CertificateViewModel>>> List(int pageIndex, string Title, int? ProviderID, string ProjectID, bool? IsEnable)
        {
            var res = new ApiResult<IEnumerable<CertificateViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            var parm = Expressionable.Create<Certificate>()
            .AndIF(ProviderID != null && ProviderID != 0, a => a.ProviderID == ProviderID)
            .AndIF(!string.IsNullOrEmpty(Title), m => m.Title.Contains(Title))
            .AndIF(!string.IsNullOrEmpty(ProjectID), m => m.ProjectID==ProjectID)
            .AndIF(IsEnable != null, m => m.IsEnable == IsEnable);
            var list = Certificatedb.GetPages(parm.ToExpression(), new PageParm(pageIndex));
            List<CertificateViewModel> list2 = new List<CertificateViewModel>();
            if (list.DataSource != null&&list.DataSource.Count>0)
            {
                foreach (var item in list.DataSource)
                {
                    CertificateViewModel n = IMapper.Map<CertificateViewModel>(item);
                    ProjectInfo projectInfo = Projectdb.Get("Projects", item.ProjectID);
                    if (projectInfo!=null)
                    {
                        n.ProjectName = projectInfo.Names;
                    }
                    Provider provider = providerdb.GetId(item.ProviderID);
                    if (provider!=null)
                    {
                        n.ProviderName = provider.Title;
                    }
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
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<CertificateViewModel>> Item(int ID)
        {
            var res = new ApiResult<CertificateViewModel>();
            Certificate m;
            CertificateViewModel n = new CertificateViewModel();
            if (ID > 0)
            {
                m = Certificatedb.GetId(ID);
                if (m != null)
                {
                    n = IMapper.Map<CertificateViewModel>(m);
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
        /// <param name="certificate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add(APICertificateViewModel certificate)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(certificate.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {
                    Certificate m = new Certificate()
                    {
                        Title = certificate.Title,
                        ProjectID = certificate.ProjectID,
                        ProviderID = certificate.ProviderID,
                        Note = certificate.Note,
                        RegisterDate = C.DateTimes(certificate.RegisterDate),
                        ExpireDate = C.DateTimes(certificate.ExpireDate),
                        AddDate = DateTime.Now,
                        IsEnable = true
                    };
                    res.success = Certificatedb.Add(m) > 0;
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
        /// <param name="certificate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update(int id, APICertificateViewModel certificate)
        {
            // 以接口的形式返回数据
            var res = new ApiResult();
            if (string.IsNullOrWhiteSpace(certificate.Title))
            {
                res.msg = "标题不能为空";
            }
            else
            {
                try
                {

                    Certificate m = Certificatedb.GetId(id);
                    m.Title = certificate.Title;
                    m.ProjectID = certificate.ProjectID;
                    m.ProviderID = certificate.ProviderID;
                    m.Note = certificate.Note;
                    m.RegisterDate = C.DateTimes(certificate.RegisterDate);
                    m.ExpireDate = C.DateTimes(certificate.ExpireDate);
                    m.IsEnable = certificate.IsEnable;
                    res.success = Certificatedb.Update(m) > 0;
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
                    if (Certificatedb.Update(a => a.ID == Convert.ToInt32(item),
                          a => new Certificate { IsEnable = false }) > 0)
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

