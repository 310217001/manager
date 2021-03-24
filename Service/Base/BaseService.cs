﻿using Pi.Common;
using Pi.PiManager.IService.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pi.PiManager.Service.Base
{
    /// <summary>
    /// 基础服务定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : DbContext, IBaseService<T> where T : class, new()
    {
        #region 事务

        /// <summary>
        /// 启用事务
        /// </summary>
        public void BeginTran()
        {
            Db.Ado.BeginTran();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran()
        {
            Db.Ado.CommitTran();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTran()
        {
            Db.Ado.RollbackTran();
        }

        #endregion

        #region 添加操作
        /// <summary>
        /// 添加一条数据 返回新ID
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        public int Insert(T parm)
        {
            return Db.Insertable(parm).RemoveDataCache().ExecuteReturnIdentity();
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        public int Add(T parm)
        {
            return Db.Insertable(parm).RemoveDataCache().ExecuteCommand();
            //return Db.Insertable(parm).ExecuteCommand();
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="parm">List<T></param>
        /// <returns></returns>
        public int Add(List<T> parm)
        {
            return Db.Insertable(parm).RemoveDataCache().ExecuteCommand();
            //return Db.Insertable(parm).ExecuteCommand();
        }

        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="parm">List<T></param>
        /// <returns></returns>
        public T Saveable(T parm, Expression<Func<T, object>> uClumns = null, Expression<Func<T, object>> iColumns = null)
        {
            var command = Db.Saveable(parm);

            if (uClumns != null)
            {
                command = command.UpdateIgnoreColumns(uClumns);
            }

            if (iColumns != null)
            {
                command = command.InsertIgnoreColumns(iColumns);
            }

            return command.ExecuteReturnEntity();
        }

        /// <summary>
        /// 批量添加或更新数据
        /// </summary>
        /// <param name="parm">List<T></param>
        /// <returns></returns>
        public List<T> Saveable(List<T> parm, Expression<Func<T, object>> uClumns = null, Expression<Func<T, object>> iColumns = null)
        {
            var command = Db.Saveable(parm);

            if (uClumns != null)
            {
                command = command.UpdateIgnoreColumns(uClumns);
            }

            if (iColumns != null)
            {
                command = command.InsertIgnoreColumns(iColumns);
            }

            return command.ExecuteReturnList();
        }
        #endregion

        #region 查询操作

        /// <summary>
        /// 根据条件查询数据是否存在
        /// </summary>
        /// <param name="where">条件表达式树</param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> where)
        {
            return Db.Queryable<T>().Any(where);
        }

        /// <summary>
        /// 根据条件合计字段
        /// </summary>
        /// <param name="where">条件表达式树</param>
        /// <returns></returns>
        public TResult Sum<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> field)
        {
            return Db.Queryable<T>().Where(where).Sum(field);
        }
        /// <summary>
        /// 根据条件统计字段
        /// </summary>
        /// <param name="where">条件表达式树</param>
        /// <returns></returns>
        public int Count<TResult>(Expression<Func<T, bool>> where)
        {
            return Db.Queryable<T>().Count(where);
        }

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public T GetId(object pkValue)
        {
            return Db.Queryable<T>().WithCache().InSingle(pkValue);
        }

        /// <summary>
        /// 根据主键查询多条数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetIn(object[] ids)
        {
            return Db.Queryable<T>().In(ids).ToList();
        }

        /// <summary>
        /// 根据条件取条数
        /// </summary>
        /// <param name="where">条件表达式树</param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> where)
        {
            return Db.Queryable<T>().Count(where);

        }

        /// <summary>
        /// 查询所有数据(无分页,请慎用)
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll(bool useCache = false, int cacheSecond = 3600)
        {
            return Db.Queryable<T>().WithCacheIF(useCache, cacheSecond).ToList();
        }

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        public T GetFirst(Expression<Func<T, bool>> where)
        {
            return Db.Queryable<T>().Where(where).First();
        }

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="parm">string</param>
        /// <returns></returns>
        public T GetFirst(string parm)
        {
            return Db.Queryable<T>().Where(parm).First();
        }

        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="parm"></param>
        /// <param name="orderFileds">排序</param>
        /// <returns></returns>
        public PagedInfo<T> GetPages(Expression<Func<T, bool>> where, PageParm parm, string orderFileds = null)
        {
            int TotalCount = 0;
            PagedInfo<T> page = new PagedInfo<T>();
            var source = Db.Queryable<T>().Where(where).OrderByIF(orderFileds != null, orderFileds);
            //if (orderFileds != null)
            //    source = source.OrderBy(orderFileds);
            var list = source.ToPageList(parm.PageIndex, parm.PageSize, ref TotalCount);
            if (list != null)
            {
                page.PageIndex = parm.PageIndex;
                page.PageSize = parm.PageSize;
                page.TotalCount = TotalCount;
                page.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(page.TotalCount) / page.PageSize));
                page.DataSource = list;
                return page;
            }
            return null;
        }

        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="parm">分页参数</param>
        /// <param name="order">排序字段 a => new { a.ID, a.Sorting }</param>
        /// <param name="orderEnum">排序方式 Asc Desc</param>
        /// <returns></returns>
        public PagedInfo<T> GetPages(Expression<Func<T, bool>> where, PageParm parm, Expression<Func<T, object>> order, string orderEnum)
        {
            //var source = Db.Queryable<T>().Where(where);
            //return source.ToPage(parm);

            int TotalCount = 0;
            PagedInfo<T> page = new PagedInfo<T>();
            var source = Db.Queryable<T>().Where(where).OrderByIF(orderEnum == "Asc", order, OrderByType.Asc).OrderByIF(orderEnum == "Desc", order, OrderByType.Desc);
            var list = source.ToPageList(parm.PageIndex, parm.PageSize, ref TotalCount);
            if (list != null)
            {
                page.PageIndex = parm.PageIndex;
                page.PageSize = parm.PageSize;
                page.TotalCount = TotalCount;
                page.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(page.TotalCount) / page.PageSize));
                page.DataSource = list;
                return page;
            }
            return null;
        }

        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="parm"></param>
        /// <param name="orderFileds">排序</param>
        /// <returns></returns>
        public List<T> GetNopage(Expression<Func<T, bool>> where, string orderFileds = null)
        {
            List<T> source = null;
            if (where!=null)
            {
                source = Db.Queryable<T>().Where(where).OrderByIF(orderFileds != null, orderFileds).ToList();
            }
            else
            {
                source = Db.Queryable<T>().OrderByIF(orderFileds != null, orderFileds).ToList();
            }
            return source;
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="where">条件表达式树</param>
        /// <returns></returns>
        public List<T> GetWhere(Expression<Func<T, bool>> where, bool useCache = false, int cacheSecond = 3600)
        {
            var query = Db.Queryable<T>().Where(where).WithCacheIF(useCache, cacheSecond);
            return query.ToList();
        }

        /// <summary>
		/// 根据条件查询数据
		/// </summary>
		/// <param name="where">条件表达式树</param>
		/// <returns></returns>
        public List<T> GetWhere(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, string orderEnum = "Asc", bool useCache = false, int cacheSecond = 3600)
        {
            var query = Db.Queryable<T>().Where(where).OrderByIF(orderEnum == "Asc", order, OrderByType.Asc).OrderByIF(orderEnum == "Desc", order, OrderByType.Desc).WithCacheIF(useCache, cacheSecond);
            return query.ToList();
        }

        #endregion

        #region 修改操作
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        public int Update(T parm)
        {
            return Db.Updateable(parm).RemoveDataCache().ExecuteCommand();
            //return Db.Updateable(parm).ExecuteCommand();
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        public int Update(List<T> parm)
        {
            return Db.Updateable(parm).RemoveDataCache().ExecuteCommand();
        }

        /// <summary>
        /// 按查询条件更新
        /// </summary>
        /// <param name="where"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> columns)
        {
            return Db.Updateable<T>().SetColumns(columns).Where(where).RemoveDataCache().ExecuteCommand();
        }

        #endregion

        #region 删除操作

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="parm">string</param>
        /// <returns></returns>
        public int Delete(object id)
        {
            return Db.Deleteable<T>(id).RemoveDataCache().ExecuteCommand();
            //return Db.Deleteable<T>(id).ExecuteCommand();
        }

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="parm">string</param>
        /// <returns></returns>
        public int Delete(object[] ids)
        {
            return Db.Deleteable<T>().In(ids).RemoveDataCache().ExecuteCommand();
        }

        /// <summary>
        /// 根据条件删除一条或多条数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> where)
        {
            //return Db.Deleteable<T>().Where(where).RemoveDataCache().ExecuteCommand();
            return Db.Deleteable<T>().Where(where).ExecuteCommand();
        }
        #endregion

    }
}
