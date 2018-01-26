using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;

namespace DapperHelper
{
    public class DapperHelper
    {
        private Dialect DbType;
        private ConnectionSelecter ConnSelecter;

        public DapperHelper(Dialect DbType = Dialect.SqlServer, ConnectionSelecter ConnSelecter = ConnectionSelecter.MsSqlLive)
        {
            this.DbType = DbType;
            this.ConnSelecter = ConnSelecter;
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 查詢Sql
        /// </summary>
        public List<TEntity> Query<TEntity>(string sql) where TEntity : class
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    return cn.Query<TEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 查詢Sql(單參數)
        /// </summary>
        public List<TEntity> Query<TEntity>(string sql, TEntity parameter)
            where TEntity : class
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    return cn.Query<TEntity>(sql, parameter).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 查詢Sql(Dynamic參數)
        /// </summary>
        public List<dynamic> Query(string sql, DynamicParameters dynamicparameter)
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    return cn.Query(sql, dynamicparameter).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 查詢第一筆資料(單參數)
        /// </summary>
        public TEntity QueryFirstOrDefault<TEntity>(string sql, TEntity parameter)
            where TEntity : class
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    return cn.QueryFirstOrDefault<TEntity>(sql, parameter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 查詢第一筆資料(Dynamic參數)
        /// </summary>
        public TEntity QueryFirstOrDefault<TEntity>(string sql, DynamicParameters dynamicparameter)
            where TEntity : class
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    return cn.QueryFirstOrDefault<TEntity>(sql, dynamicparameter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 執行Sql(單參數)
        /// </summary>
        public void Execute<TEntity>(string sql, TEntity parameter)
            where TEntity : class
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    cn.Execute(sql, parameter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 執行Sql(多參數)
        /// </summary>
        public void Execute<TEntity>(string sql, List<TEntity> parameters)
            where TEntity : class
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    cn.Execute(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 執行Sql
        /// </summary>
        public void Execute(string sql)
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    cn.Execute(sql);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 執行Sql(Dynamic參數)
        /// </summary>
        public void Execute(string sql, DynamicParameters dynamicparameter)
        {
            try
            {
                using (var cn = ConnectionFactory())
                {
                    cn.Execute(sql, dynamicparameter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 分頁
        /// </summary>
        public List<T> SearchByPagerNum<T>(List<T> list, int PageSize, int PageIndex)
        {
            return list.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// ConnectionFactory
        /// </summary>
        private DbConnection ConnectionFactory()
        {
            string ConnString = GetConnectionString(ConnSelecter);
            ConnectionAction Connection = null;

            switch (DbType)
            {
                case Dialect.SqlServer:
                    Connection = new ConnSqlServer(ConnString);
                    break;
                case Dialect.SqlLite:
                    Connection = new ConnSqlLite(ConnString);
                    break;
                default:
                    Connection = new ConnSqlServer(ConnString);
                    break;
            }

            return Connection.CreatConn();
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// 獲取連線字串
        /// </summary>
        private string GetConnectionString(ConnectionSelecter cs)
        {
            switch (cs)
            {
                case ConnectionSelecter.MsSqlTest:
                    return Properties.Settings.Default.TestMsDb;

                case ConnectionSelecter.MsSqlLive:
                    return Properties.Settings.Default.LiveMsDb;

                case ConnectionSelecter.SqlLiteLocalDb:
                    return $"data source={Properties.Settings.Default.SqlLiteDbPath}";

                default:
                    return Properties.Settings.Default.LiveMsDb;
            }
        }
        //-------------------------------------------------------------------
    }
}
