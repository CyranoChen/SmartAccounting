using System;
using System.Collections.Generic;
using System.Linq;
using Sap.SmartAccounting.Core.Dapper;

namespace Sap.SmartAccounting.Core.Extension
{
    public static class CollectionExtensions
    {
        // Many by Linq
        public static IEnumerable<TOne> Many<TOne, TMany>(this IEnumerable<TOne> source, Func<TOne, TMany, bool> func)
            where TOne : class, IMany, new()
            where TMany : class, IDao, new()
        {
            // Get the property which matches IEnumerable<TMany>
            var property = typeof(TOne).GetProperties()
                .FirstOrDefault(
                    x => (Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType) == typeof(IEnumerable<TMany>));

            var instances = source as IList<TOne> ?? source.ToList();

            if (instances.Count > 0 && property != null)
            {
                //var propertyName = string.Format("List{0}", typeof(TMany).Name);
                //var property = typeof(TOne).GetProperty(propertyName, typeof(IEnumerable<TMany>));

                var attrCol = Repository.GetColumnAttr(property);

                if (!string.IsNullOrEmpty(attrCol?.ForeignKey))
                {
                    IRepository repo = new Repository();

                    var list = repo.All<TMany>();

                    #region Package each property Ts in TSource

                    if (list != null && list.Count > 0)
                    {
                        var pi = typeof(TOne).GetProperty(property.Name, typeof(IEnumerable<TMany>));

                        if (pi != null)
                        {
                            foreach (var instance in instances)
                            {
                                var predicate = new Predicate<TMany>(t => func(instance, t));

                                var result = list.FindAll(predicate);

                                if (result.Count > 0)
                                {
                                    pi.SetValue(instance, result, null);
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            return instances;
        }

        // Many by T-SQL
        public static IEnumerable<TOne> Many<TOne, TMany, TOneKey>(this IEnumerable<TOne> source,
            Func<TOne, TOneKey> keySelector)
            where TMany : class, IDao, new()
            where TOne : class, IMany, new()
            where TOneKey : struct
        {
            // Get the property which matches IEnumerable<TMany>
            var property = typeof(TOne).GetProperties()
                .FirstOrDefault(
                    x => (Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType) == typeof(IEnumerable<TMany>));

            var instances = source as IList<TOne> ?? source.ToList();

            if (instances.Count > 0 && property != null)
            {
                //var propertyName = string.Format("List{0}", typeof(TMany).Name);
                //var property = typeof(TOne).GetProperty(propertyName, typeof(IEnumerable<TMany>));

                var attr = Repository.GetTableAttr<TMany>();
                var attrCol = Repository.GetColumnAttr(property);
                var fKey = typeof(TMany).GetProperty(attrCol.ForeignKey);

                if (attr != null && !string.IsNullOrEmpty(attrCol.ForeignKey) && fKey != null)
                {
                    #region Get All T instances where ForeignKey in T.PrimaryKeys

                    var keys = instances.Select(keySelector).ToArray();

                    IRepository repo = new Repository();

                    var query = from many in repo.All<TMany>()
                                join key in keys on fKey.GetValue(many, null) equals key
                                select many;

                    var list = (query as TMany[] ?? query.ToArray()).ToList();

                    #endregion

                    #region Package each property Ts in TSource

                    if (list.Count > 0)
                    {
                        var pi = typeof(TOne).GetProperty(property.Name, typeof(IEnumerable<TMany>));

                        if (pi != null)
                        {
                            foreach (var instance in instances)
                            {
                                var keyValue = keySelector(instance);

                                var predicate = new Predicate<TMany>(many => fKey.GetValue(many, null).Equals(keyValue));

                                var result = list.FindAll(predicate);

                                if (result.Count > 0)
                                {
                                    pi.SetValue(instance, result, null);
                                    list.RemoveAll(predicate);
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            return instances;
        }


        // Load All Records
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var skip = pageIndex * pageSize;

            if (skip > 0)
                source = source.Skip(skip);

            source = source.Take(pageSize);

            return source;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            return source.Where(instance => seenKeys.Add(keySelector(instance)));
        }

        public static IEnumerable<TKey> DistinctOrderBy<T, TKey>(this IEnumerable<T> instances,
            Func<T, TKey> keySelector)
        {
            return instances.DistinctBy(keySelector).OrderBy(keySelector).Select(keySelector);
        }
    }

    public interface IMany { }
}