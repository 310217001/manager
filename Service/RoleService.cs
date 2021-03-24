using Pi.PiManager.IServices;
using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Service.Base;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Service
{
    public class RoleService :  BaseService<Role>,IRoleService
    {
        //// 准备数据
        //private readonly List<Role> roleList = new List<Role>();

        ///// <summary>
        ///// 列表
        ///// </summary>
        ///// <param name="p">分页</param>
        ///// <param name="Names">名称</param>
        ///// <returns></returns>
        //public Task<IEnumerable<Role>> QueryList(DBPage p = null, string Names = null)
        //{
        //    var data = Db.Queryable<Role>().Where(GetPara(Names).ToExpression());
        //    if (p != null)
        //    {
        //        int count = 0;
        //        List<Role> list = data.ToPageList(p.PageIndex, p.PageSize, ref count);
        //        p.Count = count;
        //        return Task.Run(function: () => list.AsEnumerable());
        //    }
        //    return Task.Run(function: () => data.ToList().AsEnumerable());
        //}
        ///// <summary>
        ///// 获取条件
        ///// </summary>
        //private Expressionable<Role> GetPara(string Names = null)
        //{
        //    Expressionable<Role> exp = Expressionable.Create<Role>();
        //    if (Names != null)
        //        exp.And(a => a.Names.Contains(Names));
        //    return exp;
        //}
    }
}
