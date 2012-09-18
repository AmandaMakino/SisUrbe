using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysCEF.Web.Controllers
{
    public class ImovelController : Controller
    {
        public ActionResult Consulta()
        {
            return PartialView();
        }

        public ActionResult Cadastro()
        {
            return PartialView();
        }
    }
}
