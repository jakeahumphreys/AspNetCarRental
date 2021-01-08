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
using MVCWebAssignment1.Customisations;

namespace EIRLSSAssignment1.Controllers
{
    public class ExtensionRequestController : Controller
    {

        private ExtensionRequestRepository _extensionRepository;
        private BookingRepository _bookingRepository;

        public ExtensionRequestController()
        {
            _extensionRepository = new ExtensionRequestRepository(new ApplicationDbContext());
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
        }

        // GET: ExtensionRequest/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtensionRequest extensionRequest = _extensionRepository.GetExtensionRequestById(id);
            if (extensionRequest == null)
            {
                return HttpNotFound();
            }
            return View(extensionRequest);
        }

        public ActionResult Create()
        {
            ExtensionRequest extensionRequest = TempData["extensionRequest"] as ExtensionRequest;

            if (extensionRequest != null)
            {

                _extensionRepository.Insert(extensionRequest);
                _extensionRepository.Save();
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { @errorType = ErrorType.Extension });
            }
        }

        public ActionResult Approve(bool approved, int extensionId)
        {

            ExtensionRequest extension = _extensionRepository.GetExtensionRequestById(extensionId);
            
            if(extension != null)
            {
                Booking booking = _bookingRepository.GetBookingById(extension.BookingId);

                if(booking != null)
                {
                    if (approved == true)
                    {
                        booking.BookingFinish = extension.EndDateRequest;
                        _bookingRepository.Update(booking);
                        _bookingRepository.Save();

                        extension.extensionRequestStatus = ExtensionStatus.Accepted;
                        _extensionRepository.Update(extension);
                        _extensionRepository.Save();

                        return RedirectToAction("Admin", "Index", null);
                    }
                    else
                    {
                        extension.extensionRequestStatus = ExtensionStatus.Rejected;
                        _extensionRepository.Update(extension);
                        _extensionRepository.Save();
                        return RedirectToAction("Admin", "Index", null);

                    }
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { @errorType = ErrorType.Extension });
                }
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
                _extensionRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
