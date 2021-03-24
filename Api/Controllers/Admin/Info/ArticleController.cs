using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Api.Helper;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Info
{
    /// <summary>
    /// 文章管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminInfo)]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService db;
        private readonly IArticleMenuService articleMenuDB;
        private readonly IMenuService MenuDB;
        private readonly IArticleMenu_ArticleService articleMenu_ArticleService;
        private readonly IMapper IMapper;
        public ArticleController(IArticleService articleService, IArticleMenuService articleMenuDB, IArticleMenu_ArticleService articleMenu_ArticleService, IMapper IMapper, IMenuService MenuDB)
        {
            this.db = articleService;
            this.articleMenuDB = articleMenuDB;
            this.articleMenu_ArticleService = articleMenu_ArticleService;
            this.IMapper = IMapper;
            this.MenuDB = MenuDB;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        [Consumes("multipart/form-data")]
        [HttpPost("Postfile")]
        public IActionResult Postfile(IFormFile file)
        {
            IList<CustomStatusCode> code = new List<CustomStatusCode>();
            ImageFile image = new ImageFile();
            string path = Utility.HostAddress + "article\\";
            var currentCode = image.UpLoadFile(file, path);
            code.Add(currentCode);
            return StatusCode(200, code);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        [Consumes("multipart/form-data")]
        [HttpPost("PostImage")]
        public IActionResult PostImage(IFormFile file)
        {
            IList<CustomStatusCode> code = new List<CustomStatusCode>();
            ImageFile image = new ImageFile();
            string path = Utility.HostAddress + "article\\";
            var currentCode = image.UpLoadPhoto(file, path);
            code.Add(currentCode);
            return StatusCode(200, code);
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<object>> Item(int ID)
        {
            var res = new ApiResult<object>();
            if (ID > 0)
            {
                Article m = db.GetId(ID);
                if (m != null)
                {
                    List<ArticleMenu_Article> list2 = articleMenu_ArticleService.GetWhere(a => a.ArticleID == ID);
                    List<ArticleMenuViewModel> Articlemenumodel = new List<ArticleMenuViewModel>();
                    if (list2 != null)
                    {
                        foreach (var item in list2)
                        {
                            ArticleMenu articmenus = articleMenuDB.GetId(item.ArticleMenuID);
                            if (articmenus != null)
                            {
                                Articlemenumodel.Add(new ArticleMenuViewModel
                                {
                                    id = item.ArticleMenuID,
                                    Names = articmenus.Names
                                });
                            }
                            else
                            {
                                res.msg = "参数丢失";
                            }
                        }

                    }
                    //List<ArticleMenu_Article> list2 = articleMenu_ArticleService.GetWhere(a => a.ArticleID == ID);
                    //if (list2 != null)
                    //{
                    //    foreach (var item in list2)
                    //    {
                    //        ArticleMenu articmenus = articleMenuDB.GetId(item.ArticleMenuID);
                    //        if (articmenus != null)
                    //        {
                    //            ArticleMenuID = ArticleMenuID + item.ArticleMenuID + ",";
                    //            ArticleMenuName = ArticleMenuName + articmenus.Names + ",";
                    //        }
                    //        else
                    //        {
                    //            res.msg = "参数丢失";
                    //        }
                    //    }
                    if (m.Contents == null)
                    {
                        m.Contents = "";
                    }
                    res.data = new
                    {
                        m.ID,
                        m.Title,
                        AddDate = Utility.GetDateFormat(m.AddDate, 1),
                        Contents = m.Contents.Replace("src=\"/UploadFiles", "src=\"" + Utility.WEB_IMAGES_URL + "/UploadFiles"),    // 图片路径替换
                        m.ImgUrl,
                        m.Type,
                        m.FileUrl,
                        m.Keyword,
                        m.Source,
                        m.IsTop,
                        m.State,
                        m.Synopsis,
                        m.Sorting,
                        m.PageView,
                        m.Author,
                        FileSize = m.FileSize + "",
                        FileFormat = m.FileFormat + "",
                        Articlemenumodel
                    };
                    //}
                }
                else
                {
                    res.msg = "参数丢失";
                }
            }
            else
            {
                res.data = new
                {
                    Title = "",
                    AddDate = DateTime.Now,
                    Contents = "",
                    Source = "",
                    ImgUrl = "",
                    Type = 1,
                    FileUrl = "",
                    Keyword = "",
                    Sorting = 1,
                    IsTop = false,
                    State = 2,
                    Synopsis = "",
                    PageView = 0,
                    Author = "",
                    FileSize = "",
                    FileFormat = "",
                    Articlemenumodel = new List<ArticleMenuModel>()
                };
                res.msg = "无数据";
            }
            if (res.data != null)
            {
                res.success = true;
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 文章展示
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="ArticleMenuID">栏目id</param>
        /// <param name="State">状态</param>
        /// <param name="Title">标题</param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<ApiResult<IEnumerable<ArticleViewModel>>> List(int pageIndex, int? ArticleMenuID, int? State, string Title)
        {
            var res = new ApiResult<IEnumerable<ArticleViewModel>>();
            if (pageIndex == 0)
                pageIndex = 1;

            var parm = Expressionable.Create<Article>()
                .AndIF(ArticleMenuID != null && ArticleMenuID != 0, a => SqlFunc.Subqueryable<ArticleMenu_Article>().Where(s => s.ArticleID == a.ID && s.ArticleMenuID == ArticleMenuID).Any())
                .AndIF(State != null && State != 0, a => a.State == State)
                .AndIF(!string.IsNullOrWhiteSpace(Title), a => a.Title.Contains(Title));
            var list = db.GetPages(parm.ToExpression(), new Common.PageParm(pageIndex), "Sorting,ID DESC");
            res.success = true;
            List<ArticleViewModel> list2 = new List<ArticleViewModel>();
            ArticleViewModel vm;
            if (list.DataSource != null)
            {
                foreach (var item in list.DataSource)
                {
                    vm = IMapper.Map<ArticleViewModel>(item);

                    // 查询栏目
                    var menuList = articleMenuDB.GetWhere(a => SqlFunc.Subqueryable<ArticleMenu_Article>().Where(b => b.ArticleMenuID == a.ID && b.ArticleID == item.ID).Any());
                    if (menuList != null)
                    {
                        List<ArticleMenuModel> vm2 = new List<ArticleMenuModel>();
                        foreach (var item2 in menuList)
                        {
                            vm2.Add(new ArticleMenuModel
                            {
                                Names = item2.Names,
                                ArticleMenuID = item2.ID
                            });
                        }
                        vm.ArticleMenu = vm2;
                    }
                    list2.Add(vm);
                }
                res.data = list2;
                if (res.data != null && res.data.Count() > 0)
                {
                    res.success = true;
                    res.index = pageIndex;
                    res.count = list.TotalCount;
                    res.size = list.PageSize;
                    res.pages = list.TotalPages;
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

            return await Task.Run(() => res);
        }

        /// <summary>
        /// 栏目数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenuLevel")]
        public async Task<ApiResult<IEnumerable<ELTreeViewModel>>> GetMenuLevel()
        {
            var res = new ApiResult<IEnumerable<ELTreeViewModel>>();
            List<ELTreeViewModel> MenuView = new List<ELTreeViewModel>();
            var data = articleMenuDB.GetWhere(o => o.State == 2);
            ELTreeViewModel eLTreeViewModel = new ELTreeViewModel();
            if (data != null)
            {
                // 如果要看iview tree版,这里要改成Tree2ViewModel
                MenuView.Add(new ELTreeViewModel
                {
                    id = 0,
                    label = "栏目",
                    children = APIHelper.GetChildren(data, 0)
                });
                res.data = MenuView;
                if (res.data != null && res.data.Count() > 0)
                {
                    res.success = true;
                }
                else
                {
                    res.msg = "无数据";
                    res.statusCode = (int)ApiEnum.Status;
                }
            }
            //List<ArticleMenu> menus = articleMenuDB.GetWhere(o => o.ParentID == 0&&o.State==2);//第一级
            //foreach (var item in menus)
            //{
            //    List<DownMenuViewModel> downMenuView2 = new List<DownMenuViewModel>();
            //    List<ArticleMenu> menus2 = articleMenuDB.GetWhere(o => o.ParentID == item.ID);//第二级
            //    foreach (var item2 in menus2)
            //    {
            //        downMenuView2.Add(new DownMenuViewModel
            //        {
            //            ID = item2.ID,
            //            Title = "├"+item2.Names,
            //        });
            //    }
            //    downMenuView.Add(new DownMenuViewModel
            //    {
            //        ID = item.ID,
            //        Title = "└ "+item.Names,
            //        downMenu = downMenuView2
            //    });
            //}
            //res.data = downMenuView;
            //if (res.data != null && res.data.Count() > 0)
            //{
            //    res.success = true;
            //}
            //else
            //{
            //    res.msg = "无数据";
            //    res.statusCode = (int)ApiEnum.Status;
            //}
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public ApiResult<string> Update([FromBody] ArticlePostViewModel vm)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.ParameterError };
            if (!string.IsNullOrWhiteSpace(vm.Title) && !string.IsNullOrWhiteSpace(vm.ArticleMenuIDs))
            {
                bool IsAdd = false;
                if (vm.ID > 0)
                {
                    Article m = db.GetId(vm.ID);
                    if (m != null)
                    {
                        m.ID = vm.ID;
                        m.Title = vm.Title;
                        m.AddDate = C.DateTimes(vm.AddDate);
                        m.ImgUrl = vm.ImgUrl;
                        m.Type = vm.Type;
                        m.FileUrl = vm.FileUrl;
                        m.Keyword = vm.Keyword;
                        m.Sorting = vm.Sorting;
                        m.IsTop = vm.IsTop;
                        m.State = vm.State;
                        m.Synopsis = vm.Synopsis;
                        m.Contents = vm.Contents;
                        m.Source = vm.Source;
                        m.PageView = vm.PageView;
                        m.Author = vm.Author;
                        m.FileSize = m.FileSize + "";
                        m.FileFormat = m.FileFormat + "";
                        try
                        {
                            if (vm.ImgUrl != null && !string.IsNullOrWhiteSpace(vm.ImgUrl))
                            {
                                //如有图片上传则保存到本地
                                if (vm.ImgUrl.Contains("base64"))
                                {
                                    string path = "UploadFiles/article/";
                                    string path2 = Utility.HostAddress + "article\\";
                                    m.ImgUrl = ImagesUtility.Base64StringToFile(vm.ImgUrl, path2, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                                    if (m.ImgUrl != "")
                                        m.ImgUrl = path + m.ImgUrl;
                                }
                            }
                            //if (vm.FileUrl != null && !string.IsNullOrWhiteSpace(vm.FileUrl))
                            //{
                            //    //如有图片上传则保存到本地
                            //    if (vm.FileUrl.Contains("base64"))
                            //    {
                            //        string path = "UploadFiles/article/";
                            //        string path2 = Utility.HostAddress + "article\\";
                            //        m.FileUrl = ImagesUtility.Base64StringToFile(vm.FileUrl, path2, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                            //        if (m.FileUrl != "")
                            //            m.FileUrl = path + m.FileUrl;
                            //    }
                            //}
                            res.success = db.Update(m) > 0;
                            // 处理文章栏目
                            if (res.success)
                            {
                                res.statusCode = (int)ApiEnum.Status;
                                // 先删除
                                if (!IsAdd)
                                {
                                    articleMenu_ArticleService.Delete(a => a.ArticleID == m.ID);
                                }
                                // 再添加
                                string[] array = vm.ArticleMenuIDs.Split(',');
                                if (array.Length > 0)
                                {
                                    List<ArticleMenu_Article> list = new List<ArticleMenu_Article>();
                                    foreach (string item in array)
                                    {
                                        list.Add(new ArticleMenu_Article
                                        {
                                            ArticleID = m.ID,
                                            ArticleMenuID = C.Int(item)
                                        });
                                    }
                                    articleMenu_ArticleService.Add(list);
                                    res.msg = "修改成功";
                                }
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
                    {
                        res.msg = "参数丢失";
                        res.statusCode = (int)ApiEnum.Status;
                    }
                }
                else
                    res.msg = "参数丢失";
            }
            else
            {
                res.msg = "请正确填写选择的id";
            }
            return res;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public ApiResult<string> Add([FromBody] ArticlePostViewModel vm)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.ParameterError };
            if (!string.IsNullOrWhiteSpace(vm.Title) && !string.IsNullOrWhiteSpace(vm.ArticleMenuIDs))
            {
                Article m = new Article();
                m.Title = vm.Title;
                m.AddDate = C.DateTimes(vm.AddDate);
                m.ImgUrl = vm.ImgUrl;
                m.Type = vm.Type;
                m.FileUrl = vm.FileUrl;
                m.Keyword = vm.Keyword;
                m.Sorting = vm.Sorting;
                m.IsTop = vm.IsTop;
                m.State = vm.State;
                m.Synopsis = vm.Synopsis;
                m.Contents = vm.Contents;
                m.Source = vm.Source;
                m.PageView = vm.PageView;
                m.Author = vm.Author;
                m.AddDate = C.DateTimes(m.AddDate);
                m.FileSize = m.FileSize + "";
                m.FileFormat = m.FileFormat + "";
                try
                {
                    if (vm.ImgUrl != null && !string.IsNullOrWhiteSpace(vm.ImgUrl))
                    {
                        //如有图片上传则保存到本地
                        if (vm.ImgUrl.Contains("base64"))
                        {
                            string path = "UploadFiles/article/";
                            string path2 = Utility.HostAddress + "article\\";
                            m.ImgUrl = ImagesUtility.Base64StringToFile(vm.ImgUrl, path2, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                            if (m.ImgUrl != "")
                                m.ImgUrl = path + m.ImgUrl;
                        }
                    }
                    //if (vm.FileUrl != null && !string.IsNullOrWhiteSpace(vm.FileUrl))
                    //{
                    //    //如有图片上传则保存到本地
                    //    if (vm.FileUrl.Contains("base64"))
                    //    {
                    //        string path = "UploadFiles/article/";
                    //        string path2 = Utility.HostAddress + "article\\";
                    //        m.FileUrl = ImagesUtility.Base64StringToFile(vm.FileUrl, path2, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    //        if (m.FileUrl != "")
                    //            m.FileUrl = path + m.FileUrl;
                    //    }
                    //}
                    m.ID = db.Insert(m);
                    res.success = m.ID > 0;
                    // 处理文章栏目
                    if (res.success)
                    {
                        string[] array = vm.ArticleMenuIDs.Split(',');
                        if (array.Length > 0)
                        {
                            List<ArticleMenu_Article> list = new List<ArticleMenu_Article>();
                            foreach (string item in array)
                            {
                                list.Add(new ArticleMenu_Article
                                {
                                    ArticleID = m.ID,
                                    ArticleMenuID = C.Int(item)
                                });
                            }
                            if (articleMenu_ArticleService.Add(list) > 0)
                            {
                                res.msg = "添加成功";
                            }
                            else
                            {
                                res.msg = "栏目添加失败";
                                res.success = false;
                                res.statusCode = (int)ApiEnum.Status;
                            }
                        }

                    }
                    else
                    {
                        res.msg = "文章添加失败";
                        res.success = false;
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
            return res;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("SetState")]
        public async Task<ApiResult<string>> SetState(string ID)
        {
            int State = 3;
            var res = new ApiResult<string>();
            if (!string.IsNullOrWhiteSpace(ID))
            {
                string[] array = ID.Trim(',').Split(',');
                int i = 0;
                foreach (string item in array)
                {
                    if (db.Update(a => a.ID == Convert.ToInt32(item),
                          a => new Article { State = State }) > 0)
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
