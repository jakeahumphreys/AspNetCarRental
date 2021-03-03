using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity;
using EIRLSSAssignment1.Customisations;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.UI;
using EIRLSSAssignment1.Models.ViewModels;
using EIRLSSAssignment1.RepeatLogic;
using Microsoft.Owin.Security.Facebook;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class DocumentValidationService
    {
        private readonly DrivingLicenseRepository _drivingLicenseRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly Library _library;

        public DocumentValidationService()
        {
            _drivingLicenseRepository = new DrivingLicenseRepository(new ApplicationDbContext());
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
            _library = new Library();
            _applicationDbContext = new ApplicationDbContext();
        }

        public ServiceResponse CaptureDrivingLicense(CaptureLicenseViewModel captureLicenseViewModel)
        {
            if (captureLicenseViewModel == null)
            {
                return new ServiceResponse {Result = false, ResponseError = ResponseError.NullParameter};
            }

            var booking = _bookingRepository.GetBookingById(captureLicenseViewModel.BookingId);

            if (booking == null)
            {
                return new ServiceResponse {Result = false, ResponseError = ResponseError.EntityNotFound};
            }

            if (CustomerAppearsOnDvlaImport(captureLicenseViewModel.License.LicenseNumber))
            {
                BlacklistUser(booking.UserId);
                SetBookingStatus(BookingStatus.Rejected, booking);

                return new ServiceResponse {Result = false, ResponseError = ResponseError.ValidationFailed};
            }

            booking.DrivingLicenseImage = _library.convertImageToByteArray(captureLicenseViewModel.LicenseImage);

            _bookingRepository.Update(booking);
            _bookingRepository.Save();

            return new ServiceResponse {Result = true};
        }

        public ServiceResponse CaptureSupportingDocument()
        {
            return null;
        }

        public bool CustomerAppearsOnDvlaImport(string licenseNumber)
        {
            return false;

        }

        public bool CustomerAppearsOnAbiImport(string familyName, string forenames, string address)
        {
            return false;
        }


        private void CreateDirectory(string filePath)
        {
            Directory.CreateDirectory(filePath);
        }

        private void BlacklistUser(string userId)
        {
            var user = _applicationDbContext.Users.Find(userId);

            user.IsBlackListed = true;

            _applicationDbContext.Entry(user).State = EntityState.Modified;
            _applicationDbContext.SaveChanges();
        }

        private void SetBookingStatus(BookingStatus bookingStatus , Booking booking)
        {
            booking.BookingStatus = bookingStatus;
            _bookingRepository.Update(booking);
            _bookingRepository.Save();
        }
    }
}