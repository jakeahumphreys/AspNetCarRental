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
    public class VehicleController : Controller
    {
        private VehicleRepository _vehicleRepository;
        private FuelTypeRepository _fuelTypeRepository;
        private VehicleTypeRepository _vehicleTypeRepository;

        public VehicleController()
        {
            _vehicleRepository = new VehicleRepository(new VehicleContext());
            _fuelTypeRepository = new FuelTypeRepository(new FuelTypeContext());
            _vehicleTypeRepository = new VehicleTypeRepository(new VehicleTypeContext());
        }

        public ActionResult Index()
        {
            return View(_vehicleRepository.GetVehicles());
        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicle/Create
        public ActionResult Create()
        {
            ViewBag.FuelTypeId = new SelectList(_fuelTypeRepository.GetFuelTypes(), "Id", "Value");
            ViewBag.VehicleTypeId = new SelectList(_vehicleTypeRepository.GetVehicleTypes(), "Id", "Value");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VRM,VIN,VehicleTypeId,FuelTypeId,Make,Model,EngineSize,Transmission,RentalCost,MinimumAgeToRent,Remarks")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _vehicleRepository.Insert(vehicle);
                _vehicleRepository.Save();

                return RedirectToAction("Index");
            }

            ViewBag.FuelTypeId = new SelectList(_fuelTypeRepository.GetFuelTypes(), "Id", "Value", vehicle.FuelTypeId);
            ViewBag.VehicleTypeId = new SelectList(_vehicleTypeRepository.GetVehicleTypes(), "Id", "Value", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.FuelTypeId = new SelectList(_fuelTypeRepository.GetFuelTypes(), "Id", "Value", vehicle.FuelTypeId);
            ViewBag.VehicleTypeId = new SelectList(_vehicleTypeRepository.GetVehicleTypes(), "Id", "Value", vehicle.VehicleTypeId);
            return View(vehicle);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VRM,VIN,VehicleTypeId,FuelTypeId,Make,Model,EngineSize,Transmission,RentalCost,MinimumAgeToRent,Remarks")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _vehicleRepository.Update(vehicle);
                _vehicleRepository.Save();

                return RedirectToAction("Index");
            }
            ViewBag.FuelTypeId = new SelectList(_fuelTypeRepository.GetFuelTypes(), "Id", "Value", vehicle.FuelTypeId);
            ViewBag.VehicleTypeId = new SelectList(_vehicleTypeRepository.GetVehicleTypes(), "Id", "Value", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
            _vehicleRepository.Delete(vehicle);
            _vehicleRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vehicleRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
