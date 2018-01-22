using System;
using System.Linq;
using System.Web.Mvc;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Core.Dapper;
using Sap.SmartAccounting.Mvc.Entities;
using Sap.SmartAccounting.Mvc.Models;

namespace Sap.SmartAccounting.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository _repo = new Repository();

        // GET: AccountId
        public ActionResult Index()
        {
            var model = new SettingModels.AccountListDto();

            var list = Entities.Account.Cache.AccountListActive;

            if (list.Count > 0)
            {
                model.Accounts = list.MapToList<Account, AccountDto>().ToList();
            }

            return View(model);
        }

        //// GET: AccountId/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: AccountId/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountId/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = new Account
                    {
                        B1Id = string.Empty,
                        AccountCode = model.AccountCode,
                        AccountName = model.AccountName,
                        IsIncoming = model.IsIncoming,
                        CreateTime = DateTime.Now,
                        IsActive = true
                    };

                    _repo.Insert(account);

                    Entities.Account.Cache.RefreshCache();

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Warn", ex.Message);
                }
            }

            return View(model);
        }

        //// GET: AccountId/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AccountId/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: AccountId/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var account = _repo.Single<Account>(id);

                if (account != null)
                {
                    account.IsActive = false;

                    _repo.Update(account);

                    Account.Cache.RefreshCache();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction("Index", "Account");
        }
    }
}
