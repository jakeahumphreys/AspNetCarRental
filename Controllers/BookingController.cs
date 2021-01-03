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
using EIRLSSAssignment1.RepeatLogic.Objects;
using Microsoft.AspNet.Identity;

namespace EIRLSSAssignment1.Controllers
{
    public class BookingController : Controller
    {
        private BookingRepository _bookingRepository;
        private VehicleRepository _vehicleRepository;
        private OptionalExtraRepository _optionalExtraRepository;
        private ApplicationDbContext appDbContext = new ApplicationDbContext();
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

            List<OptionalExtra> bookedOptionalExtras = booking.OptionalExtras.ToList();

            if (bookedOptionalExtras != null)
            {
                bookingVM.BookedOptionalExtras = bookedOptionalExtras;
            }

            return View(bookingVM);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            //Handle preventing users from creating bookings based on conditions
            if (_library.IsGarageAllowingOrders())
            {
                var userId = User.Identity.GetUserId();

                if (!_library.IsUserBlacklisted(userId))
                {
                    var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age

                    var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge), "Id", "DisplayString");

                    var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras(), "Id", "DisplayString");

                    //Set Viewbag Vehicle Data
                    ViewBag.Vehicles = vehicles;
                    ViewBag.VehicleCount = vehicles.Count();

                    //Set Viewbag Optional Extra Data
                    ViewBag.OptionalExtras = optionalExtras;
                    ViewBag.OptionalExtraCount = optionalExtras.Count();

                    return View();
                }
                else
                {
                    //user is blacklisted
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
               
            }
            else
            {
                //Garage is not open for orders
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingViewModel bookingVM)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                List<Booking> conflictingBookings = CheckConflictingBookings(bookingVM.booking);

                if (conflictingBookings != null && conflictingBookings.Count > 0)
                {
                    //There are conflicting bookings
                    ViewBag.ConflictingBookings = conflictingBookings;
                }
                else
                {
                    if (bookingVM.SelectedExtraIds != null)
                    {
                        List<ConflictingExtraItem> conflictingOptionalExtras = CheckConflictingOptionalExtras(bookingVM);

                        if (conflictingOptionalExtras != null && conflictingOptionalExtras.Count > 0)
                        {
                            //There are conflicting optional extras
                            ViewBag.ConflictingOptionalExtras = conflictingOptionalExtras;
                        }
                        else
                        {

                            var bookedOptionalExtras = new List<OptionalExtra>();

                            foreach(var id in bookingVM.SelectedExtraIds)
                            {
                                //HERE
                                bookedOptionalExtras.Add(_optionalExtraRepository.GetOptionalExtraById(id));
                            }
                           
                            bookingVM.booking.UserId = User.Identity.GetUserId();
                            bookingVM.booking.OptionalExtras = bookedOptionalExtras;

                            //appDbContext.Bookings.Add(bookingVM.booking);
                            //appDbContext.SaveChanges();

                            _bookingRepository.Insert(bookingVM.booking);
                            _bookingRepository.Save();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        //User has not selected optional extras
                        bookingVM.booking.UserId = User.Identity.GetUserId(); //Attribute booking to user
                        _bookingRepository.Insert(bookingVM.booking);
                        _bookingRepository.Save();
                        return RedirectToAction("Index");
                    }
                } 
            }
            else
            {
                

                return View(bookingVM);
            }

            var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age

            var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge), "Id", "DisplayString");

            var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras(), "Id", "DisplayString");

            //Set Viewbag Vehicle Data
            ViewBag.Vehicles = vehicles;
            ViewBag.VehicleCount = vehicles.Count();

            //Set Viewbag Optional Extra Data
            ViewBag.OptionalExtras = optionalExtras;
            ViewBag.OptionalExtraCount = optionalExtras.Count();

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

            var userId = User.Identity.GetUserId();
            var UserAge = _library.CalculateUserAge(userId);

            var availableVehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= UserAge), "Id", "DisplayString");
            var availableExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras(), "Id", "DisplayString");
            var bookedExtras = new MultiSelectList(_bookingRepository.GetBookingById(id).OptionalExtras);
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

                if (bookingVM.SelectedExtraIds != null)
                {
                    foreach (var id in bookingVM.SelectedExtraIds)
                    {
                        OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);

                        if (optionalExtra != null)
                        {
                            bookingVM.booking.OptionalExtras.Add(optionalExtra);
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
                            bookingVM.booking.OptionalExtras.Remove(optionalExtra);
                        }
                    }
                }

                _bookingRepository.Update(bookingVM.booking);
                _bookingRepository.Save();

                return RedirectToAction("Index");
            }
            ViewBag.VehicleId = new SelectList(_vehicleRepository.GetVehicles(), "Id", "DisplayString");
            ViewBag.OptionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras(), "Id", "DisplayString");
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

        private List<Booking> CheckConflictingBookings(Booking booking)
        {
            if(booking != null)
            {
                DateRange bookingDateRange = new DateRange(booking.BookingStart, booking.BookingFinish);
                List<Booking> conflictingBookings = new List<Booking>();
                //List<DateStatus> dateStatuses = new List<DateStatus>();

                foreach (var existingBooking in _bookingRepository.GetBookings().Where(x => x.VehicleId == booking.VehicleId).ToList())
                {
                    if (existingBooking != null)
                    {
                        bool startDateOk = bookingDateRange.Includes(existingBooking.BookingStart);
                        bool endDateOk = bookingDateRange.Includes(existingBooking.BookingFinish);

                        if (startDateOk == false || endDateOk == false)
                        {
                            conflictingBookings.Add(existingBooking);
                        }
                    }
                }

                return conflictingBookings;
            }
            else
            {
                return null;
            }
            
            
        }

        private List<ConflictingExtraItem> CheckConflictingOptionalExtras(BookingViewModel bookingVM)
        {
            if(bookingVM != null)
            {
                DateRange bookingDateRange = new DateRange(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish);
                List<Booking> conlictingBookingsWithExtra = new List<Booking>();
                List<ConflictingExtraItem> conflictingExtras = new List<ConflictingExtraItem>();

                foreach (var extraId in bookingVM.SelectedExtraIds)
                {
                    OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(extraId);
                    List<Booking> bookingsWithSelectedExtra = _bookingRepository.GetBookings().Where(x => x.OptionalExtras.Contains(optionalExtra)).ToList();

                    foreach(var bookingWithSelectedExtra in bookingsWithSelectedExtra)
                    {
                        bool startDateOk = bookingDateRange.Includes(bookingWithSelectedExtra.BookingStart);
                        bool endDateOk = bookingDateRange.Includes(bookingWithSelectedExtra.BookingFinish);

                        if (startDateOk == false || endDateOk == false)
                        {
                            conflictingExtras.Add(new ConflictingExtraItem {
                                StartDate = bookingWithSelectedExtra.BookingStart,
                                EndDate = bookingWithSelectedExtra.BookingFinish,
                                OptionalExtra = optionalExtra
                            });
                        }
                    }
                }

                return conflictingExtras;
            }
            else
            {
                return null;
            }
        }
    }
}
