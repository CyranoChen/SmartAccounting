using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Mvc.Entities;
using Sap.SmartAccounting.Mvc.Models;
using Sap.SmartAccounting.Mvc.Services;

namespace Sap.SmartAccounting.Mvc.Controllers
{
    /// <summary>
    /// Smart Guess
    /// </summary>
    public class SmartMatchingController : ApiController
    {
        // GET api/<controller>
        /// <summary>
        /// Get Smart Account Depended by the Features of Payment
        /// </summary>
        /// <param name="paymentB1Id">Id of Payment in Business One</param>
        /// <param name="companyId">Leave empty</param>
        /// <param name="companyB1Id">Id of Customers or Supplier in Business One</param>
        /// <param name="bank">Bank Information</param>
        /// <param name="amount">Amount of Payment</param>
        /// <param name="reference">Remark</param>
        /// <returns>JSON format</returns>
        /// <exception cref="Exception"></exception>
        public AlgorithmModels.Result Get(string paymentB1Id, int companyId, string companyB1Id,
            string bank, double amount, string reference)
        {
            try
            {
                var param = new AlgorithmModels.Parameter
                {
                    PaymentB1Id = paymentB1Id,
                    Company = null,
                    BankCode = string.Empty,
                    BankName = bank,
                    Reference = reference,
                    Amount = amount
                };

                if (companyId <= 0 && string.IsNullOrEmpty(companyB1Id))
                { throw new Exception("Company Information is required"); }

                if (companyId <= 0 && !string.IsNullOrEmpty(companyB1Id))
                {
                    if (Company.Cache.CompanyListActive.Exists(x => x.B1Id.Equals(companyB1Id)))
                    {
                        param.Company = Company.Cache.CompanyListActive
                            .Find(x => x.B1Id.Equals(param.Company.B1Id)).MapTo<Company, CompanyDto>();
                    }
                    else
                    {
                        param.Company = null;
                    }
                }
                else
                {
                    param.Company = Company.Cache.Load(companyId).MapTo<Company, CompanyDto>();
                }

                var result = AlgorithmV1.SmartMatching(param);

                return result;
            }
            catch (Exception e)
            {
                return new AlgorithmModels.Result
                {
                    ResultAccount = null,
                    Probability = 0,
                    ResultType = AlgorithmModels.ResultTypeEnum.Exception,
                    Remark = e.Message
            };
        }
    }
}
}