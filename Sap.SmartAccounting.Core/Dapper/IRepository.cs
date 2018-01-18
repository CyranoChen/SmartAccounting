using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Sap.SmartAccounting.Core.Dapper
{
    public interface IRepository : IDisposable
    {
        T Single<T>(object key) where T : class, IEntity, new();
        T Single<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao, new();

        int Count<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao;

        bool Any<T>(object key) where T : class, IEntity;
        bool Any<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao;

        List<T> All<T>(IDbTransaction trans = null) where T : class, IDao, new();
        List<T> All<T>(IPager pager, string orderBy = null) where T : class, IDao, new();

        List<T> Query<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao, new();
        List<T> Query<T>(IPager pager, Expression<Func<T, bool>> whereBy, string orderBy = null) where T : class, IDao, new();
        List<T> Query<T>(Criteria criteria) where T : class, IDao, new();

        int Insert<T>(T instance) where T : class, IDao;
        int Insert<T>(T instance, out object key) where T : class, IEntity;

        int Update<T>(T instance) where T : class, IEntity;
        int Update<T>(T instance, Expression<Func<T, bool>> whereBy) where T : class, IDao;

        int Save<T>(T instance, out object key) where T : class, IEntity;
        int Save<T>(T instance, Expression<Func<T, bool>> whereBy) where T : class, IDao;

        int Delete<T>(object key) where T : class, IEntity;
        int Delete<T>(T instance) where T : class, IEntity;
        int Delete<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao;
    }
}