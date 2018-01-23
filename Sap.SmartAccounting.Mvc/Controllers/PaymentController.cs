using System;
using Sap.SmartAccounting.Core.Dapper;
using Sap.SmartAccounting.Mvc.Entities;
using Sap.SmartAccounting.Mvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sap.SmartAccounting.Mvc.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IRepository _repo = new Repository();

        // GET: Payment
        public ActionResult Index()
        {
            var model = new MatchingModels.PaymentListDto();

            var list = _repo.All<Payment>().FindAll(x => x.IsActive);

            if (list.Count > 0)
            {
                var mapper = PaymentDto.ConfigMapper().CreateMapper();
                var payments = mapper.Map<IEnumerable<PaymentDto>>(list.AsEnumerable()).ToList();

                model.Payments = payments;
            }

            return View(model);
        }

        //// GET: Payment/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Payment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = Entities.Account.Cache.Load(model.AccountId);
                    if (account == null) { throw new Exception("The selected account is invaild"); }

                    var payment = new Payment()
                    {
                        B1Id = string.Empty,
                        CompanyCode = string.Empty,
                        CompanyName = model.Company,
                        BankCode = string.Empty,
                        BankName = model.Bank,
                        Amount = model.Amount,
                        ReceiveDate = model.ReceiveDate,
                        Reference = model.Reference,
                        AccountId = account.ID,
                        AccountCode = account.AccountCode,
                        AccountName = account.AccountName,
                        Status = PaymentStatusEnum.ToBePost,
                        CreateTime = DateTime.Now,
                        IsActive = true
                    };

                    if (int.TryParse(model.Company, out var tmpId))
                    {
                        var company = Company.Cache.Load(tmpId);

                        if (company != null)
                        {
                            payment.CompanyCode = company.CompanyCode;
                            payment.CompanyName = company.CompanyName;
                        }
                    }

                    _repo.Insert(payment);

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Warn", ex.Message);
                }
            }

            return View();
        }

        // AJAX JsonResult
        // GET: Payment/SmartAccountMatching?company=&bank=&amount=
        public JsonResult SmartAccountMatching(int company, string bank, double amount)
        {
            //var list = Entities.Account.Cache.AccountListActive.FindAll(x => x.IsIncoming.Equals(amount > 0));

            return Json(new { Result = "info", Company = company, Bank = bank, Amount = amount, Porbability = "90%", Account = 1}, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var payment = _repo.Single<Payment>(id);

                if (payment != null)
                {
                    payment.IsActive = false;

                    _repo.Update(payment);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction("Index", "Payment");
        }
    }
}
