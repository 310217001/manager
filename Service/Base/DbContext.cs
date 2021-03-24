using Pi.Common;
using Pi.PiManager.Model.Models;
using SqlSugar;
using System;
using System.Diagnostics;

namespace Pi.PiManager.Service
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DbContext
    {

        public SqlSugarClient Db;   //用来处理事务多表查询和复杂的操作

        public static DbContext Current
        {
            get
            {
                return new DbContext();
            }
        }

        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = AppSettings.Configuration["DbConnection:ConnectionString"],
                DbType = (DbType)Convert.ToInt32(AppSettings.Configuration["DbConnection:DbType"]),
                //ConnectionString = "Host=192.168.1.122;UserName=root;Password=123;Database=templatebase_core;Port=3306;CharSet=utf8;",
                //DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                IsShardSameThread = true,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = new SugarCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = true
                }
            });
            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Debug.WriteLine(sql);
            };
        }

        public DbSet<T> DbTable<T>() where T : class, new()
        {
            return new DbSet<T>(Db);
        }

        public DbSet<Advert> AdvertDb => new DbSet<Advert>(Db);
    }

    /// <summary>
    /// 扩展ORM
    /// </summary>
    public class DbSet<T> : SimpleClient<T> where T : class, new()
    {
        public DbSet(SqlSugarClient context) : base(context)
        {

        }
    }
}
