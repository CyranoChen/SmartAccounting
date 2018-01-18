using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using Arsenalcn.Core.Logger;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Core
{
    public static class DataAccess
    {
        public static readonly string ConnectString;
        private static bool _debugMode;
        private static ILog _log = new DaoLog();

        static DataAccess()
        {
            ConnectString = ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectString"].ConnectionString;
            _debugMode = ConfigurationManager.AppSettings["DebugMode"] != null &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);
        }

        public static SqlConnection GetOpenConnection(bool mars = false)
        {
            var cs = ConnectString;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs)
                {
                    MultipleActiveResultSets = true
                };

                cs = scsb.ConnectionString;
            }
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        public static SqlConnection GetClosedConnection()
        {
            var conn = new SqlConnection(ConnectString);
            if (conn.State != ConnectionState.Closed) throw new InvalidOperationException("should be closed!");
            return conn;
        }

        public static DataSet ExecuteDataset(string sql, SqlParameter[] para = null)
        {
            try
            {
                var ds = SqlHelper.ExecuteDataset(ConnectString, CommandType.Text, sql, para);

                if (_debugMode)
                {
                    _log.Debug(OutputSqlCommand(sql, para), new LogInfo
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });
                }

                return ds;
            }
            catch
            {
                _log.Error(OutputSqlCommand(sql, para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public static void ExecuteNonQuery(string sql, SqlParameter[] para = null, SqlTransaction trans = null)
        {
            try
            {
                Contract.Requires(!string.IsNullOrEmpty(sql));

                if (trans != null)
                {
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(ConnectString, CommandType.Text, sql, para);
                }

                if (_debugMode)
                {
                    _log.Debug(OutputSqlCommand(sql, para), new LogInfo
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });
                }
            }
            catch
            {
                _log.Error(OutputSqlCommand(sql, para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public static object ExecuteScalar(string sql, SqlParameter[] para = null, SqlTransaction trans = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(sql));

            try
            {
                object key;

                if (trans != null)
                {
                    key = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, para);
                }
                else
                {
                    key = SqlHelper.ExecuteScalar(ConnectString, CommandType.Text, sql, para);
                }

                if (_debugMode)
                {
                    _log.Debug(OutputSqlCommand(sql, para), new LogInfo
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });
                }

                return key;
            }
            catch
            {
                _log.Error(OutputSqlCommand(sql, para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        private static string OutputSqlCommand(string sql, SqlParameter[] para = null)
        {
            if (para != null && para.Length > 0)
            {
                var jsonSerializer = new JavaScriptSerializer();

                var result = new
                {
                    sql,
                    para = para.MapToList<SqlParameter, SqlParameterDto>()
                };

                return jsonSerializer.Serialize(result);
            }

            return sql;
        }

        private class SqlParameterDto
        {
            public string ParameterName { get; set; }
            public object Value { get; set; }
        }
    }
}