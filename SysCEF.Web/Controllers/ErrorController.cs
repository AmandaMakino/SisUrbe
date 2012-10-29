using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Core;
using SysCEF.Common.Interface;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using SysCEF.Web.Models;
using System.Text;
using System.Security.Cryptography;

namespace SysCEF.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
