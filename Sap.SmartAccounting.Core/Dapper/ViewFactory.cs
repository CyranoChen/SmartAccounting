namespace Shsict.Core
{
    public abstract class ViewerFactory
    {
        protected IDapperHelper Dapper { get; set; }
        protected string ViewerSql { get; set; }
        protected string SplitOn { get; set; }
        protected DbSchema DbSchema { get; set; }

        protected string BuildSingleSql()
        {
            return $"SELECT * FROM ({ViewerSql}) AS {DbSchema.Name} WHERE {DbSchema.Key} = @key";
        }

        protected string BuildSingleSql(Criteria criteria)
        {
            var strWhere = criteria?.GetWhereClause();

            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " WHERE " + strWhere;
            }

            return $"SELECT * FROM ({ViewerSql}) AS {DbSchema.Name} {strWhere}";
        }

        protected string BuildAllSql()
        {
            return $"SELECT * FROM ({ViewerSql}) AS {DbSchema.Name} ORDER BY {DbSchema.Sort}";
        }

        protected string BuildAllSql(IPager pager, string orderBy = null)
        {
            var strOrderBy = " ORDER BY " + (!string.IsNullOrEmpty(orderBy) ? orderBy : DbSchema.Sort);

            // Get TotalCount First
            var countSql = $"SELECT COUNT(*) AS TotalCount FROM ({ViewerSql}) AS {DbSchema.Name}";

            pager.SetTotalCount(new DapperHelper().ExecuteScalar<int>(countSql));

            // Get Query Result
            var innerSql = $"SELECT ROW_NUMBER() OVER({strOrderBy}) AS RowNo, * FROM ({ViewerSql}) AS {DbSchema.Name}";

            return $"SELECT * FROM ({innerSql}) AS t WHERE t.RowNo BETWEEN {pager.CurrentPage * pager.PagingSize + 1} AND {(pager.CurrentPage + 1) * pager.PagingSize}";
        }

        protected string BuildQuerySql(Criteria criteria)
        {
            var strOrderBy = " ORDER BY " + (!string.IsNullOrEmpty(criteria?.OrderClause) ? criteria.OrderClause : DbSchema.Sort);

            var strWhere = criteria?.GetWhereClause();

            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " WHERE " + strWhere;
            }

            var sql = $"SELECT * FROM ({ViewerSql}) AS {DbSchema.Name} {strWhere} {strOrderBy}";

            if (criteria?.PagingSize > 0)
            {
                // Get TotalCount First
                var countSql = $"SELECT COUNT(*) AS TotalCount FROM ({ViewerSql}) AS {DbSchema.Name} {strWhere}";

                criteria.SetTotalCount(new DapperHelper().ExecuteScalar<int>(countSql, criteria.Parameters));

                // Get Query Result
                var innerSql =
                    $"SELECT ROW_NUMBER() OVER({strOrderBy}) AS RowNo, * FROM ({ViewerSql}) AS {DbSchema.Name} {strWhere}";

                sql =
                    $"SELECT * FROM ({innerSql}) AS t WHERE t.RowNo BETWEEN {criteria.CurrentPage * criteria.PagingSize + 1} AND {(criteria.CurrentPage + 1) * criteria.PagingSize}";
            }

            return sql;
        }
    }
}
