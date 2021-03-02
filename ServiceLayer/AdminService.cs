using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.RepeatLogic;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class AdminService
    {
        private ConfigurationRepository _configurationRepository;
        private VehicleRepository _vehicleRepository;
        private VehicleTypeRepository _vehicleTypeRepository;
        private FuelTypeRepository _fuelTypeRepository;
        private ApplicationDbContext _appDbContext;
        private BookingRepository _bookingRepository;
        private OptionalExtraRepository _optionalExtraRepository;
        private DrivingLicenseRepository _drivingLicenseRepository;
        private ExtensionRequestRepository _extentionRepository;

        public AdminService()
        {
            _configurationRepository = new ConfigurationRepository(new ApplicationDbContext());
            _vehicleRepository = new VehicleRepository(new ApplicationDbContext());
            _vehicleTypeRepository = new VehicleTypeRepository(new ApplicationDbContext());
            _fuelTypeRepository = new FuelTypeRepository(new ApplicationDbContext());
            _appDbContext = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
            _optionalExtraRepository = new OptionalExtraRepository(new ApplicationDbContext());
            _drivingLicenseRepository = new DrivingLicenseRepository(new ApplicationDbContext());
            _extentionRepository = new ExtensionRequestRepository(new ApplicationDbContext());
        }

        public AdminViewModel GetIndex()
        {
            List<Configuration> configurations = _configurationRepository.GetConfigurations().ToList();
            List<Vehicle> vehicles = _vehicleRepository.GetVehicles().ToList();
            List<VehicleType> vehicleTypes = _vehicleTypeRepository.GetVehicleTypes().ToList();
            List<FuelType> fuelTypes = _fuelTypeRepository.GetFuelTypes().ToList();
            List<ApplicationUser> applicationUsers = _appDbContext.Users.ToList();
            List<Booking> bookings = _bookingRepository.GetBookings().ToList();
            List<OptionalExtra> optionalExtras = _optionalExtraRepository.GetOptionalExtras().ToList();
            List<ExtensionRequest> extensionRequests = _extentionRepository.GetExtensionRequests().ToList();


            var adminVM = new AdminViewModel
            {
                Configurations = configurations,
                Vehicles = vehicles,
                VehicleTypes = vehicleTypes,
                FuelTypes = fuelTypes,
                Users = applicationUsers,
                Bookings = bookings,
                OptionalExtras = optionalExtras,
                ExtensionRequests = extensionRequests,
                PendingCollections = bookings.Where(x=>x.BookingStatus == BookingStatus.Reserved).ToList()
            };

            return adminVM;
        }
    }
}