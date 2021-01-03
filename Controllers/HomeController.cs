using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext _appDbContext;

        public HomeController()
        {
            _appDbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            ApplicationUser user = _appDbContext.Users.Find(User.Identity.GetUserId());
            ViewBag.User = user;
            ViewBag.FirstName = user.Name.Substring(0, user.Name.IndexOf(" "));
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}