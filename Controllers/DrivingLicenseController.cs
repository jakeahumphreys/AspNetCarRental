using System;
using System.Web.Mvc;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Customisations;
using EIRLSSAssignment1.ServiceLayer;

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class DrivingLicenseController : Controller
    {
        private DrivingLicenseService _drivingLicenseService;

        public DrivingLicenseController()
        {
            _drivingLicenseService = new DrivingLicenseService();
        }

        public ActionResult Index()
        {
            return View(_drivingLicenseService.GetIndex());
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(_drivingLicenseService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (DrivingLicenseNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            return View(_drivingLicenseService.CreateView());            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DrivingLicenseViewModel drivingLicenseVM)
        {
            ServiceResponse response = _drivingLicenseService.CreationAction(drivingLicenseVM);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(_drivingLicenseService.EditView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (BookingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DrivingLicenseViewModel drivingLicenseVM)
        {
            ServiceResponse response = _drivingLicenseService.EditAction(drivingLicenseVM);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                return View(_drivingLicenseService.DeleteView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (BookingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceResponse response = _drivingLicenseService.DeleteAction(id);

            if (response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _drivingLicenseService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
