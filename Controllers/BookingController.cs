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
using MVCWebAssignment1.Customisations;

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
            ApplicationDbContext context = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(context);
            _vehicleRepository = new VehicleRepository(context);
            _optionalExtraRepository = new OptionalExtraRepository(context);
            
            _library = new Library();
        }

        // GET: Bookings
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                return View(_bookingRepository.GetBookings());
            }
            else
            {
                return View(_bookingRepository.GetBookings().Where(x => x.UserId == userId));
            }
        }

        // GET: Bookings/Details/5
        [CustomAuthorize(Roles = "User,Admin")]
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
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Create()
        {
            //Handle preventing users from creating bookings based on conditions
            if (_library.IsGarageAllowingOrders())
            {
                var userId = User.Identity.GetUserId();
                //if autotrust is enabled, send userid to be judged for promotion.
                _library.HandleAutoTrust(userId);

                ViewBag.IsUserTrusted = _library.CanUserReturnLate(userId);

                if (!_library.IsUserBlacklisted(userId))
                {
                    var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age

                    var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge).Where(x => x.IsInactive == false), "Id", "DisplayString");

                    var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.IsInactive == false), "Id", "DisplayString");

                    //Set Viewbag Vehicle Data
                    ViewBag.Vehicles = vehicles;
                    ViewBag.VehicleCount = vehicles.Count();

                    //Set Viewbag Optional Extra Data
                    ViewBag.OptionalExtras = optionalExtras;
                    ViewBag.OptionalExtraCount = optionalExtras.Count();

                    ViewBag.Users = new SelectList(appDbContext.Users.ToList(), "Id", "Name");

                    return View();
                }
                else
                {
                    //user is blacklisted
                    return RedirectToAction("Error", "Error", new { @errorType = ErrorType.Account });
                }

            }
            else
            {
                //Garage is not open for orders
                return RedirectToAction("Error", "Error", new {@errorType = ErrorType.GarageClosed});
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Create(BookingViewModel bookingVM)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                //Set variables for re-display of viewModel

                var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age
                var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge).Where(x => x.IsInactive == false), "Id", "DisplayString");
                var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.IsInactive == false), "Id", "DisplayString");

                //Set Viewbag Vehicle Data
                ViewBag.Vehicles = vehicles;
                ViewBag.VehicleCount = vehicles.Count();

                //Set Viewbag Optional Extra Data
                ViewBag.OptionalExtras = optionalExtras;
                ViewBag.OptionalExtraCount = optionalExtras.Count();

                ViewBag.IsUserTrusted = _library.CanUserReturnLate(userId);

                DateTime startTime = new DateTime(bookingVM.StartDate.Year, bookingVM.StartDate.Month, bookingVM.StartDate.Day, bookingVM.StartDateTime.Hour, bookingVM.StartDateTime.Minute, bookingVM.StartDateTime.Second);
                DateTime endTime = new DateTime(bookingVM.EndDate.Year, bookingVM.EndDate.Month, bookingVM.EndDate.Day, bookingVM.EndDateTime.Hour, bookingVM.EndDateTime.Minute, bookingVM.EndDateTime.Second);

                bookingVM.booking.BookingStart = startTime;
                bookingVM.booking.BookingFinish = endTime;

                CreateBookingErrorObj errorObj = new CreateBookingErrorObj();

                if(startTime > endTime)
                {
                    errorObj.isStartAfterEnd = true;
                    ViewBag.ErrorObj = errorObj;
                    return View(bookingVM);
                }

                if(endTime < startTime)
                {
                    errorObj.isEndBeforeStart = true;
                    ViewBag.ErrorObj = errorObj;
                    return View(bookingVM);
                }

                if(!User.IsInRole("Admin"))
                {
                    if (startTime < DateTime.Now)
                    {
                        errorObj.isInThePast = true;
                        ViewBag.ErrorObj = errorObj;
                        return View(bookingVM);
                    }
                }
                


                if (_library.isBeforeOpening(startTime.Hour) == true)
                {
                    errorObj.isStartBeforeOpen = true;
                    ViewBag.ErrorObj = errorObj;
                    return View(bookingVM);
                }

                if(_library.isBeforeOpening(endTime.Hour) == true)
                {
                    errorObj.isEndBeforeOpen = true;
                    ViewBag.ErrorObj = errorObj;
                    return View(bookingVM);
                }

                if(_library.isBeyondClosing(startTime.Hour) ==true)
                {
                    errorObj.isStartAfterClose = true;
                    ViewBag.ErrorObj = errorObj;
                    return View(bookingVM);
                }

                if(_library.isBeyondClosing(endTime.Hour) == true) 
                {
                    if(bookingVM.booking.IsLateReturn == false)
                    {
                        errorObj.isEndAfterClose = true;
                        ViewBag.ErrorObj = errorObj;
                        return View(bookingVM);
                    }
                }

                if (_library.isMinRental(startTime, endTime) == false)
                {
                    errorObj.isBelowMinRental = true;
                    ViewBag.ErrorObj = errorObj;
                    return View(bookingVM);
                }

                if (_library.isMaxRental(startTime, endTime) == true)
                {
                    errorObj.isBeyondMaxRental = true;
                    ViewBag.ErrorObj = errorObj;
                    return View(bookingVM);
                }

                List<Booking> conflictingBookings = CheckConflictingBookings(bookingVM.booking);

                if (conflictingBookings != null && conflictingBookings.Count > 0)
                {
                    //There are conflicting bookings
                    errorObj.conflictingBookings = conflictingBookings;
                    ViewBag.ErrorObj = errorObj;
                }
                else
                {
                    if (bookingVM.SelectedExtraIds != null)
                    {
                        List<ConflictingExtraItem> conflictingOptionalExtras = CheckConflictingOptionalExtras(bookingVM);

                        if (conflictingOptionalExtras != null && conflictingOptionalExtras.Count > 0)
                        {
                            //There are conflicting optional extras
                            errorObj.conflictingOptionalExtras = conflictingOptionalExtras;
                            ViewBag.ErrorObj = errorObj;
                        }
                        else
                        {

                            var bookedOptionalExtras = new List<OptionalExtra>();

                            foreach (var id in bookingVM.SelectedExtraIds)
                            {
                                bookedOptionalExtras.Add(_optionalExtraRepository.GetOptionalExtraById(id));
                            }

                            if(!User.IsInRole("Admin"))
                            {
                                bookingVM.booking.UserId = User.Identity.GetUserId();
                            }
              

                            bookingVM.booking.OptionalExtras = bookedOptionalExtras;

                            bookingVM.booking.BookingCost = CalculateBookingCost(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish, bookingVM.booking.VehicleId);
                            _bookingRepository.Insert(bookingVM.booking);
                            _bookingRepository.Save();
                            if (User.IsInRole("Admin"))
                            {
                                return RedirectToAction("Index", "Admin", null);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", null);
                            }
                        }
                    }
                    else
                    {
                        //User has not selected optional extras
                        if(!User.IsInRole("Admin"))
                        {
                            bookingVM.booking.UserId = User.Identity.GetUserId();
                        }

                        bookingVM.booking.BookingCost = CalculateBookingCost(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish, bookingVM.booking.VehicleId);
                         //Attribute booking to user
                        _bookingRepository.Insert(bookingVM.booking);
                        _bookingRepository.Save();
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index", "Admin", null);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", null);
                        }
                    }
                }
            }
            else
            {
                return View(bookingVM);
            }

            return View(bookingVM);
        }


        // GET: Bookings/Edit/5
        [CustomAuthorize(Roles = "User,Admin")]
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
            var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age

            //If user is trusted show the late return box
            if(_library.CanUserReturnLate(userId))
            {
                ViewBag.IsTrustedUser = true;
            }

            var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge).Where(x => x.IsInactive == false), "Id", "DisplayString");
            var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.Bookings.Contains(booking) == false).Where(x => x.IsInactive == false), "Id", "DisplayString");
            var bookedOptionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.Bookings.Contains(booking)).ToList(), "Id", "DisplayString");


            //Set Viewbag Vehicle Data
            ViewBag.Vehicles = vehicles;
            ViewBag.VehicleCount = vehicles.Count();

            //Set Viewbag Optional Extra Data
            ViewBag.OptionalExtras = optionalExtras;
            ViewBag.OptionalExtraCount = optionalExtras.Count();

            ViewBag.BookedOptionalExtras = bookedOptionalExtras;
            ViewBag.BookedOptionalExtracount = bookedOptionalExtras.Count();


            ViewBag.BookedVehicle = booking.Vehicle.DisplayString;

            var bookingVM = new BookingViewModel { booking = booking, StartDate = booking.BookingStart, StartDateTime = booking.BookingStart, EndDate = booking.BookingFinish, EndDateTime = booking.BookingFinish };

            return View(bookingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Edit(BookingViewModel bookingVM)
        {
            if (ModelState.IsValid)
            {
                DateTime StartTime = new DateTime(bookingVM.StartDate.Year, bookingVM.StartDate.Month, bookingVM.StartDate.Day, bookingVM.StartDateTime.Hour, bookingVM.StartDateTime.Minute, bookingVM.StartDateTime.Second);
                DateTime EndTime = new DateTime(bookingVM.EndDate.Year, bookingVM.EndDate.Month, bookingVM.EndDate.Day, bookingVM.EndDateTime.Hour, bookingVM.EndDateTime.Minute, bookingVM.EndDateTime.Second);

                bookingVM.booking.BookingStart = StartTime;
                bookingVM.booking.BookingFinish = EndTime;
               
                Booking existingBooking = _bookingRepository.GetBookingById(bookingVM.booking.Id);

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
                        foreach(var extraId in bookingVM.SelectedExtraIds)
                        {
                            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(extraId);
                            existingBooking.OptionalExtras.Add(optionalExtra);
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
                            existingBooking.OptionalExtras.Remove(optionalExtra);
                        }
                    }
                }

                existingBooking.BookingStart = bookingVM.booking.BookingStart;
                existingBooking.BookingFinish = bookingVM.booking.BookingFinish;
                existingBooking.IsLateReturn = bookingVM.booking.IsLateReturn;
                existingBooking.IsReturned = bookingVM.booking.IsReturned;
                existingBooking.ReturnDate = bookingVM.booking.ReturnDate;
                existingBooking.Remarks = bookingVM.booking.Remarks;

                if (existingBooking.BookingCost == 0)
                {
                    existingBooking.BookingCost = CalculateBookingCost(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish, bookingVM.booking.VehicleId);
                }

                _bookingRepository.Update(existingBooking);
                _bookingRepository.Save();

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin", null);
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }


            }

            var userId = User.Identity.GetUserId();

            var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age

            var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge).Where(x => x.IsInactive == false), "Id", "DisplayString");

            var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.IsInactive == false), "Id", "DisplayString");

            var bookedOptionalExtras = new MultiSelectList(bookingVM.booking.OptionalExtras.ToList(), "Id", "DisplayString");

            //Set Viewbag Vehicle Data
            ViewBag.Vehicles = vehicles;
            ViewBag.VehicleCount = vehicles.Count();

            //Set Viewbag Optional Extra Data
            ViewBag.OptionalExtras = optionalExtras;
            ViewBag.OptionalExtraCount = optionalExtras.Count();

            ViewBag.BookedOptionalExtras = bookedOptionalExtras;
            ViewBag.BookedOptionalExtracount = bookedOptionalExtras.Count();
            return View(bookingVM);
        }

        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult ExtendBooking(int id)
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

            var bookingVM = new BookingViewModel { booking = booking, StartDate = booking.BookingStart, StartDateTime = booking.BookingStart, EndDate = booking.BookingFinish, EndDateTime = booking.BookingFinish };

            return View(bookingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult ExtendBooking(BookingViewModel bookingVM)
        {
            if (ModelState.IsValid)
            {
                DateTime StartTime = new DateTime(bookingVM.StartDate.Year, bookingVM.StartDate.Month, bookingVM.StartDate.Day, bookingVM.StartDateTime.Hour, bookingVM.StartDateTime.Minute, bookingVM.StartDateTime.Second);
                DateTime EndTime = new DateTime(bookingVM.EndDate.Year, bookingVM.EndDate.Month, bookingVM.EndDate.Day, bookingVM.EndDateTime.Hour, bookingVM.EndDateTime.Minute, bookingVM.EndDateTime.Second);

                bookingVM.booking.BookingStart = StartTime;
                bookingVM.booking.BookingFinish = EndTime;

                Booking existingBooking = _bookingRepository.GetBookingById(bookingVM.booking.Id);

                EditBookingErrorObj errorObj = new EditBookingErrorObj();


                if (bookingVM.booking.BookingFinish != existingBooking.BookingFinish)
                {
                    //User has changed the end date.
                    if (_library.isBookedNextDay(bookingVM.booking.BookingFinish, bookingVM.booking.VehicleId) == true)
                    {
                        if(bookingVM.booking.BookingFinish.Hour <= 16)
                        {
                            //User is reducing end time, which is permitted up until 4pm
                            existingBooking.BookingStart = bookingVM.booking.BookingStart;
                            existingBooking.BookingFinish = bookingVM.booking.BookingFinish;

                            if(existingBooking.BookingCost == 0)
                            {
                                existingBooking.BookingCost = CalculateBookingCost(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish, bookingVM.booking.VehicleId);
                            }

                            _bookingRepository.Update(existingBooking);
                            _bookingRepository.Save();
                            return RedirectToAction("Details", "Booking", new { @id = existingBooking.Id });
                        }
                        else
                        {
                            //User is attempting to extend, where there is a booking the next day.
                            errorObj.isBookedNextDay = true;
                            ViewBag.ErrorObj = errorObj;
                            return View(bookingVM);
                        }
                        
                    }
                    else
                    {
                        if (_library.isBeyondClosing(bookingVM.booking.BookingFinish.Hour) == true)
                        {
                            if (bookingVM.booking.IsLateReturn)
                            {
                                _bookingRepository.Update(existingBooking);
                                _bookingRepository.Save();
                                return RedirectToAction("Details", "Booking", new { @id = existingBooking.Id });
                            }
                            else
                            {
                                errorObj.isBeyondClose = true;
                                ViewBag.ErrorObj = errorObj;
                                return View(bookingVM);
                            }

                        }
                        else
                        {
                            existingBooking.BookingStart = bookingVM.booking.BookingStart;
                            existingBooking.BookingFinish = bookingVM.booking.BookingFinish;

                            _bookingRepository.Update(existingBooking);
                            _bookingRepository.Save();
                            return RedirectToAction("Details", "Booking", new { @id = existingBooking.Id });
                        }

                    }
                }
            }

            return View(bookingVM);
        }

        [CustomAuthorize(Roles = "User,Admin")]
        // GET: Bookings/Delete/5
        public ActionResult Return(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = _bookingRepository.GetBookingById(id);

            if(DateTime.Now > booking.BookingFinish)
            {
                ViewBag.dateStatus = "late";
            }

            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult ReturnConfirmed(int id)
        {
            Booking booking = _bookingRepository.GetBookingById(id);
            booking.IsReturned = true;
            booking.ReturnDate = DateTime.Now;
            _bookingRepository.Update(booking);
            _bookingRepository.Save();
            return RedirectToAction("Details", "Booking", new {Id = id});

        }


        // GET: Bookings/Delete/5
        [CustomAuthorize(Roles = "Admin")]
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
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = _bookingRepository.GetBookingById(id);
            _bookingRepository.Delete(booking);
            _bookingRepository.Save();
            return RedirectToAction("Index", "Admin", null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bookingRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        private double CalculateBookingCost(DateTime startDate, DateTime endDate, int vehicleId)
        {
            TimeSpan dateRange = endDate - startDate;
            double rentalDays = dateRange.TotalDays;
            Vehicle vehicle = _vehicleRepository.GetVehicleById(vehicleId);
            double totalCost = vehicle.RentalCost * rentalDays;
            return Math.Round(totalCost,2);
        }

        private List<Booking> CheckConflictingBookings(Booking booking)
        {
            if(booking != null)
            {
                List<Booking> conflictingBookings = new List<Booking>();

                foreach (var existingBooking in _bookingRepository.GetBookings().Where(x => x.VehicleId == booking.VehicleId).ToList())
                {
                    if (existingBooking != null)
                    { 
                        if(booking.BookingStart <= existingBooking.BookingFinish && existingBooking.BookingStart <= booking.BookingFinish)
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
                List<Booking> conlictingBookingsWithExtra = new List<Booking>();
                List<ConflictingExtraItem> conflictingExtras = new List<ConflictingExtraItem>();

                foreach (var extraId in bookingVM.SelectedExtraIds)
                {
                    OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(extraId);
                    List<Booking> bookingsWithSelectedExtra = _bookingRepository.GetBookings().Where(x => x.OptionalExtras.Contains(optionalExtra)).ToList();

                    foreach(var bookingWithSelectedExtra in bookingsWithSelectedExtra)
                    {
                        if (bookingVM.booking.BookingStart <= bookingWithSelectedExtra.BookingFinish && bookingWithSelectedExtra.BookingStart <= bookingVM.booking.BookingFinish)
                        {
                            conflictingExtras.Add(new ConflictingExtraItem
                            {
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
