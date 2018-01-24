using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Mvc.Entities;
using Sap.SmartAccounting.Mvc.Models;

namespace Sap.SmartAccounting.Mvc.Controllers
{
    public class SmartMatchingController : ApiController
    {
        // GET api/<controller>
        public AlgorithmModels.Result Get()
        {
            var account = Entities.Account.Cache.AccountListActive[0].MapTo<Account, AccountDto>();

            var result = new AlgorithmModels.Result
            {
                ResultAccount = account
            };

            return result;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}