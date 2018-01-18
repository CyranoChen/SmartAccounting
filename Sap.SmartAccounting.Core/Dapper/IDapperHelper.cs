using System;
using System.Collections.Generic;
using System.Data;

namespace Sap.SmartAccounting.Core.Dapper
{
    public interface IDapperHelper : IDisposable
    {
        IDbTransaction BeginTransaction();

        int Execute(string sql, object para = null, CommandType? commandType = null, bool ignoreLog = false);

        IDataReader ExecuteReader(string sql, object para = null, CommandType? commandType = null);
        DataTable ExecuteDataTable(string sql, object para = null, CommandType? commandType = null);

        object ExecuteScalar(string sql, object para = null, CommandType? commandType = null);
        T ExecuteScalar<T>(string sql, object para = null, CommandType? commandType = null);

        T QueryFirstOrDefault<T>(string sql, object para = null, CommandType? commandType = null);

        IEnumerable<dynamic> Query(string sql, object para = null, CommandType? commandType = null);

        IEnumerable<T> Query<T>(string sql, object para = null, CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T>(string sql, Func<T1, T2, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T>(string sql, Func<T1, T2, T3, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T4, T>(string sql, Func<T1, T2, T3, T4, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T4, T5, T>(string sql, Func<T1, T2, T3, T4, T5, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T4, T5, T6, T>(string sql, Func<T1, T2, T3, T4, T5, T6, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null);
    }
}
