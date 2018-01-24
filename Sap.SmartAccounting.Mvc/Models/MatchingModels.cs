using System;
using System.Collections.Generic;

namespace Sap.SmartAccounting.Mvc.Models
{
    public class MatchingModels
    {
        public class PaymentListDto
        {
            public List<PaymentDto> Payments { get; set; }
        }
    }
}