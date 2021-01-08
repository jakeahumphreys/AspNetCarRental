﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Models.ViewModels;
using EIRLSSAssignment1.Models.enums;
using EIRLSSAssignment1.RepeatLogic;
using EIRLSSAssignment1.RepeatLogic.Objects;
using Microsoft.AspNet.Identity;
using MVCWebAssignment1.Customisations;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class BookingService
    {




        private BookingRepository _bookingRepository;
        private VehicleRepository _vehicleRepository;
        private OptionalExtraRepository _optionalExtraRepository;
        private ExtensionRequestRepository _extensionRequestRepository;
        private ApplicationDbContext appDbContext = new ApplicationDbContext();
        private Library _library;

        public BookingService()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(context);
            _vehicleRepository = new VehicleRepository(context);
            _optionalExtraRepository = new OptionalExtraRepository(context);
            _extensionRequestRepository = new ExtensionRequestRepository(new ApplicationDbContext());

            _library = new Library();
        }

        public IList<Booking> GetIndex()
        {
            var httpContext = HttpContext.Current;

            var userId = httpContext.User.Identity.GetUserId();

            if (httpContext.User.IsInRole("Admin"))
            {
                return _bookingRepository.GetBookings().ToList();
            }
            else
            {
                return _bookingRepository.GetBookings().Where(x => x.UserId == userId).ToList();
            }
        }

        public BookingViewModel GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException();
            }
            Booking booking = _bookingRepository.GetBookingById(id);

            if (booking == null)
            {
                throw new HttpException(404, "Booking not found");
            }

            var bookingVM = new BookingViewModel { booking = booking };

            List<OptionalExtra> bookedOptionalExtras = booking.OptionalExtras.ToList();

            if (bookedOptionalExtras != null)
            {
                bookingVM.BookedOptionalExtras = bookedOptionalExtras;
            }

            return bookingVM;
        }

        public BookingCreateViewModel CreateView()
        {
            //Handle preventing users from creating bookings based on conditions
            if (_library.IsGarageAllowingOrders())
            {
                var httpContext = HttpContext.Current;

                var userId = httpContext.User.Identity.GetUserId();
                //if autotrust is enabled, send userid to be judged for promotion.
                _library.HandleAutoTrust(userId);

                BookingCreateViewModel createBookingVM = new BookingCreateViewModel();

                createBookingVM.IsTrustedUser = _library.CanUserReturnLate(userId);

                if (!_library.IsUserBlacklisted(userId))
                {
                    var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age

                    var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge).Where(x => x.IsInactive == false), "Id", "DisplayString");

                    var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.IsInactive == false), "Id", "DisplayString");

                    //Set Viewbag Vehicle Data
                    createBookingVM.Vehicles = vehicles;
                    createBookingVM.VehicleCount = vehicles.Count();

                    //Set Viewbag Optional Extra Data
                    createBookingVM.OptionalExtras = optionalExtras;
                    createBookingVM.OptionalExtraCount = optionalExtras.Count();

                    createBookingVM.Users = new SelectList(appDbContext.Users.ToList(), "Id", "Name");


                    return createBookingVM;
                }
                else
                {
                    //user is blacklisted
                    throw new Exception("User is blacklisted");
                }

            }
            else
            {
                //Garage is not open for orders
                throw new Exception("Garage is closed");
            }
        }

        public bool CreateAction(BookingCreateViewModel bookingVM)
        {
            var httpContext = HttpContext.Current;

            var userId = httpContext.User.Identity.GetUserId();

            var userAge = _library.CalculateUserAge(userId); //Calculate users age from their dob for minimum rental age
            var vehicles = new SelectList(_vehicleRepository.GetVehicles().Where(x => x.MinimumAgeToRent <= userAge).Where(x => x.IsInactive == false), "Id", "DisplayString");
            var optionalExtras = new MultiSelectList(_optionalExtraRepository.GetOptionalExtras().Where(x => x.IsInactive == false), "Id", "DisplayString");

            //Set Viewbag Vehicle Data
            bookingVM.Vehicles = vehicles;
            bookingVM.VehicleCount = vehicles.Count();

            //Set Viewbag Optional Extra Data
            bookingVM.OptionalExtras = optionalExtras;
            bookingVM.OptionalExtraCount = optionalExtras.Count();

            DateTime startTime = new DateTime(bookingVM.StartDate.Year, bookingVM.StartDate.Month, bookingVM.StartDate.Day, bookingVM.StartDateTime.Hour, bookingVM.StartDateTime.Minute, bookingVM.StartDateTime.Second);
            DateTime endTime = new DateTime(bookingVM.EndDate.Year, bookingVM.EndDate.Month, bookingVM.EndDate.Day, bookingVM.EndDateTime.Hour, bookingVM.EndDateTime.Minute, bookingVM.EndDateTime.Second);

            bookingVM.booking.BookingStart = startTime;
            bookingVM.booking.BookingFinish = endTime;

            BookingErrorObj errorObj = new BookingErrorObj();

            //Create and store values in an error object
            errorObj = CreateBookingErrorObject(bookingVM);

            //Validate created error object and perform booking if valid
            if (ValidateErrorObject(errorObj) == true)
            {
                //Add Optional Extras to bookings if any were selected
                if (bookingVM.SelectedExtraIds != null && bookingVM.SelectedExtraIds.Count > 0)
                {
                    var bookedOptionalExtras = new List<OptionalExtra>();

                    foreach (var id in bookingVM.SelectedExtraIds)
                    {
                        bookedOptionalExtras.Add(_optionalExtraRepository.GetOptionalExtraById(id));
                    }

                    bookingVM.booking.OptionalExtras = bookedOptionalExtras;

                }

                //Attribute the user ID automatically if they aren't an admin.
                //Admins set the user manually to create bookings for others.
                if (!httpContext.User.IsInRole("Admin"))
                {
                    bookingVM.booking.UserId = httpContext.User.Identity.GetUserId();
                }

                //Calculate the booking cost.
                bookingVM.booking.BookingCost = CalculateBookingCost(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish, bookingVM.booking.VehicleId);

                //Save booking
                _bookingRepository.Insert(bookingVM.booking);
                _bookingRepository.Save();

                //Redirect to appropriate place.
                return true;
            }
            else
            {
                //An error was recorded within the object, pass to viewbag for display and return view.
                bookingVM.ErrorObj = errorObj;
                return false;
            }

        }

        public BookingErrorObj CreateBookingErrorObject(BookingCreateViewModel bookingVM)
        {
            var httpContext = HttpContext.Current;
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
                if (_library.isMinRental(bookingVM.booking.BookingStart, bookingVM.booking.BookingFinish) == false)
                {
                    errorObj.isBelowMinRental = true;
                }
                if (_library.isBeforeOpening(bookingVM.booking.BookingStart.Hour) == true)
                {
                    errorObj.isStartBeforeOpen = true;
                }
                if (_library.isBeforeOpening(bookingVM.booking.BookingFinish.Hour) == true)
                {
                    errorObj.isEndBeforeOpen = true;
                }
                if (_library.isBeyondClosing(bookingVM.booking.BookingFinish.Hour) == true)
                {
                    errorObj.isEndAfterClose = true;
                }
                if (_library.isBeyondClosing(bookingVM.booking.BookingStart.Hour) == true)
                {
                    errorObj.isStartAfterClose = true;
                }
                if (bookingVM.booking.BookingFinish < bookingVM.booking.BookingStart)
                {
                    errorObj.isEndBeforeStart = true;
                }
                if (bookingVM.booking.BookingStart > bookingVM.booking.BookingFinish)
                {
                    errorObj.isStartAfterEnd = true;
                }
                if (!httpContext.User.IsInRole("Admin"))
                {
                    if (bookingVM.booking.BookingStart < DateTime.Today)
                    {
                        errorObj.isInThePast = true;
                    }
                }
                if (conflictingOptionalExtras.Count > 0)
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
            if (errorObj != null)
            {
                if (errorObj.isBookedNextDay == true)
                {
                    return false;
                }
                if (errorObj.isBelowMinRental == true)
                {
                    return false;
                }
                if (errorObj.isBeyondMaxRental == true)
                {
                    return false;
                }
                if (errorObj.isStartBeforeOpen == true)
                {
                    return false;
                }
                if (errorObj.isStartAfterClose == true)
                {
                    return false;
                }
                if (errorObj.isEndBeforeOpen == true)
                {
                    return false;
                }
                if (errorObj.isEndAfterClose == true)
                {
                    return false;
                }
                if (errorObj.isInThePast == true)
                {
                    return false;
                }
                if (errorObj.isEndBeforeOpen == true)
                {
                    return false;
                }
                if (errorObj.isStartAfterEnd == true)
                {
                    return false;
                }
                if (errorObj.conflictingBookings != null)
                {
                    return false;
                }
                if (errorObj.conflictingOptionalExtras != null)
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
            return Math.Round(totalCost, 2);
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
                        if (booking.BookingStart <= existingBooking.BookingFinish && existingBooking.BookingStart <= booking.BookingFinish)
                        {
                            conflictingBookings.Add(existingBooking);
                        }
                    }
                }

            }

            return conflictingBookings;

        }

        private List<ConflictingExtraItem> CheckConflictingOptionalExtras(BookingCreateViewModel bookingVM)
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