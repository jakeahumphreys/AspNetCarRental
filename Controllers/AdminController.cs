using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using MVCWebAssignment1.Customisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ConfigurationRepository _configurationRepository;
        private VehicleRepository _vehicleRepository;
        private VehicleTypeRepository _vehicleTypeRepository;
        private FuelTypeRepository _fuelTypeRepository;
        private ApplicationDbContext _appDbContext;
        private BookingRepository _bookingRepository;
        private OptionalExtraRepository _optionalExtraRepository;
        private DrivingLicenseRepository _drivingLicenseRepository;
        private SupportingDocumentRepository _supportingDocumentRepository;
        private ExtensionRequestRepository _extentionRepository;


       
        public AdminController()
        {
            _configurationRepository = new ConfigurationRepository(new ApplicationDbContext());
            _vehicleRepository = new VehicleRepository(new ApplicationDbContext());
            _vehicleTypeRepository = new VehicleTypeRepository(new ApplicationDbContext());
            _fuelTypeRepository = new FuelTypeRepository(new ApplicationDbContext());
            _appDbContext = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
            _optionalExtraRepository = new OptionalExtraRepository(new ApplicationDbContext());
            _drivingLicenseRepository = new DrivingLicenseRepository(new ApplicationDbContext());
            _supportingDocumentRepository = new SupportingDocumentRepository(new ApplicationDbContext());
            _extentionRepository = new ExtensionRequestRepository(new ApplicationDbContext());

        }

        // GET: Admin
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Configuration> configurations = _configurationRepository.GetConfigurations().ToList();
            List<Vehicle> vehicles = _vehicleRepository.GetVehicles().ToList();
            List<VehicleType> vehicleTypes = _vehicleTypeRepository.GetVehicleTypes().ToList();
            List<FuelType> fuelTypes = _fuelTypeRepository.GetFuelTypes().ToList();
            List<ApplicationUser> applicationUsers = _appDbContext.Users.ToList();
            List<Booking> bookings = _bookingRepository.GetBookings().ToList();
            List<OptionalExtra> optionalExtras = _optionalExtraRepository.GetOptionalExtras().ToList();
            List<ExtensionRequest> extensionRequests = _extentionRepository.GetExtensionRequests().ToList();


            var adminVM = new AdminViewModel {
                Configurations = configurations, 
                Vehicles = vehicles,
                VehicleTypes = vehicleTypes,
                FuelTypes = fuelTypes,
                Users = applicationUsers,
                Bookings = bookings,
                OptionalExtras = optionalExtras,
                ExtensionRequests = extensionRequests
            };


            return View(adminVM);
        }



        [CustomAuthorize(Roles = "Admin")]
        public ActionResult DrivingLicenses()
        {
            return View(_drivingLicenseRepository.GetDrivingLicenses());
        }
    }
}