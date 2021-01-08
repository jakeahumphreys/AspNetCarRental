using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.ServiceLayer;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Customisations;
using System;

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ConfigurationController : Controller
    {
        private ConfigurationService _configurationService;

        public ConfigurationController()
        {
            _configurationService = new ConfigurationService();
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(_configurationService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (BookingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Configuration configuration)
        {
            ServiceResponse response = _configurationService.CreateAction(configuration);

            if (response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.System, message = "Error" });
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(_configurationService.EditView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (BookingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Configuration configuration)
        {
            ServiceResponse response = _configurationService.EditAction(configuration);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                return View(_configurationService.DeleteView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (ConfigurationNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceResponse response = _configurationService.DeleteAction(id);

            if(response.Result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _configurationService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
