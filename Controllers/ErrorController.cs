using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebAssignment1.Customisations;

namespace MVCWebAssignment1.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Unauthorized
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error(ErrorType errorType, string message)
        {
            ViewBag.ErrorType = errorType;
            ViewBag.Message = message;
            return View();
        }
    }
}