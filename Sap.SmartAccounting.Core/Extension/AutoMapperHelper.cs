using System.Collections.Generic;
using AutoMapper;

namespace Sap.SmartAccounting.Core
{
    public static class AutoMapperHelper
    {
        /// <summary>
        ///     类型映射，返回新实例
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return default(TDestination);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());

            var mapper = config.CreateMapper();

            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        ///     类型映射，按传入实例返回
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return default(TDestination);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());

            var mapper = config.CreateMapper();

            return mapper.Map(source, destination);
        }

        /// <summary>
        ///     集合列表类型映射，返回新实例集合
        /// </summary>
        public static IEnumerable<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return default(IEnumerable<TDestination>);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());

            var mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<TDestination>>(source);
        }

        /// <summary>
        ///     集合列表类型映射，按传入实例集合返回
        /// </summary>
        public static IEnumerable<TDestination> MapToList<TSource, TDestination>
            (this IEnumerable<TSource> sources, IEnumerable<TDestination> destinations)
            where TSource : class
            where TDestination : class
        {
            if (sources == null) return default(IEnumerable<TDestination>);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());

            var mapper = config.CreateMapper();

            return mapper.Map(sources, destinations);
        }
    }
}