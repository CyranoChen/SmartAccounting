using System;
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

            if (param.Company.ID <= 0 && !string.IsNullOrEmpty(param.Company.B1Id))
            {
                if (Company.Cache.CompanyListActive.Exists(x => x.B1Id.Equals(param.Company.B1Id)))
                {
                    param.Company = Company.Cache.CompanyListActive
                        .Find(x => x.B1Id.Equals(param.Company.B1Id)).MapTo<Company, CompanyDto>();
                }
                else
                {
                    param.Company = null;
                }
            }

            IRepository repo = new Repository();

            AccountDto account;
            AlgorithmModels.ResultTypeEnum resultType;
            double probability = 0f;

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
                        account = Account.Cache.Load(payments[0].AccountId.Value).MapTo<Account, AccountDto>(); ;
                        resultType = AlgorithmModels.ResultTypeEnum.HistoricData;
                        probability = 0.5;
                    }
                    else
                    {
                        // Decide by Reference Matching
                        if (string.IsNullOrEmpty(param.Reference))
                        { throw new Exception("Reference is required"); }

                        // ReSharper disable once RedundantBoolCompare
                        payments = repo.Query<Payment>(x => x.IsActive == true && x.Reference == param.Reference)
                            .FindAll(x => x.AccountId > 0 && x.Status.Equals(PaymentStatusEnum.Posted) &&
                                          (x.Amount > 0).Equals(param.Amount > 0));

                        if (payments.Count > 0)
                        {
                            account = Account.Cache.Load(payments[0].AccountId.Value).MapTo<Account, AccountDto>(); ;
                            resultType = AlgorithmModels.ResultTypeEnum.ReferenceMatching;
                            probability = 0.5;
                        }
                        else
                        {
                            account = null;
                            resultType = AlgorithmModels.ResultTypeEnum.None;
                            probability = 0f;
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

            var result = new AlgorithmModels.Result
            {
                PaymentId = param.PaymentId,
                PaymentB1Id = param.PaymentB1Id,
                ResultAccount = account,
                Probability = probability,
                ResultType = resultType
            };

            return result;
        }
    }
}