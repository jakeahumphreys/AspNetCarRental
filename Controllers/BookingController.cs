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
using EIRLSSAssignment1.RepeatLogic;
using Microsoft.AspNet.Identity;

namespace EIRLSSAssignment1.Controllers
{
    public class BookingController : Controller
    {
        private BookingRepository _bookingRepository;
        private VehicleRepository _vehicleRepository;
        private OptionalExtraRepository _optionalExtraRepository;
        private Library _library;

        public BookingController()
        {
            _bookingRepository = new BookingRepository(new BookingContext());
            _vehicleRepository = new VehicleRepository(new VehicleContext());
            _optionalExtraRepository = new OptionalExtraRepository(new OptionalExtraContext());
            _library = new Library();
        }

        // GET: Bookings
        public ActionResult Index()
        {
            return View(_bookingRepository.GetBookings());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = _bookingRepository.GetBookingById(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            var bookingVM = new BookingViewModel { booking = booking };

            List<OptionalExtra> bookedOptionalExtras = _optionalExtraRepository.GetOptionalExtras().Where(o => o.bookingId == id).ToList();

            if(bookedOptionalExtras != null)
            {
                bookingVM.BookedOptionalExtras = bookedOptionalExtras;
            }
            
            return View(bookingVM);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            //Handle preventing users from creating bookings based on conditions
            var userId = User.Identity.GetUserId();
            //Check if garage is allowing orders
            if (_library.IsGarageAllowingOrders())
            {
                //Check if user is blacklisted
                if(!_library.IsUserBlacklisted(userId))
                {
                    //Record user age for retrieving the correct vehicles
                    var UserAge = _library.CalculateUserAge(userId);

                    var availableVehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.IsRented == false).Where(x => x.MinimumAgeToRent <= UserAge), "Id", "DisplayString");
                    var availableExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.bookingId == null), "Id", "DisplayString");
                    ViewBag.VehicleId = availableVehicles;
                    ViewBag.OptionalExtras = availableExtras;
                    ViewBag.AvailableExtraCount = availableExtras.Count();
                    ViewBag.AvailableVehicles = availableVehicles.Count();
                    return View();
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
               
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }

          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingViewModel bookingVM)
        {
            //Logic to prevent bookings based on conditions of garage

            if (ModelState.IsValid)
            {
                _bookingRepository.Insert(bookingVM.booking);
                _bookingRepository.Save();

                if (bookingVM.SelectedExtraIds != null)
                {
                    foreach (var id in bookingVM.SelectedExtraIds)
                    {
                        OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);

                        if (optionalExtra != null)
                        {
                            optionalExtra.bookingId = bookingVM.booking.Id;
                            _optionalExtraRepository.Update(optionalExtra);
                            _optionalExtraRepository.Save();
                        }
                    }
                }

                var vehicleToUpdate = _vehicleRepository.GetVehicleById(bookingVM.booking.VehicleId);

                if(vehicleToUpdate != null)
                {
                    vehicleToUpdate.IsRented = true;
                    _vehicleRepository.Update(vehicleToUpdate);
                    _vehicleRepository.Save();
                }

                return RedirectToAction("Index");
            }

            var availableVehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.IsRented == false), "Id", "DisplayString");
            var availableExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.bookingId == null), "Id", "DisplayString");
            ViewBag.VehicleId = availableVehicles;
            ViewBag.OptionalExtras = availableExtras;
            ViewBag.AvailableExtraCount = availableExtras.Count();
            ViewBag.BookedExtraCount = availableExtras.Count();

            return View(bookingVM);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = _bookingRepository.GetBookingById(id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            var availableVehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.IsRented == false), "Id", "DisplayString");
            var availableExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.bookingId == null), "Id", "DisplayString");
            var bookedExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.bookingId == id), "Id", "DisplayString");
            ViewBag.AvailableExtraCount = availableExtras.Count();
            ViewBag.BookedExtraCount = bookedExtras.Count();
            ViewBag.VehicleId = availableVehicles;
            ViewBag.OptionalExtras = availableExtras;
            ViewBag.BookedOptionalExtras = bookedExtras;

            var bookingVM = new BookingViewModel { booking = booking};

            ViewBag.BookedVehicle = bookingVM.booking.Vehicle.DisplayString;

            return View(bookingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookingViewModel bookingVM)
        {
            if (ModelState.IsValid)
            {
                _bookingRepository.Update(bookingVM.booking);
                _bookingRepository.Save();

                if (bookingVM.SelectedExtraIds != null)
                {
                    foreach (var id in bookingVM.SelectedExtraIds)
                    {
                        OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);

                        if (optionalExtra != null)
                        {
                            optionalExtra.bookingId = bookingVM.booking.Id;
                            _optionalExtraRepository.Update(optionalExtra);
                            _optionalExtraRepository.Save();
                        }
                    }
                }

                //Remove selected optional extras attributed to this booking
                if(bookingVM.SelectedExtraToRemoveIds !=null)
                {
                    foreach(var id in bookingVM.SelectedExtraToRemoveIds)
                    {
                        OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);

                        if (optionalExtra != null)
                        {
                            optionalExtra.bookingId = null;
                            _optionalExtraRepository.Update(optionalExtra);
                            _optionalExtraRepository.Save();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.VehicleId = new SelectList(_vehicleRepository.GetVehicles(), "Id", "DisplayString");
            ViewBag.OptionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.bookingId == null), "Id", "DisplayString");
            return View(bookingVM);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = _bookingRepository.GetBookingById(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = _bookingRepository.GetBookingById(id);
            _bookingRepository.Delete(booking);
            _bookingRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bookingRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
