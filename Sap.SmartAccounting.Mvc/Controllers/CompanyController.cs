using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sap.SmartAccounting.Core;
using Sap.SmartAccounting.Core.Dapper;
using Sap.SmartAccounting.Mvc.Entities;
using Sap.SmartAccounting.Mvc.Models;

namespace Sap.SmartAccounting.Mvc.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IRepository _repo = new Repository();

        // GET: Company
        public ActionResult Index()
        {
            var model = new SettingModels.CompanyListDto();

            var list = _repo.All<Company>().FindAll(x => x.IsActive);

            if (list.Count > 0)
            {
                model.Companies = list.MapToList<Company, CompanyDto>().ToList();
            }

            return View(model);
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var company = new Company()
                    {
                        B1Id = string.Empty,
                        CompanyCode = model.CompanyCode,
                        CompanyName = model.CompanyName,
                        CreateTime = DateTime.Now,
                        IsActive = true
                    };

                    _repo.Insert(company);

                    Entities.Company.Cache.RefreshCache();

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Warn", ex.Message);
                }
            }

            return View(model);
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Company/Edit/5
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

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
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
