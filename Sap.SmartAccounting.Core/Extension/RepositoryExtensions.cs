using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sap.SmartAccounting.Core.Dapper;
using Dapper;

namespace Sap.SmartAccounting.Core.Extension
{
    public static class RepositoryExtensions
    {
        public static string ToSqlDebugInfo(this string sql, object para = null)
        {
            if (para != null)
            {
                var dp = para as DynamicParameters;

                if (dp != null)
                {
                    return new
                    {
                        sql,
                        para = (object) dp.ParameterNames.ToDictionary(x => x, x => dp.Get<dynamic>(x))
                    }
                    .ToJson();
                }

                return new { sql, para }.ToJson();
            }
            else
            {
                return sql;
            }
        }

        public static object ToDapperParameters(this SqlParameter[] para)
        {
            var args = new DynamicParameters(new { });

            foreach (var p in para)
            {
                args.Add(p.ParameterName, p.Value != DBNull.Value ? p.Value : null);
            }

            return args;
        }


        public static int Insert<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            if (list.Count > 0)
            {
                IRepository repo = new Repository();

                foreach (var instance in list)
                {
                    repo.Insert((IDao)instance);
                }
            }

            return list.Count;
        }

        public static int Update<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            if (list.Count > 0)
            {
                IRepository repo = new Repository();

                foreach (var instance in list)
                {
                    repo.Update(instance);
                }
            }

            return list.Count;
        }

        public static int Delete<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            if (list.Count > 0)
            {
                IRepository repo = new Repository();

                foreach (var instance in list)
                {
                    repo.Delete(instance);
                }
            }

            return list.Count;
        }
    }
}