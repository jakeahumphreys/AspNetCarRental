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
    public class OptionalExtraController : Controller
    {
        private OptionalExtraRepository _optionalExtraRepository;

        public OptionalExtraController()
        {
            _optionalExtraRepository = new OptionalExtraRepository(new ApplicationDbContext());
        }

        // GET: OptionalExtras
        public ActionResult Index()
        { 
            return View(_optionalExtraRepository.GetOptionalExtras());
        }

        // GET: OptionalExtras/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
            if (optionalExtra == null)
            {
                return HttpNotFound();
            }
            return View(optionalExtra);
        }

        // GET: OptionalExtras/Create
        public ActionResult Create()
        {
            //ViewBag.bookingId = new SelectList(db.Bookings, "Id", "Remarks");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,serialNumber,Remarks")] OptionalExtra optionalExtra)
        {
            if (ModelState.IsValid)
            {
                _optionalExtraRepository.Insert(optionalExtra);
                _optionalExtraRepository.Save();
                return RedirectToAction("Index", "Admin", null);
            }

            //ViewBag.bookingId = new SelectList(db.Bookings, "Id", "Remarks", optionalExtra.bookingId);
            return View(optionalExtra);
        }

        // GET: OptionalExtras/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
            if (optionalExtra == null)
            {
                return HttpNotFound();
            }
            //ViewBag.bookingId = new SelectList(db.Bookings, "Id", "Remarks", optionalExtra.bookingId);
            return View(optionalExtra);
        }

        // POST: OptionalExtras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,serialNumber,Remarks")] OptionalExtra optionalExtra)
        {
            if (ModelState.IsValid)
            {
                _optionalExtraRepository.Update(optionalExtra);
                _optionalExtraRepository.Save();
                return RedirectToAction("Index", "Admin", null);
            }
            //ViewBag.bookingId = new SelectList(db.Bookings, "Id", "Remarks", optionalExtra.bookingId);
            return View(optionalExtra);
        }

        // GET: OptionalExtras/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
            if (optionalExtra == null)
            {
                return HttpNotFound();
            }
            return View(optionalExtra);
        }

        // POST: OptionalExtras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
            _optionalExtraRepository.Delete(optionalExtra);
            _optionalExtraRepository.Save();
            return RedirectToAction("Index", "Admin", null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _optionalExtraRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
