using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Service.Base;

namespace Pi.PiManager.Service
{
    /// <summary>
    /// 网站基本配置
    /// </summary>
    public class ConfigBaseService : BaseService<ConfigBase>, IConfigBaseService //DbContext<ConfigBase>
    {
    }
}
