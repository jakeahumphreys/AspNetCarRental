using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebAssignment1.Customisations;

namespace MVCWebAssignment1.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Unauthorized
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error(ErrorType errorType)
        {
            ViewBag.ErrorType = errorType;
            return View();
        }
    }
}