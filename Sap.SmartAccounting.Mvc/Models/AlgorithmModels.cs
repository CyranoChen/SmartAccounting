namespace Sap.SmartAccounting.Mvc.Models
{
    public class AlgorithmModels
    {
        public class Parameter
        {
            #region Members and Properties

            public int PaymentId { get; set; }

            public string PaymentB1Id { get; set; }

            public CompanyDto Company { get; set; }

            public string BankCode { get; set; }

            public string BankName { get; set; }

            public double Amount { get; set; }

            public string Reference { get; set; }

            #endregion
        }

        public class Result
        {
            #region Members and Properties

            public int PaymentId { get; set; }

            public string PaymentB1Id { get; set; }

            public ResultTypeEnum ResultType { get; set; }

            public AccountDto ResultAccount { get; set; }

            public double Probability { get; set; }

            #endregion
        }

        public enum ResultTypeEnum
        {
            None = 0,
            RoleBase = 1,
            HistoricData = 2,
            ReferenceMatching = 3
        }
    }
}