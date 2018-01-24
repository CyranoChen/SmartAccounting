using System;
using System.Linq;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Core.Dapper;
using Sap.SmartAccounting.Mvc.Entities;
using Sap.SmartAccounting.Mvc.Models;

namespace Sap.SmartAccounting.Mvc.Services
{
    public static class AlgorithmV1
    {
        public static AlgorithmModels.Result SmartMatching(AlgorithmModels.Parameter param)
        {
            if (param.Amount.Equals(0f))
            { throw new Exception("Amount is required or equals zero"); }

            if (param.Company.ID <= 0 && string.IsNullOrEmpty(param.Company.B1Id))
            { throw new Exception("Company Information is required"); }

            AccountDto account;
            AlgorithmModels.ResultTypeEnum resultType;
            double probability = 0f;
            var remark = string.Empty;

            using (IRepository repo = new Repository())
            {
                if (param.Company != null)
                {
                    var role = Role.Cache.RoleListActive.Find(x => x.CompanyId.Equals(param.Company.ID)
                                                                    && x.IsIncoming.Equals(param.Amount > 0));

                    if (role != null)
                    {
                        // Decide by Role
                        account = Account.Cache.Load(role.AccountId).MapTo<Account, AccountDto>();
                        resultType = AlgorithmModels.ResultTypeEnum.RoleBase;
                        probability = 1.0;
                        remark = "Exactly matched";
                    }
                    else
                    {
                        // Decide by histric Data
                        if (string.IsNullOrEmpty(param.BankName) && string.IsNullOrEmpty(param.BankCode))
                        { throw new Exception("Bank Information is required"); }

                        // ReSharper disable once RedundantBoolCompare
                        var payments = repo.Query<Payment>(x => x.IsActive == true &&
                                                                (x.CompanyName == param.Company.CompanyName ||
                                                                 x.CompanyCode == param.Company.CompanyCode) &&
                                                                (x.BankName == param.BankName ||
                                                                 x.BankCode == param.BankCode))
                            .FindAll(x => x.AccountId > 0 && x.Status.Equals(PaymentStatusEnum.Posted) &&
                                          (x.Amount > 0).Equals(param.Amount > 0));

                        if (payments.Count > 0)
                        {
                            var query = (from p in payments
                                         group p by p.AccountId into g
                                         orderby g.Count() descending
                                         select new { g.Key, Nums = g.Count() }).ToList();

                            account = Account.Cache.Load((int)query.First().Key).MapTo<Account, AccountDto>();
                            resultType = AlgorithmModels.ResultTypeEnum.HistoricData;
                            probability = Math.Round((double)query[0].Nums / payments.Count, 2);
                            remark = "Guessed";
                        }
                        else
                        {
                            // Decide by Reference Matching
                            if (string.IsNullOrEmpty(param.Reference))
                            { throw new Exception("Reference is required"); }

                            // ReSharper disable once RedundantBoolCompare
                            payments = repo.Query<Payment>(x => x.IsActive == true)
                                .FindAll(x => x.AccountId > 0 && x.Status.Equals(PaymentStatusEnum.Posted) &&
                                              (x.Amount > 0).Equals(param.Amount > 0) &&
                                              x.Reference.Equals(param.Reference, StringComparison.OrdinalIgnoreCase));

                            if (payments.Count > 0)
                            {
                                var query = (from p in payments
                                             group p by p.AccountId into g
                                             orderby g.Count() descending
                                             select new { g.Key, Nums = g.Count() }).ToList();

                                account = Account.Cache.Load((int)query[0].Key).MapTo<Account, AccountDto>();
                                resultType = AlgorithmModels.ResultTypeEnum.ReferenceMatching;
                                probability = Math.Round((double)query[0].Nums / payments.Count, 2);
                                remark = "Guessed";
                            }
                            else
                            {
                                account = null;
                                resultType = AlgorithmModels.ResultTypeEnum.None;
                                probability = 0f;
                                remark = "No matched";
                            }
                        }
                    }
                }
                else
                {
                    account = null;
                    resultType = AlgorithmModels.ResultTypeEnum.None;
                    probability = 0f;
                }
            }

            var result = new AlgorithmModels.Result
            {
                PaymentB1Id = param.PaymentB1Id,
                ResultAccount = account,
                Probability = probability,
                ResultType = resultType,
                Remark = remark
            };

            return result;
        }
    }
}