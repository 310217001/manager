using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Api.Helper;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Info
{
    /// <summary>
    /// 广告
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminInfo)]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService db;
        private readonly IAdvertTypeService advertTypedb;
        private readonly IMapper IMapper;


        public AdvertController(IAdvertService db, IAdvertTypeService advertTypedb, IWebHostEnvironment _webHostEnvironment,
           IMapper IMapper)
        {
            this.db = db;
            this.advertTypedb = advertTypedb;
            this.IMapper = IMapper;

        }
        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<object>> GetadvertViewModel()
        {
            var res = new ApiResult<object>() { statusCode = (int)ApiEnum.Status };
            var advertypename = advertTypedb.GetAll();
            List<BaseViewModel> advertlist = new List<BaseViewModel>();
            foreach (var item in advertypename)
            {
                advertlist.Add(new BaseViewModel(item.ID, item.Names));
            }
            res.data = advertlist;
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="Title"></param>
        /// <param name="IsEnable"></param>
        /// <param name="TypeID"></param>
        /// <param name="SelStartDate"></param>
        /// <param name="SelEndDate"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<ApiResult<IEnumerable<AdverViewModel>>> List(int pageIndex, string Title, bool? IsEnable, int TypeID, string? SelStartDate, string? SelEndDate)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<AdverViewModel>>() { statusCode = (int)ApiEnum.Status };
            if (pageIndex == 0)
                pageIndex = 1;
            //DBPage dp = new DBPage(pageIndex, 3);
            //var departments = await db.QueryList(dp, Title);
            DateTime? SelStartDate1 = null;
            DateTime? SelEndDate1 = null;
            if (!string.IsNullOrWhiteSpace(SelStartDate))
            {
                SelStartDate1 = C.DateTimes(SelStartDate);
            }
            if (!string.IsNullOrWhiteSpace(SelStartDate))
            {
                SelEndDate1 = C.DateTimes(SelEndDate);
            }
            var parm = Expressionable.Create<Advert>()
                    .AndIF(!string.IsNullOrEmpty(Title), m => m.Title.Contains(Title))
                    .AndIF(TypeID != 0, m => m.TypeID == TypeID)
                    .AndIF(IsEnable != null, m => m.IsEnable == IsEnable)
                    .AndIF(SelStartDate1 != null, m => m.StartDate >= SelStartDate1)
                    .AndIF(SelEndDate1 != null, m => m.EndDate <= SelEndDate1);
            //var list = db.GetWhere(parm.ToExpression());
            //var list = db.GetPages(parm.ToExpression(), new PageParm(pageIndex), a => a.ID, "Desc");
            //var list = db.GetPages(parm.ToExpression(), new PageParm(pageIndex), a => new { a.ID, a.Sorting }, "Desc");
            var list = db.GetPages(parm.ToExpression(), new PageParm(pageIndex), "Sorting,ID DESC");
            res.success = true;
            List<AdverViewModel> list2 = new List<AdverViewModel>();
            if (list.DataSource != null)
            {
                foreach (var item in list.DataSource)
                {
                    list2.Add(IMapper.Map<AdverViewModel>(item));
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
        /// 详情显示
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<AdverViewModel>> Item(int ID)
        {
            var res = new ApiResult<AdverViewModel>();
            AdverViewModel m = new AdverViewModel();
            if (ID > 0)
            {
                Advert advert = db.GetId(ID);
                if (advert != null)
                {
                    m.ID = advert.ID;
                    m.Title = advert.Title;
                    m.StartDate = Utility.GetDateFormat(advert.StartDate);
                    m.EndDate = Utility.GetDateFormat(advert.EndDate);
                    m.TypeID = advert.TypeID;
                    m.BackgroundColor = advert.BackgroundColor;
                    m.Pic = Utility.GetImgUrl(advert.Pic);
                    m.Url = advert.Url;
                    m.Sorting = advert.Sorting;
                    m.IsEnable = advert.IsEnable;
                    res.success = true;
                }
            }
            else
            {
                m.StartDate = "";
                m.EndDate = "";
                m.IsEnable = true;
                m.Title = "";
            }
            res.data = m;

            return await Task.Run(() => res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="avm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Add([FromBody] AdverViewModel avm)
        {
            var res = new ApiResult();
            if (!string.IsNullOrWhiteSpace(avm.Title) && avm.TypeID > 0)
            {
                try
                {
                    Advert m = new Advert();
                    m.Title = avm.Title;
                    m.TypeID = avm.TypeID;
                    m.Pic = avm.Pic;
                    m.BackgroundColor = avm.BackgroundColor;
                    m.StartDate = C.DateTimes(avm.StartDate);
                    m.EndDate = C.DateTimes(avm.EndDate);
                    m.Url = avm.Url;
                    m.Sorting = avm.Sorting;
                    m.IsEnable = avm.IsEnable;
                    if (avm.Pic != null)
                    {
                        //如有图片上传则保存到本地
                        if (avm.Pic.Contains("base64"))
                        {
                            string path = "UploadFiles/advert/";
                            string path2 = Utility.HostAddress + "advert\\";
                            m.Pic = ImagesUtility.Base64StringToFile(avm.Pic, path2, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                            if (m.Pic != "")
                                m.Pic = path + m.Pic;
                        }
                    }
                    res.success = db.Add(m) > 0;
                }
                catch (Exception ex)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="avm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Update([FromBody] AdverViewModel avm)
        {
            var res = new ApiResult();
            if (!string.IsNullOrWhiteSpace(avm.Title) && avm.TypeID > 0)
            {
                try
                {
                    Advert m = null;
                    m = db.GetId(avm.ID);
                    if (m == null)
                        m = new Advert();
                    m.Title = avm.Title;
                    m.TypeID = avm.TypeID;
                    m.Pic = avm.Pic;
                    m.BackgroundColor = avm.BackgroundColor;
                    m.StartDate = C.DateTimes(avm.StartDate);
                    m.EndDate = C.DateTimes(avm.EndDate);
                    m.IsEnable = avm.IsEnable;
                    m.Url = avm.Url;
                    m.Sorting = avm.Sorting;
                    if (res.success)
                        res.statusCode = (int)ApiEnum.Status;
                    // 如有图片上传则保存到本地
                    if (avm.Pic.Contains("base64"))
                    {
                        string path = "UploadFiles/advert/";
                        string path2 = Utility.HostAddress + "advert\\";
                        m.Pic = ImagesUtility.Base64StringToFile(avm.Pic, path2, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        if (m.Pic != "")
                            m.Pic = path + m.Pic;
                    }
                    res.success = db.Update(m) > 0;
                }
                catch (Exception ex)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                }
            }
            return await Task.Run(() => res);
        }
        [HttpDelete("{ID}")]
        public ApiResult Delete(int ID)
        {
            if (ID > 0)
                return new ApiResult { statusCode = (int)ApiEnum.Status, success = db.Delete(ID) > 0 };
            return new ApiResult { statusCode = (int)ApiEnum.ParameterError, msg = "参数丢失" };
        }
    }
}