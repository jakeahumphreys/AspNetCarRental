using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Common;
using EIRLSSAssignment1.ServiceLayer;
using EIRLSSAssignment1.Models.ViewModels;

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class VehicleController : Controller
    {
        private VehicleService _vehicleService;

        public VehicleController()
        {
            _vehicleService = new VehicleService();
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(_vehicleService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (VehicleNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            return View(_vehicleService.CreateView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleViewModel vehicleVM)
        {

            ServiceResponse response = _vehicleService.CreateAction(vehicleVM);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);

            }
            else
            {
                return View(response.ServiceObject as VehicleViewModel);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(_vehicleService.EditView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (VehicleNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleViewModel vehicleVM)
        {
            ServiceResponse response = _vehicleService.EditAction(vehicleVM);

            if(response.Result == true)
            {
                return RedirectToAction("Details", "Vehicle", new { Id = vehicleVM.vehicle.Id });
            }
            else
            {
                return View(vehicleVM);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                return View(_vehicleService.DeleteView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (VehicleNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceResponse response = _vehicleService.DeleteAction(id);

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
                _vehicleService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
