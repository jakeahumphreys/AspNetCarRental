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
using EIRLSSAssignment1.Models.enums;
using EIRLSSAssignment1.Models.ViewModels;
using EIRLSSAssignment1.RepeatLogic;
using EIRLSSAssignment1.RepeatLogic.Objects;
using EIRLSSAssignment1.ServiceLayer;
using Microsoft.AspNet.Identity;
using MVCWebAssignment1.Customisations;

namespace EIRLSSAssignment1.Controllers
{
    [HandleError]
    public class BookingController : Controller
    {
        private BookingRepository _bookingRepository;
        private VehicleRepository _vehicleRepository;
        private OptionalExtraRepository _optionalExtraRepository;
        private ExtensionRequestRepository _extensionRequestRepository;
        private ApplicationDbContext appDbContext = new ApplicationDbContext();
        private Library _library;

        private BookingService _bookingService;

        public BookingController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(context);
            _vehicleRepository = new VehicleRepository(context);
            _optionalExtraRepository = new OptionalExtraRepository(context);
            _extensionRequestRepository = new ExtensionRequestRepository(new ApplicationDbContext());
            
            _library = new Library();

            _bookingService = new BookingService();
        }

        // GET: Bookings
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_bookingService.GetIndex());
        }

        // GET: Bookings/Details/5
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_bookingService.GetDetails(id));
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }

        }

        // GET: Bookings/Create
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Create()
        {
            return View(_bookingService.CreateView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Create(BookingCreateViewModel bookingVM)
        {
            var result = _bookingService.CreateAction(bookingVM);

            if(result == true)
            {
                if(User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin", null);
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }
            }
            else
            {
                return View(bookingVM);
            }
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

            ViewBag.IsTrustedUser = _library.CanUserReturnLate(userId);

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
            ViewBag.HasPendingRequest = HasPendingRequest(booking.Id);

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
                DateTime StartDate = new DateTime(bookingVM.StartDate.Year, bookingVM.StartDate.Month, bookingVM.StartDate.Day, bookingVM.StartDateTime.Hour, bookingVM.StartDateTime.Minute, bookingVM.StartDateTime.Second);
                DateTime EndDate = new DateTime(bookingVM.EndDate.Year, bookingVM.EndDate.Month, bookingVM.EndDate.Day, bookingVM.EndDateTime.Hour, bookingVM.EndDateTime.Minute, bookingVM.EndDateTime.Second);

                bookingVM.booking.BookingStart = StartDate;
                bookingVM.booking.BookingFinish = EndDate;

                Booking existingBooking = _bookingRepository.GetBookingById(bookingVM.booking.Id);
                ViewBag.HasPendingRequest = HasPendingRequest(bookingVM.booking.Id);

                BookingErrorObj errorObj = new BookingErrorObj();

                //If user has requested a changed date
                if (bookingVM.booking.BookingFinish != existingBooking.BookingFinish)
                {
                    errorObj = CreateBookingErrorObject(bookingVM);

                    if(ValidateErrorObject(errorObj) == true)
                    {
                        ExtensionRequest request = new ExtensionRequest {
                            BookingId = bookingVM.booking.Id,
                            EndDateRequest = bookingVM.booking.BookingFinish,
                            extensionRequestStatus = ExtensionStatus.Pending
                        };
                        TempData["extensionRequest"] = request;

                        return RedirectToAction("Create", "ExtensionRequest");
                    }
                    else
                    {
                        ViewBag.ErrorObj = errorObj;
                        return View(bookingVM);
                    }

                }
            }

            return View();
        }

        public bool HasPendingRequest(int bookingId)
        {
            bool HasRequest = false;

            if(bookingId != 0)
            {
                List<ExtensionRequest> extensions = _extensionRequestRepository.GetExtensionRequests().Where(x => x.extensionRequestStatus == ExtensionStatus.Pending && x.BookingId == bookingId).ToList();
                if(extensions.Count > 0)
                {
                    HasRequest = true;
                }
            }

            return HasRequest;
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

        public BookingErrorObj CreateBookingErrorObject(BookingViewModel bookingVM)
        {
            BookingErrorObj errorObj = new BookingErrorObj();
            List<Booking> conflictingBookings = CheckConflictingBookings(bookingVM.booking);
            List<ConflictingExtraItem> conflictingOptionalExtras = CheckConflictingOptionalExtras(bookingVM);


            if (bookingVM != null)
            {
                if (_library.isBookedNextDay(bookingVM.booking.BookingFinish, bookingVM.booking.VehicleId) == true)
                {
                    errorObj.isBookedNextDay = true;
                }
                if (_library.isMaxRental(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish) == true)
                {
                    errorObj.isBeyondMaxRental = true;
                }
                if(_library.isMinRental(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish) == false)
                {
                    errorObj.isBelowMinRental = true;
                }
                if (_library.isBeforeOpening(bookingVM.booking.BookingStart.Hour) == true)
                {
                    errorObj.isStartBeforeOpen = true;
                }
                if(_library.isBeforeOpening(bookingVM.booking.BookingFinish.Hour) == true)
                {
                    errorObj.isEndBeforeOpen = true;
                }
                if (_library.isBeyondClosing(bookingVM.booking.BookingFinish.Hour) == true)
                {
                    errorObj.isEndAfterClose = true;
                }
                if(_library.isBeyondClosing(bookingVM.booking.BookingStart.Hour) == true)
                {
                    errorObj.isStartAfterClose = true;
                }
                if(bookingVM.booking.BookingFinish < bookingVM.booking.BookingStart)
                {
                    errorObj.isEndBeforeStart = true;
                }
                if(bookingVM.booking.BookingStart > bookingVM.booking.BookingFinish)
                {
                    errorObj.isStartAfterEnd = true;
                }
                if(!User.IsInRole("Admin"))
                {
                    if(bookingVM.booking.BookingStart < DateTime.Today)
                    {
                        errorObj.isInThePast = true;
                    }
                }
                if(conflictingOptionalExtras.Count > 0)
                {
                    errorObj.conflictingOptionalExtras = conflictingOptionalExtras;
                }
                if (conflictingBookings.Count > 0)
                {
                    errorObj.conflictingBookings = conflictingBookings;
                }
            }
            else
            {
                return null;
            }

            return errorObj;

        }

        public bool ValidateErrorObject(BookingErrorObj errorObj)
        {
            if(errorObj != null)
            {
                if(errorObj.isBookedNextDay == true)
                {
                    return false;
                }
                if(errorObj.isBelowMinRental == true)
                {
                    return false;
                }
                if(errorObj.isBeyondMaxRental == true)
                {
                    return false;
                }
                if(errorObj.isStartBeforeOpen == true)
                {
                    return false;
                }
                if(errorObj.isStartAfterClose == true)
                {
                    return false;
                }
                if(errorObj.isEndBeforeOpen == true)
                {
                    return false;
                }
                if(errorObj.isEndAfterClose == true)
                {
                    return false;
                }
                if(errorObj.isInThePast == true)
                {
                    return false;
                }
                if(errorObj.isEndBeforeOpen == true)
                {
                    return false;
                }
                if(errorObj.isStartAfterEnd == true)
                {
                    return false;
                }
                if(errorObj.conflictingBookings != null)
                {
                    return false;
                }
                if(errorObj.conflictingOptionalExtras != null)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
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
            List<Booking> conflictingBookings = new List<Booking>();

            if (booking != null)
            {
                foreach (var existingBooking in _bookingRepository.GetBookings().Where(x => x.VehicleId == booking.VehicleId && x.IsReturned == false && x.Id != booking.Id).ToList())
                {
                    if (existingBooking != null)
                    { 
                        if(booking.BookingStart <= existingBooking.BookingFinish && existingBooking.BookingStart <= booking.BookingFinish)
                        {
                            conflictingBookings.Add(existingBooking);
                        }
                    }
                }

            }

            return conflictingBookings;

        }

        private List<ConflictingExtraItem> CheckConflictingOptionalExtras(BookingViewModel bookingVM)
        {
            List<Booking> conlictingBookingsWithExtra = new List<Booking>();
            List<ConflictingExtraItem> conflictingExtras = new List<ConflictingExtraItem>();

            if (bookingVM != null && bookingVM.SelectedExtraIds != null)
            {
                foreach (var extraId in bookingVM.SelectedExtraIds)
                {
                    OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(extraId);
                    List<Booking> bookingsWithSelectedExtra = _bookingRepository.GetBookings().Where(x => x.OptionalExtras.Contains(optionalExtra) && x.IsReturned == false).ToList();

                    foreach (var bookingWithSelectedExtra in bookingsWithSelectedExtra)
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
            }
            return conflictingExtras;
        }
    }
}
