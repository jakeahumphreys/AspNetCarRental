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
using EIRLSSAssignment1.Customisations;
using EIRLSSAssignment1.ServiceLayer;

namespace EIRLSSAssignment1.Controllers
{
    public class ExtensionRequestController : Controller
    {

        private ExtensionRequestRepository _extensionRepository;
        private BookingRepository _bookingRepository;
        private ExtensionRequestService _extensionRequestService;

        public ExtensionRequestController()
        {
            _extensionRepository = new ExtensionRequestRepository(new ApplicationDbContext());
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
            _extensionRequestService = new ExtensionRequestService();
        }

        // GET: ExtensionRequest/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_extensionRequestService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (DrivingLicenseNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            ExtensionRequest extensionRequest = TempData["extensionRequest"] as ExtensionRequest;

            ServiceResponse response = _extensionRequestService.CreateAction(extensionRequest);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { @errorType = ErrorType.Extension });
            }

        }

        public ActionResult Approve(bool approved, int extensionId)
        {
            ServiceResponse response = _extensionRequestService.ApproveExtension(approved, extensionId);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { @errorType = ErrorType.Extension });
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _extensionRequestService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
