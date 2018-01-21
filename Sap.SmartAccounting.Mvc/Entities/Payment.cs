using System;
using Sap.SmartAccounting.Core;

namespace Sap.SmartAccounting.Mvc.Entities
{
    [DbSchema("Accounting_Payment", Sort = "CreateTime DESC")]
    public class Payment : Entity<int>
    {
        #region Members and Properties

        [DbColumn("B1Id")]
        public string B1Id { get; set; }

        [DbColumn("CompanyCode")]
        public string CompanyCode { get; set; }

        [DbColumn("CompanyName")]
        public string CompanyName { get; set; }

        [DbColumn("BankCode")]
        public string BankCode { get; set; }

        [DbColumn("BankName")]
        public string BankName { get; set; }

        [DbColumn("Amount")]
        public double Amount { get; set; }

        [DbColumn("ReceiveDate")]
        public DateTime ReceiveDate { get; set; }

        [DbColumn("Reference")]
        public string Reference { get; set; }

        [DbColumn("AccountId")]
        public int? AccountId { get; set; }

        [DbColumn("AccountCode")]
        public string AccountCode { get; set; }

        [DbColumn("AccountName")]
        public string AccountName { get; set; }

        [DbColumn("Status")]
        public PaymentStatusEnum Status { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        #endregion
    }

    public enum PaymentStatusEnum
    {
        Draft = 0,
        ToBePost = 1,
        Posted = 2
    }

}