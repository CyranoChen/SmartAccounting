using System;
using System.Collections.Generic;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Core.Dapper;

namespace Sap.SmartAccounting.Mvc.Entities
{
    [DbSchema("Accounting_Company", Sort = "CompanyCode")]
    public class Company : Entity<int>
    {
        public override void Inital()
        {
            CompanyDisplay = $"{CompanyName} ({CompanyCode})";
        }

        #region Members and Properties

        [DbColumn("B1Id")]
        public string B1Id { get; set; }

        [DbColumn("CompanyCode")]
        public string CompanyCode { get; set; }

        [DbColumn("CompanyName")]
        public string CompanyName { get; set; }

        public string CompanyDisplay { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        #endregion

        public static class Cache
        {
            public static List<Company> CompanyList;
            public static List<Company> CompanyListActive;

            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                IRepository repo = new Repository();

                CompanyList = repo.All<Company>();
                CompanyListActive = CompanyList.FindAll(x => x.IsActive);
            }

            public static Company Load(int id)
            {
                return CompanyListActive.Find(x => x.ID.Equals(id));
            }
        }

    }
}