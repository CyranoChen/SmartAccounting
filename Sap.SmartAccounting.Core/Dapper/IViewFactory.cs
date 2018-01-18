using System.Collections.Generic;
using System.Data;

namespace Shsict.Core
{
    // Abstract Factory
    public interface IViewerFactory<T> where T : class, IViewer, new()
    {
        T Single(Criteria criteria, IDbTransaction trans = null);

        List<T> All(IDbTransaction trans = null);

        List<T> All(IPager pager, string orderBy = null, IDbTransaction trans = null);

        List<T> Query(Criteria criteria, IDbTransaction trans = null);
    }
}
