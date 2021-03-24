using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Common;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Api.Helper;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels.API;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Controllers.Admin.Info
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin/[controller]")]
    [Authorize(Policy = "Admin")]
    [ApiGroup(ApiGroupNames.AdminInfo)]
    public class ArticleMenuController : ControllerBase
    {
        private readonly IArticleMenuService articleMenuDB;
        public ArticleMenuController(IArticleMenuService articleMenuDB)
        {
            this.articleMenuDB = articleMenuDB;
        }
        /// <summary>
        /// 栏目列表
        /// </summary>
        /// <param name="State">状态</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IEnumerable<ELTreeViewModel>>> List(int? State = null)
        {
            // 以接口的形式返回数据
            var res = new ApiResult<IEnumerable<ELTreeViewModel>>();
            List<ELTreeViewModel> list = new List<ELTreeViewModel>();
            var parm = Expressionable.Create<ArticleMenu>()
                .AndIF(State != null, m => m.State == State);
            var data = articleMenuDB.GetWhere(parm.ToExpression()).OrderBy(a => a.Sorting);
            ELTreeViewModel eLTreeViewModel = new ELTreeViewModel();
            if (data != null)
            {
                // 如果要看iview tree版,这里要改成Tree2ViewModel
                list.Add(new ELTreeViewModel
                {
                    label = "根目录",
                    children = APIHelper.GetChildren(data, 0)
                });
                res.data = list;
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
            else
            {
                res.msg = "参数丢失";
                res.statusCode = (int)ApiEnum.Status;
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 根据id查
        /// </summary>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<ApiResult<object>> Item(int ID)
        {
            var res = new ApiResult<object>();
            if (ID > 0)
            {
                ArticleMenu m = articleMenuDB.GetId(ID);
                if (m != null)
                {
                    res.success = true;
                    res.data = new
                    {
                        m.ParentID,
                        m.Names,
                        m.ENames,
                        m.Sorting
                    };
                }
                else
                {
                    res.data = new { ParentID = 0, Names = "", ENames = "", Sorting = "" };
                }
            }
            if (res.data!=null)
            {
                res.success = true;
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 栏目新增
        /// </summary>
        /// <param name="ParentID">父类id</param>
        /// <param name="Names">名称</param>
        /// <param name="ENames">存英文名或副标题</param>
        /// <param name="Sorting">顺序</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Item(int ParentID, string Names, string ENames, int Sorting)
        {
            var res = new ApiResult<string>();
            if (string.IsNullOrWhiteSpace(Names))
            {
                res.msg = "请填写名称";
            }
            else if (ParentID < 0)
            {
                res.msg = "请正确填写父类id";
            }
            else
            {
                ArticleMenu m = new ArticleMenu();
                ArticleMenu am = articleMenuDB.GetId(ParentID);
                if (ParentID == 0 || am != null)
                {
                    m.Names = Names;
                    m.ENames = ENames;
                    m.Sorting = Sorting;
                    m.ParentID = ParentID;
                    m.State = 2;
                    m.AddDate = DateTime.Now;
                    m.URL = "";
                    try
                    {
                        res.success = articleMenuDB.Add(m) > 0;
                        if (res.success)
                            res.msg = "添加成功";
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
                {
                    res.msg = "父类填写错误，无该父类";
                }
            }
            return await Task.Run(() => res);
        }
        /// <summary>
        /// 栏目更新
        /// </summary>
        /// <param name="ID">选择id</param>
        /// <param name="ParentID">父类id</param>
        /// <param name="Names">名称</param>
        /// <param name="ENames">存英文名或副标题</param>
        /// <param name="Sorting">顺序</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<string>> Item(int ID, int ParentID, string Names, string ENames, int Sorting)
        {
            var res = new ApiResult<string>();
            if (string.IsNullOrWhiteSpace(Names))
            {
                res.msg = "请填写名称";
            }
            else if (ID > 0 && ID == ParentID)
            {
                res.msg = "类型不能在自己之下！";
            }
            else if (ParentID < 0)
            {
                res.msg = "请正确填写父类id";
            }
            else
            {
                try
                {
                    ArticleMenu m = articleMenuDB.GetId(ID);
                    ArticleMenu am = articleMenuDB.GetId(ParentID);
                    if (ParentID == 0 || am != null)
                    {
                        if (m != null)
                        {
                            m.Names = Names;
                            m.ENames = ENames;
                            m.Sorting = Sorting;
                            res.success = articleMenuDB.Update(m) > 0;
                            if (res.success)
                                res.msg = "修改成功";
                            else
                            {
                                res.msg = "修改失败";
                                res.statusCode = (int)ApiEnum.Status;
                            }
                        }
                        else
                        {
                            res.msg = "栏目查询失败！";
                            res.statusCode = (int)ApiEnum.Error;
                        }
                    }
                    else
                    {
                        res.msg = "父类填写错误，无该父类";
                    }
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
        /// 更换父级
        /// </summary>
        /// <returns></returns>
        [HttpPut("ModifyParentID")]
        public async Task<ApiResult<string>> ModifyParentID(int ID, int ParentID)
        {
            var res = new ApiResult<string>();
            if (ID > 0)
            {
                res.success = articleMenuDB.Update(a => a.ID == ID, a => new ArticleMenu { ParentID = ParentID }) > 0;
                if (res.success)
                {
                    res.msg = "更换成功";
                }
                else
                {
                    res.msg = "更换失败";
                    res.statusCode = (int)ApiEnum.Status;
                }
            }
            else
                res.msg = "参数丢失";
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <returns></returns>
        [HttpPut("Enable")]
        public async Task<ApiResult<string>> Enable(string ID)
        {
            var res = new ApiResult<string>();
            if (!string.IsNullOrWhiteSpace(ID))
            {
                string[] array = ID.Trim(',').Split(',');
                int i = 0;
                int State = 1;
                foreach (string item in array)
                {
                    if (articleMenuDB.Update(a => a.ID == Convert.ToInt32(item),
                          a => new ArticleMenu { State = State }) > 0)
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
