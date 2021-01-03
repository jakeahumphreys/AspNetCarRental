using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1.Controllers
{
    public class AdminController : Controller
    {
        private ConfigurationRepository _configurationRepository;

        public AdminController()
        {
            _configurationRepository = new ConfigurationRepository(new ApplicationDbContext());
        }

        // GET: Admin
        public ActionResult Index()
        {
            List<Configuration> configurations = _configurationRepository.GetConfigurations().ToList();


            var adminVM = new AdminViewModel {Configurations = configurations};


            return View(adminVM);
        }
    }
}