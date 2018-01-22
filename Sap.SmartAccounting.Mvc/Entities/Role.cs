using System;
using System.Collections.Generic;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Core.Dapper;

namespace Sap.SmartAccounting.Mvc.Entities
{
    [DbSchema("Accounting_Role", Sort = "CreateTime DESC")]
    public class Role : Entity<int>
    {
        #region Members and Properties
        [DbColumn("CompanyId")]
        public int CompanyId { get; set; }

        [DbColumn("AccountId")]
        public int AccountId { get; set; }

        [DbColumn("IsIncoming")]
        public bool IsIncoming { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        #endregion

        public static class Cache
        {
            public static List<Role> RoleList;
            public static List<Role> RoleListActive;

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

                RoleList = repo.All<Role>();
                RoleListActive = RoleList.FindAll(x => x.IsActive);
            }
        }
    }
}