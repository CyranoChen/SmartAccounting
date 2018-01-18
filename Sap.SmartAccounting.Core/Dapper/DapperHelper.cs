using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using System.Web.Configuration;
using Sap.SmartAccounting.Core.Extension;
using Sap.SmartAccounting.Core.Logger;
using Dapper;

namespace Sap.SmartAccounting.Core.Dapper
{
    public class DapperHelper : IDapperHelper
    {
        private static string _connectionString;
        private static string ConnectionString => _connectionString ?? (_connectionString =
            ConfigurationManager.ConnectionStrings["Sqlserver.ConnectionString"].ConnectionString);

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private DapperHelper() { }
        private DapperHelper(string conn) { _connectionString = conn; }

        public static IDapperHelper GetInstance(bool mars = false)
        {
            IDapperHelper instance = new DapperHelper
            {
                _connection = GetOpenConnection(mars)
            };

            return instance;
        }

        public static IDapperHelper GetInstance(string conn, bool mars = false)
        {
            IDapperHelper instance = new DapperHelper(conn)
            {
                _connection = GetOpenConnection(mars)
            };

            return instance;
        }

        private static SqlConnection GetOpenConnection(bool mars = false)
        {
            var cs = ConnectionString;
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

        public IDbTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();

            return _transaction;
        }

        private readonly ILog _log = new DaoLog();

        private bool DebugMode
        {
            get
            {
                var compilation = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");

                return compilation != null && compilation.Debug;
            }
        }

        private int CommandTimeout => 90;

        //private static SqlConnection GetClosedConnection()
        //{
        //    var conn = new SqlConnection(ConnectionString);
        //    if (conn.State != ConnectionState.Closed) throw new InvalidOperationException("should be closed!");
        //    return conn;
        //}

        public int Execute(string sql, object para = null, CommandType? commandType = null, bool ignoreLog = false)
        {
            if (DebugMode && !ignoreLog)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Execute(sql, para, _transaction, CommandTimeout, commandType);
        }

        public IDataReader ExecuteReader(string sql, object para = null, CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.ExecuteReader(sql, para, _transaction, CommandTimeout, commandType);
        }

        public DataTable ExecuteDataTable(string sql, object para = null, CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            using (var reader = _connection.ExecuteReader(sql, para, _transaction, CommandTimeout, commandType))
            {
                var dt = new DataTable();

                var intFieldCount = reader.FieldCount;

                for (var intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    dt.Columns.Add(reader.GetName(intCounter).ToUpper(), reader.GetFieldType(intCounter));
                }

                dt.BeginLoadData();

                var values = new object[intFieldCount];
                while (reader.Read())
                {
                    reader.GetValues(values);
                    dt.LoadDataRow(values, true);
                }

                dt.EndLoadData();

                return dt.Rows.Count > 0 ? dt : null;
            }
        }

        public object ExecuteScalar(string sql, object para = null, CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.ExecuteScalar(sql, para, _transaction, CommandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object para = null, CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.ExecuteScalar<T>(sql, para, _transaction, CommandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object para = null, CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.QueryFirstOrDefault<T>(sql, para, _transaction, CommandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(string sql, object para = null, CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Query(sql, para, _transaction, true, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object para = null, CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Query<T>(sql, para, _transaction, true, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T>(string sql, Func<T1, T2, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Query(sql, map, para, _transaction, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T>(string sql, Func<T1, T2, T3, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Query(sql, map, para, _transaction, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T>(string sql, Func<T1, T2, T3, T4, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Query(sql, map, para, _transaction, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T5, T>(string sql, Func<T1, T2, T3, T4, T5, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Query(sql, map, para, _transaction, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T5, T6, T>(string sql, Func<T1, T2, T3, T4, T5, T6, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            if (DebugMode)
            {
                _log.Debug(sql.ToSqlDebugInfo(para), new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }

            return _connection.Query(sql, map, para, _transaction, true, splitOn, CommandTimeout, commandType);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}