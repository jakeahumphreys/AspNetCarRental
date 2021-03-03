using System;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Common;
using EIRLSSAssignment1.ServiceLayer;

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class FuelTypeController : Controller
    {
        private FuelTypeService _fuelTypeService;

        public FuelTypeController()
        {
            _fuelTypeService = new FuelTypeService();
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(_fuelTypeService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (FuelTypeNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value")] FuelType fuelType)
        {
            ServiceResponse response = _fuelTypeService.CreateAction(fuelType);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return View(fuelType);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(_fuelTypeService.EditView(id));
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
        public ActionResult Edit([Bind(Include = "Id,Value,IsInactive")] FuelType fuelType)
        {
            ServiceResponse response = _fuelTypeService.EditAction(fuelType);

            if (response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
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
                return View(_fuelTypeService.DeleteView(id));
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
            ServiceResponse response = _fuelTypeService.DeleteAction(id);

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
                _fuelTypeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
