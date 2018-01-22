using System;
using System.Collections.Generic;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Core.Dapper;

namespace Sap.SmartAccounting.Mvc.Entities
{
    [DbSchema("Accounting_Account", Sort = "AccountCode")]
    public class Account : Entity<int>
    {
        #region Members and Properties

        [DbColumn("B1Id")]
        public string B1Id { get; set; }

        [DbColumn("AccountCode")]
        public string AccountCode { get; set; }

        [DbColumn("AccountName")]
        public string AccountName { get; set; }

        [DbColumn("IsIncoming")]
        public bool IsIncoming { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        #endregion

        public static class Cache
        {
            public static List<Account> AccountList;
            public static List<Account> AccountListActive;

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

                AccountList = repo.All<Account>();
                AccountListActive = AccountList.FindAll(x => x.IsActive);
            }

            public static Account Load(int id)
            {
                return AccountListActive.Find(x => x.ID.Equals(id));
            }
        }
    }
}