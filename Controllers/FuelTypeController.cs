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
    public class FuelTypeController : Controller
    {
        private FuelTypeRepository _fuelTypeRepository;

        public FuelTypeController()
        {
            _fuelTypeRepository = new FuelTypeRepository(new FuelTypeContext());
        }

        // GET: FuelType
        public ActionResult Index()
        {
            return View(_fuelTypeRepository.GetFuelTypes());
        }

        // GET: FuelType/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
            if (fuelType == null)
            {
                return HttpNotFound();
            }
            return View(fuelType);
        }

        // GET: FuelType/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value")] FuelType fuelType)
        {
            if (ModelState.IsValid)
            {
                _fuelTypeRepository.Insert(fuelType);
                _fuelTypeRepository.Save();
                return RedirectToAction("Index");
            }

            return View(fuelType);
        }

        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
            if (fuelType == null)
            {
                return HttpNotFound();
            }
            return View(fuelType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value")] FuelType fuelType)
        {
            if (ModelState.IsValid)
            {
                _fuelTypeRepository.Update(fuelType);
                _fuelTypeRepository.Save();
                return RedirectToAction("Index");
            }
            return View(fuelType);
        }

        // GET: FuelType/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
            if (fuelType == null)
            {
                return HttpNotFound();
            }
            return View(fuelType);
        }

        // POST: FuelType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
            _fuelTypeRepository.Delete(fuelType);
            _fuelTypeRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fuelTypeRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
