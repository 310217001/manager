using MongoDB.Driver;
using Pi.PiManager.IService;
using Pi.PiManager.Model;
using Pi.PiManager.Service.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Service
{
    public class ProjectInfoService: MongoDBHelper<ProjectInfo>, IProjectInfoService
    {
        
    }
}
