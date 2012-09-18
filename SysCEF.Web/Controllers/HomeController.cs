using System.Web.Mvc;
using SysCEF.Common.Implementacao;

namespace SysCEF.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (new UsuarioLogado().Usuario == null)
                return RedirectToAction("Index", "Login");

            return View();
        }
    }
}
