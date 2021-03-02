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

        public void ImportLicenses()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var importFileName = "dvla.csv";
            var importDirectory = Path.Combine(myDocuments, "BangerImports");
            var filePath = Path.Combine(importDirectory, importFileName);

            if (!Directory.Exists(importDirectory))
            {
                CreateDirectory(importDirectory);
            }

            if (File.Exists(filePath))
            {
                _drivingLicenseRepository.Truncate();

                //var csv = File.ReadAllLines(filePath).Skip(1);
                Regex CSVSplitter = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                foreach (var entry in File.ReadAllLines(filePath).Skip(1).Select(line => CSVSplitter.Split(line)))
                {
                    var license = new DrivingLicense
                    {
                        Id = 0,
                        LicenseNumber = entry[0],
                        FamilyName = entry[1],
                        Forenames = entry[2],
                        DateOfBirth = DateTime.Parse(entry[3]),
                        YearOfIssue = DateTime.Parse(entry[4]),
                        Expires = DateTime.Parse(entry[5]),
                        IssuingAuthority = entry[6],
                        Address = entry[7].Trim('"'),
                        Status = entry[8],
                        Date = DateTime.Parse(entry[9])
                    };

                    _drivingLicenseRepository.Insert(license);
                    _drivingLicenseRepository.Save();
                }

                File.Delete(filePath);
            }
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
            var existingLicense = _drivingLicenseRepository.GetDrivingLicenses()
                .SingleOrDefault(x => x.LicenseNumber == licenseNumber);

            if (existingLicense != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CustomerAppearsOnAbiImport(string familyName, string forenames, string address)
        {
            OleDbConnection connection = ConnectToAbiDatabase();

            try
            {
                connection.Open();
                var count = 0;
                OleDbCommand command =
                    new OleDbCommand(
                        $"SELECT COUNT(*) from fraudulent_claim_data WHERE FAMILY_NAME='{familyName}' AND FORENAMES='{forenames}' AND ADDRESS_OF_CLAIM='{address}'", connection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = (int) reader[0];
                }

                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        private OleDbConnection ConnectToAbiDatabase()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var importFileName = "ABI_DRIVER_FRAUD.accdb";
            var importDirectory = Path.Combine(myDocuments, "BangerImports");
            var filePath = Path.Combine(importDirectory, importFileName);

            OleDbConnection connection = new OleDbConnection();

            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + $@"Data source={filePath}";

            return connection;
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