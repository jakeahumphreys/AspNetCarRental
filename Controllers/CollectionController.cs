using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.Models.ViewModels;
using EIRLSSAssignment1.RepeatLogic;
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
                RedirectToAction(null);
            }
            else
            {
                return null;
            }

            return View(captureDlViewModel);
        }


        public ActionResult ValidationFailed(ValidationFailReason failureReason)
        {
            var validationViewModel = new ValidationFailedViewModel
            {
                failReason = failureReason
            };

            return View(validationViewModel);
        }
    }
}