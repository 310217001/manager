using Pi.PiManager.IService.MongoDB;
using Pi.PiManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.IService
{
    public interface IProjectInfoService: IMongoDBHelper<ProjectInfo>
    {
    }
}
