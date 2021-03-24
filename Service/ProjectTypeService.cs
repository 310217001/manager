using Pi.PiManager.IService;
using Pi.PiManager.Model;
using Pi.PiManager.Service.Base;
using Pi.PiManager.Service.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class ProjectTypeService: MongoDBHelper<ProjectTypeInfo>, IProjectTypeService 
    { 
    }
}
