using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.Common;
using EIRLSSAssignment1.Models.ViewModels;
using EIRLSSAssignment1.Common;
using EIRLSSAssignment1.ServiceLayer;

namespace EIRLSSAssignment1.Controllers
{
    public class CollectionController : Controller
    {

        private readonly DocumentValidationService _documentValidationService;

        public CollectionController()
        {
            _documentValidationService = new DocumentValidationService();
        }

        public ActionResult CaptureLicense(int bookingId)
        {
            var captureDlVieWModel = new CaptureLicenseViewModel
            {
                BookingId = bookingId
            };

            return View(captureDlVieWModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaptureLicense(CaptureLicenseViewModel captureDlViewModel)
        {
            var result = _documentValidationService.CaptureDrivingLicense(captureDlViewModel);

            if (result.Result == true)
            {
                return RedirectToAction("CaptureDocument", "Collection", new {bookingId = captureDlViewModel.BookingId});
            }
            else
            {
                if (result.ResponseError == ResponseError.NullParameter)
                {
                    return RedirectToAction("Error", "Error", new { errorType = ErrorType.System, message = "Driving License model was null" });
                }

                if (result.ResponseError == ResponseError.EntityNotFound)
                {
                    return RedirectToAction("Error", "Error", new { errorType = ErrorType.System, message = "The booking was not found during license validation." });
                }

                return RedirectToAction("ValidationFailed", "Collection", new { failureReason = ValidationFailReason.LicenseValidationFailed});

            }
        }

        public ActionResult CaptureDocument(int bookingId)
        {
            var captureDocViewModel = new CaptureDocumentViewModel
            {
                BookingId = bookingId
            };

            return View(captureDocViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaptureDocument(CaptureDocumentViewModel captureDocViewModel)
        {
            var result = _documentValidationService.CaptureSupportingDocument(captureDocViewModel);

            if (result.Result == true)
            {
                return RedirectToAction("Complete", "Collection");
            }
            else
            {
                if (result.ResponseError == ResponseError.NullParameter)
                {
                    return RedirectToAction("Error", "Error", new { errorType = ErrorType.System, message = "Driving License model was null" });
                }

                if (result.ResponseError == ResponseError.EntityNotFound)
                {
                    return RedirectToAction("Error", "Error", new { errorType = ErrorType.System, message = "The booking was not found during license validation." });
                }

                return RedirectToAction("ValidationFailed", "Collection", new { failureReason = ValidationFailReason.DocumentValidationFailed });
            }
        }


        public ActionResult ValidationFailed(ValidationFailReason failureReason)
        {
            var validationViewModel = new ValidationFailedViewModel
            {
                failReason = failureReason
            };

            return View(validationViewModel);
        }

        public ActionResult Complete()
        {
            return View();
        }
    }
}