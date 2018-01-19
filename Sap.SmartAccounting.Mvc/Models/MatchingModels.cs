using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sap.SmartAccounting.Mvc.Entities;

namespace Sap.SmartAccounting.Mvc.Models
{
    public class MatchingModels
    {
        public class PaymentListDto
        {
            public DateTime? MenuDate { get; set; }

            public List<PaymentDto> Payments { get; set; }
        }

        public class PaymentCreateDto
        {
            public PaymentDto Payment { get; set; }
        }
    }
}