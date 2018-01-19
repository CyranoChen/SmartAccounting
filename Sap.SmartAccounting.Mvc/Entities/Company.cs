using System;
using Sap.SmartAccounting.Core;

namespace Sap.SmartAccounting.Mvc.Entities
{
    [DbSchema("Accounting_Company", Sort = "CompanyCode")]
    public class Company : Entity<int>
    {
        #region Members and Properties

        [DbColumn("B1Id")]
        public string B1Id { get; set; }

        [DbColumn("CompanyCode")]
        public string CompanyCode { get; set; }

        [DbColumn("CompanyName")]
        public string CompanyName { get; set; }

        [DbColumn("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        #endregion
    }
}