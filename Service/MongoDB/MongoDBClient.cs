using MongoDB.Driver;
using Pi.Common;

namespace Pi.PiManager.Service.MongoDB
{
    public class MongoDBClient<T> where T : class
    {
        protected MongoDBClient()
        {

        }

        /// <summary>
        /// 获取集合实例
        /// </summary>
        /// <param name="host">连接字符串，库，集合</param>
        /// <returns></returns>
        public static IMongoCollection<T> GetCollectionInstance(string Collection)
        {
            string DataBase = AppSettings.Configuration["DbConnection:MongoDBName"];
            string ConnectionString = AppSettings.Configuration["DbConnection:MongoDBConnection"];
            MongoClient client = new MongoClient(ConnectionString);
            var dataBase = client.GetDatabase(DataBase);
            return dataBase.GetCollection<T>(Collection);
        }
    }
}
