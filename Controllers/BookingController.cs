using System.Web.Mvc;
using EIRLSSAssignment1.Customisations;
using EIRLSSAssignment1.Models.ViewModels;
using EIRLSSAssignment1.ServiceLayer;

namespace EIRLSSAssignment1.Controllers
{
    [HandleError]
    public class BookingController : Controller
    {
        private BookingService _bookingService;

        public BookingController()
        {
            _bookingService = new BookingService();
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_bookingService.GetIndex());
        }

        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_bookingService.GetDetails(id));
            }
            catch (ParameterNotValidException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (BookingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }

        }

        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Create()
        {
            try
            {
                return View(_bookingService.CreateView());
            }
            catch (UserIsBlacklistedException)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.Account });
            }
            catch (GarageIsClosedException)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.GarageClosed });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Create(BookingCreateViewModel bookingVM)
        {
            ServiceResponse response = _bookingService.CreateAction(bookingVM);

            if (response.Result == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin", null);
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }
            }
            else
            {
                return View(response.ServiceObject as BookingCreateViewModel);
            }

        }

        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_bookingService.EditView(id));
            }
            catch (ParameterNotValidException ex)
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
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Edit(BookingCreateViewModel bookingVM)
        {
            ServiceResponse response = _bookingService.EditAction(bookingVM);

            if (response.Result == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin", null);
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }
            }
            else
            {
                return View(response.ServiceObject as BookingCreateViewModel);
            }
        }

        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult ExtendBooking(int id)
        {
            try
            {
                return View(_bookingService.ExtendBookingView(id));
            }
            catch (ParameterNotValidException ex)
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
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult ExtendBooking(BookingCreateViewModel bookingVM)
        {
            ServiceResponse result = _bookingService.ExtendBookingAction(bookingVM);

            if(result.Result == true)
            {
                TempData["extensionRequest"] = result.ServiceObject;
                return RedirectToAction("Create", "ExtensionRequest");
            }
            else
            {
                return View(result.ServiceObject as BookingCreateViewModel);

            }
        }

        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult Return(int id)
        {
            try
            {
                return View(_bookingService.ReturnView(id));
            }
            catch (ParameterNotValidException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (BookingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "User,Admin")]
        public ActionResult ReturnConfirmed(int id)
        {
            var result = _bookingService.ReturnAction(id);

            if(result == true)
            {
                return RedirectToAction("Details", "Booking", new { Id = id });
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.System, message = "Error"});
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                return View(_bookingService.DeleteView(id));
            }
            catch (ParameterNotValidException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
            catch (BookingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.HTTP, message = ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _bookingService.DeleteAction(id);

            if (result == true)
            {
                return RedirectToAction("Index", "Admin", null);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = ErrorType.System, message = "Error" });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bookingService.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
