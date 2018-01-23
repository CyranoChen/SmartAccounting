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
    public class RoleController : Controller
    {
        private readonly IRepository _repo = new Repository();

        // GET: Role
        public ActionResult Index()
        {
            var model = new SettingModels.RoleListDto();

            var list = Entities.Role.Cache.RoleListActive;

            if (list.Count > 0)
            {
                var mapper = RoleDto.ConfigMapper().CreateMapper();
                var roles = mapper.Map<IEnumerable<RoleDto>>(list.AsEnumerable()).ToList();

                model.Roles = roles;
            }

            return View(model);
        }

        //// GET: Role/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Role/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Role/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Role/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Role/Edit/5
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

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var role = _repo.Single<Role>(id);

                if (role != null)
                {
                    role.IsActive = false;

                    _repo.Update(role);
                    
                    Role.Cache.RefreshCache();
                    Company.Cache.RefreshCache();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction("Index", "Role");
        }
    }
}
