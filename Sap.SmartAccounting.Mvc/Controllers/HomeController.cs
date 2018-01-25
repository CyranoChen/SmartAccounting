using System.Web.Mvc;

namespace Sap.SmartAccounting.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Refresh
        public ActionResult Refresh()
        {
            Entities.ConfigGlobal.Refresh();

            Entities.Company.Cache.RefreshCache();
            Entities.Account.Cache.RefreshCache();
            Entities.Role.Cache.RefreshCache();

            return RedirectToAction("Index", "Home");
        }
    }
}