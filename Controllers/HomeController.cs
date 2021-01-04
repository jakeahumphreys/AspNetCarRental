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
        private VehicleRepository _vehicleRepository;
        private Library _library;

        public HomeController()
        {
            _appDbContext = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
            _vehicleRepository = new VehicleRepository(new ApplicationDbContext());
            _library = new Library();
        }


        public ActionResult Index(DateTime? searchParamDateStart, DateTime? searchParamDateEnd, string searchParamVehicle, string searchParamLateReturn)
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = _appDbContext.Users.Find(userId);
            if(user != null)
            {
                var userHasLicense =  _library.userHasStoredLicense(userId);
                var userHasDocument = _library.userHasStoredDocument(userId);

                ViewBag.hasStoredLicense = userHasLicense;
                ViewBag.hasStoredDocument = userHasDocument;

                if(userHasLicense == true)
                {
                    ViewBag.licenseId = user.DrivingLicenseId;
                }
                
                if(userHasDocument == true)
                {
                    ViewBag.licenseId = user.SupportingDocumentId;
                }
                

                ViewBag.User = user;
                ViewBag.FirstName = user.Name.Substring(0, user.Name.IndexOf(" "));
            }

            //Get all vehicles for searchlist
            SelectList vehicles = new SelectList(_vehicleRepository.GetVehicles().ToList(), "DisplayString", "DisplayString");
            ViewBag.vehicles = vehicles;
            

            List<Booking> userBookings = _bookingRepository.GetBookings().Where(b => b.UserId == userId).ToList();

            ViewBag.UserBookingsCount = userBookings.Count();


            if (searchParamDateStart != null && searchParamDateEnd !=null)
            {
                userBookings = userBookings.Where(b => b.BookingStart >= searchParamDateStart && b.BookingFinish <= searchParamDateEnd).ToList();
            }

            if(searchParamLateReturn != null)
            {
                if(searchParamLateReturn == "Yes")
                {
                    userBookings = userBookings.Where(b => b.IsLateReturn == true).ToList();
                }
                else
                {
                    userBookings = userBookings.Where(b => b.IsLateReturn == false).ToList();
                }
            }

            if(searchParamVehicle != null && searchParamVehicle != "")
            {
                userBookings = userBookings.Where(b => b.Vehicle.DisplayString == searchParamVehicle).ToList();
            }

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