using Pi.PiManager.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Pi.PiManager.IServices;
using Pi.PiManager.Service.Base;
using Pi.PiManager.IService;

namespace Pi.PiManager.Service
{
    public class AdvertTypeService : BaseService<AdvertType>, IAdvertTypeService
    {
        ///// <summary>
        ///// 查询列表
        ///// </summary>
        ///// <param name="p">分页</param>
        ///// <param name="Names">名称</param>
        ///// <returns></returns>
        //public Task<IEnumerable<AdvertType>> QueryList(DBPage p, string Names = null)
        //{
        //    var data = Db.Queryable<AdvertType>().Where(GetPara(Names).ToExpression()).OrderBy(a => a.Sorting).OrderBy(a => a.ID, OrderByType.Desc); ;
        //    if (p != null)
        //    {
        //        int count = 0;
        //        List<AdvertType> list = data.ToPageList(p.PageIndex, p.PageSize, ref count);
        //        p.Count = count;
        //        return Task.Run(function: () => list.AsEnumerable());
        //    }
        //    return Task.Run(function: () => data.ToList().AsEnumerable());
        //}
        ///// <summary>
        ///// 获取条件
        ///// </summary>
        //private Expressionable<AdvertType> GetPara(string Names = null)
        //{
        //    Expressionable<AdvertType> exp = Expressionable.Create<AdvertType>();
        //    if (Names != null)
        //        exp.And(a => a.Names.Contains(Names));
        //    return exp;
        //}

    }
}
