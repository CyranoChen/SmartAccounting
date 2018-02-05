using System;
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
        /// (obsolete) Get Smart Account Depended by the Features of Payment
        /// </summary>
        /// <param name="paymentB1Id">Id of Payment in Business One</param>
        /// <param name="companyId">Leave empty</param>
        /// <param name="companyCode">Id of Customers or Supplier in Business One</param>
        /// <param name="bank">Bank Information</param>
        /// <param name="amount">Amount of Payment</param>
        /// <param name="reference">Remark</param>
        /// <returns>JSON format</returns>
        /// <exception cref="Exception"></exception>
        public AlgorithmModels.Result Get(string paymentB1Id, int companyId, string companyCode,
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

                if (companyId <= 0 && string.IsNullOrEmpty(companyCode))
                { throw new Exception("Company Information is required"); }

                if (companyId <= 0 && !string.IsNullOrEmpty(companyCode))
                {
                    if (Company.Cache.CompanyListActive.Exists(x => x.CompanyCode.Equals(companyCode)))
                    {
                        param.Company = Company.Cache.CompanyListActive
                            .Find(x => x.CompanyCode.Equals(param.Company.CompanyCode)).MapTo<Company, CompanyDto>();
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

        /// <summary>
        /// Parameter Object of SmartMatching API
        /// </summary>
        public class SmartMatchingRequest
        {
            #region Members and Properties

            /// <summary>
            /// Id of Payment in Business One
            /// </summary>
            public string PaymentB1Id { get; set; }

            /// <summary>
            /// Leave empty
            /// </summary>
            public int CompanyId { get; set; }

            /// <summary>
            /// Code of Customers or Supplier in Business One
            /// </summary>
            public string CompanyCode { get; set; }

            /// <summary>
            /// Bank Information
            /// </summary>
            public string Bank { get; set; }

            /// <summary>
            /// Amount of Payment
            /// </summary>
            public double Amount { get; set; }

            /// <summary>
            /// Remark
            /// </summary>
            public string Reference { get; set; }

            #endregion
        }

        // POST api/<controller>
        /// <summary>
        /// Require Smart Account Depended by the Features of Payment
        /// </summary>
        /// <returns>JSON format</returns>
        /// <exception cref="Exception"></exception>
        public AlgorithmModels.Result Post(SmartMatchingRequest request)
        {
            try
            {
                var param = new AlgorithmModels.Parameter
                {
                    PaymentB1Id = request.PaymentB1Id,
                    Company = null,
                    BankCode = string.Empty,
                    BankName = request.Bank,
                    Reference = request.Reference,
                    Amount = request.Amount
                };

                if (request.CompanyId <= 0 && string.IsNullOrEmpty(request.CompanyCode))
                { throw new Exception("Company Information is required"); }

                if (request.CompanyId <= 0 && !string.IsNullOrEmpty(request.CompanyCode))
                {
                    if (Company.Cache.CompanyListActive.Exists(x => x.CompanyCode.Equals(request.CompanyCode)))
                    {
                        param.Company = Company.Cache.CompanyListActive
                            .Find(x => x.CompanyCode.Equals(request.CompanyCode)).MapTo<Company, CompanyDto>();
                    }
                    else
                    {
                        param.Company = null;
                    }
                }
                else
                {
                    param.Company = Company.Cache.Load(request.CompanyId).MapTo<Company, CompanyDto>();
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