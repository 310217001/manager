using Pi.PiManager.IService.Base;
using Pi.PiManager.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.IService
{
    public interface IUserService : IBaseService<UserInfo>
    {
        /// <summary>
        /// 获取用户显示名称
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public string GetNames(UserInfo u);
    }
}
