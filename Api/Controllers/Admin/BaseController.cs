using Microsoft.AspNetCore.Mvc;
using Pi.PiManager.Api.Extensions;
using static Pi.PiManager.Api.Extensions.JwtHelper;

namespace Pi.PiManager.Controllers
{
    /// <summary>
    /// 后台接口父类
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 获取UserID
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "GetUserID")]
        [ApiExplorerSettings(IgnoreApi = true)] //隐藏接口
        public int GetUserID()
        {
            string authHeader = this.Request.Headers["Authorization"];//Header中的token
            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                authHeader = authHeader.Replace("Bearer ", "");
                return JwtHelper.SerializeJwt2(authHeader);
            }
            return 0;
        }
        /// <summary>
        /// 解码Token
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "TokenDecode")]
        [ApiExplorerSettings(IgnoreApi = true)] //隐藏接口
        public TokenModelJwt TokenDecode()
        {
            string authHeader = this.Request.Headers["Authorization"];//Header中的token
            if (!string.IsNullOrWhiteSpace(authHeader))
                authHeader = authHeader.Replace("Bearer ", "");
            return JwtHelper.SerializeJwt(authHeader);
        }
    }
}
