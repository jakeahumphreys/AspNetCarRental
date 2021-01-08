using EIRLSSAssignment1.ServiceLayer;
using EIRLSSAssignment1.Customisations;
using System.Web.Mvc;

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private AdminService _adminService;
        
        public AdminController()
        {
            _adminService = new AdminService();
        }

        // GET: Admin
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_adminService.GetIndex());
        }



        [CustomAuthorize(Roles = "Admin")]
        public ActionResult DrivingLicenses()
        {
            return View(_adminService.GetDrivingLicenses());
        }
    }
}