using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.RepeatLogic;
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
        private BookingRepository _bookingRepository;
        private Library _library;

        public HomeController()
        {
            _appDbContext = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
            _library = new Library();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = _appDbContext.Users.Find(userId);
            if(user != null)
            {
                ViewBag.hasStoredLicense = _library.userHasStoredLicense(userId);
                ViewBag.hasStoredDocument = _library.userHasStoredDocument(userId);
                ViewBag.User = user;
                ViewBag.FirstName = user.Name.Substring(0, user.Name.IndexOf(" "));
            }

            List<Booking> userBookings = _bookingRepository.GetBookings().Where(b => b.UserId == userId).ToList();

            ViewBag.UserBookingsCount = userBookings.Count();
            return View(userBookings);
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