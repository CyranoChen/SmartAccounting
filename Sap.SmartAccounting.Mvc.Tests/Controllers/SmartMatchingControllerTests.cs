using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sap.SmartAccounting.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Core.Extension;
using Sap.SmartAccounting.Mvc.Entities;
using Sap.SmartAccounting.Mvc.Models;
using Sap.SmartAccounting.Mvc.Services;

namespace Sap.SmartAccounting.Mvc.Controllers.Tests
{
    [TestClass()]
    public class SmartMatchingControllerTests
    {
        [TestMethod()]
        public void PostTest()
        {
            var client = new WebClient();

            //{ "amount":4000,"bank":"BOC.USD","companyB1Id":"CP100120","companyId":-1,"paymentB1Id":"ab2306345e6f78","reference":"a"}
            var uri = "http://pvgd55160875a:7080/api/SmartMatching";

            var data = new
            {
                amount = 4000,
                bank = "boc.usd",
                companyCode = "CP100120",
                companyId = -1,
                paymentB1Id = "ab2306345e6f78",
                reference = "a"
            };

            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("ContentLength", data.ToString().Length.ToString());

            var responseResult = client.UploadData(uri, RequestMethod.Post.ToString().ToUpper()
                , Encoding.UTF8.GetBytes(data.ToJson()));

            var result = Encoding.UTF8.GetString(responseResult);

            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod()]
        public void ApiPostTest()
        {
            var request = new
            {
                Amount = 4000,
                Bank = "boc.usd",
                CompanyCode = "CP100120",
                CompanyId = -1,
                PaymentB1Id = "ab2306345e6f78",
                Reference = "a"
            };

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

            Assert.IsNotNull(result);
        }
    }
}