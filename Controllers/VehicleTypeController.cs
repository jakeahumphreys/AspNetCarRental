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

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class VehicleTypeController : Controller
    {
        private VehicleTypeRepository _vehicleTypeRepository;
        private VehicleTypeService _vehicleTypeService;

        public VehicleTypeController()
        {
            _vehicleTypeRepository = new VehicleTypeRepository(new ApplicationDbContext());
            _vehicleTypeService = new VehicleTypeService();
        }

        public ActionResult Details(int id)
        {
            if (id ==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = _vehicleTypeRepository.GetVehicleTypeById(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value")] VehicleType vehicleType)
        {
            ServiceResponse response = _vehicleTypeService.CreateAction(vehicleType);

            if (response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return View(vehicleType);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(_vehicleTypeService.EditView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (VehicleTypeNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value,IsInactive")] VehicleType vehicleType)
        {
            ServiceResponse response = _vehicleTypeService.EditAction(vehicleType);

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
                return View(_vehicleTypeService.DeleteView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (OptionalExtraNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceResponse response = _vehicleTypeService.DeleteAction(id);

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
                _vehicleTypeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
