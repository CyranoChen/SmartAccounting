using System.Collections.Generic;
using System.Data;
using System.Reflection;
using DataReaderMapper;
using DataReaderMapper.Mappers;

namespace Arsenalcn.Core
{
    public static class DataReaderMapperHelper
    {
        ///// <summary>
        ///// DataReader映射
        ///// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader) where T : class, IDao, new()
        {
            //Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Insert(0, new DataReaderMapper.DataReaderMapper {YieldReturnEnabled = false});

                var mapper = typeof (T).GetMethod("CreateMap", BindingFlags.Static | BindingFlags.Public);

                if (mapper != null)
                {
                    mapper.Invoke(null, null);
                }
                else
                {
                    cfg.CreateMap<IDataReader, T>();
                }
            });

            return Mapper.Map<IDataReader, IEnumerable<T>>(reader);
        }

        /// <summary>
        ///     IDataReader GetValueByColumnName Extension
        /// </summary>
        public static object GetValue(this IDataRecord reader, string colName)
        {
            if (reader != null && !string.IsNullOrEmpty(colName))
            {
                var index = reader.GetOrdinal(colName);

                if (index >= 0)
                {
                    return !reader.IsDBNull(index) ? reader.GetValue(index) : null;
                }
                return null;
            }
            return null;
        }
    }
}