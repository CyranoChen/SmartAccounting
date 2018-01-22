using System.Collections.Generic;

namespace Sap.SmartAccounting.Mvc.Models
{
    public class SettingModels
    {
        public class CompanyListDto
        {
            public List<CompanyDto> Companies { get; set; }
        }

        public class AccountListDto
        {
            public List<AccountDto> Accounts { get; set; }
        }
    }
}