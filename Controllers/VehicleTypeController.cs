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

namespace EIRLSSAssignment1.Controllers
{
    public class VehicleTypeController : Controller
    {
        private VehicleTypeRepository _vehicleTypeRepository;

        public VehicleTypeController()
        {
            _vehicleTypeRepository = new VehicleTypeRepository(new ApplicationDbContext());
        }

        // GET: VehicleTypes
        public ActionResult Index()
        {
            return View(_vehicleTypeRepository.GetVehicleTypes());
        }

        // GET: VehicleTypes/Details/5
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
            if (ModelState.IsValid)
            {
                _vehicleTypeRepository.Insert(vehicleType);
                _vehicleTypeRepository.Save();
                return RedirectToAction("Index", "Admin", null);
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value,IsInactive")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                _vehicleTypeRepository.Update(vehicleType);
                _vehicleTypeRepository.Save();
                return RedirectToAction("Index", "Admin", null);
            }
            return View(vehicleType);
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleType vehicleType = _vehicleTypeRepository.GetVehicleTypeById(id);
            _vehicleTypeRepository.Delete(vehicleType);
            _vehicleTypeRepository.Save();
            return RedirectToAction("Index", "Admin", null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vehicleTypeRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
