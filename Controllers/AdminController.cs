using EIRLSSAssignment1.ServiceLayer;
using EIRLSSAssignment1.Common;
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
    }
}