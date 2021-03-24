using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UEditorNetCore;

namespace Pi.PiManager.Api.Controllers
{
    /// <summary>
    /// 百度富文本
    /// </summary>
    [Route("api/Admin/[controller]")] //配置路由
    public class UEditorController : Controller
    {
        private UEditorService ue;
        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }
        [HttpGet("Do")]
        [ApiExplorerSettings(IgnoreApi = true)] //隐藏接口
        public void Do()
        {
            ue.DoAction(HttpContext);
        }
    }
}
