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

        // GET: Account
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

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
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

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
