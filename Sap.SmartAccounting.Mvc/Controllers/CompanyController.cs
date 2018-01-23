using System;
using System.Linq;
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

            var list = Company.Cache.CompanyListActive;

            if (list.Count > 0)
            {
                model.Companies = list.MapToList<Company, CompanyDto>().ToList();
            }

            return View(model);
        }

        //// GET: Company/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

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
                    var company = new Company
                    {
                        B1Id = string.Empty,
                        CompanyCode = model.CompanyCode,
                        CompanyName = model.CompanyName,
                        CreateTime = DateTime.Now,
                        IsActive = true
                    };

                    _repo.Insert(company);

                    Company.Cache.RefreshCache();

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
            var model = Company.Cache.Load(id).MapTo<Company, CompanyDto>();

            var roles = Role.Cache.RoleListActive.FindAll(x => x.CompanyId.Equals(id));

            if (roles.Count > 0)
            {
                if (roles.Exists(x => x.IsIncoming))
                {
                    model.IncomingAccountId = roles.Find(x => x.IsIncoming).AccountId;
                    model.IncomingAccount = Account.Cache.Load(model.IncomingAccountId).MapTo<Account, AccountDto>();
                }

                if (roles.Exists(x => !x.IsIncoming))
                {
                    model.OutcomingAccountId = roles.Find(x => !x.IsIncoming).AccountId;
                    model.OutcomingAccount = Account.Cache.Load(model.OutcomingAccountId).MapTo<Account, AccountDto>();
                }
            }

            return View(model);
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CompanyDto model)
        {
            if (ModelState.IsValid)
            {
                using (var dapper = DapperHelper.GetInstance())
                {
                    var trans = dapper.BeginTransaction();

                    try
                    {
                        // ReSharper disable once RedundantBoolCompare
                        var roles = _repo.Query<Role>(x => x.CompanyId == model.ID && x.IsActive == true);

                        if (roles != null && roles.Count > 0)
                        {
                            foreach (var r in roles)
                            {
                                r.IsActive = false;
                                _repo.Update(r);
                            }
                        }

                        if (model.IncomingAccountId > 0)
                        {
                            var roleIn = new Role
                            {
                                AccountId = model.IncomingAccountId,
                                CompanyId = model.ID,
                                CreateTime = DateTime.Now,
                                IsActive = true,
                                IsIncoming = true
                            };

                            _repo.Insert(roleIn);
                        }

                        if (model.OutcomingAccountId > 0)
                        {
                            var roleOut = new Role
                            {
                                AccountId = model.OutcomingAccountId,
                                CompanyId = model.ID,
                                CreateTime = DateTime.Now,
                                IsActive = true,
                                IsIncoming = false
                            };

                            _repo.Insert(roleOut);
                        }

                        trans.Commit();

                        Company.Cache.RefreshCache();
                        Role.Cache.RefreshCache();

                        ModelState.AddModelError("Success", "Save Successfully");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();

                        ModelState.AddModelError("Warn", ex.Message);
                    }
                }
            }

            return View(model);
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var company = _repo.Single<Company>(id);

                if (company != null)
                {
                    company.IsActive = false;

                    _repo.Update(company);

                    Company.Cache.RefreshCache();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction("Index", "Company");
        }
    }
}
